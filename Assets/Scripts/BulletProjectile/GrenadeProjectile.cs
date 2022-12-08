using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeProjectile : MonoBehaviour
{
    private delegate IEnumerator ExplodeBehavior();
    private ExplodeBehavior Explode ;
    private float damage;
    
    private Rigidbody GrenadeRigidBody;

    public GrenadeData.BombType bombType;
    private float lifeTime;
    private float ExplodeTime;
    private float explodeRadius;

    [SerializeField] private GameObject explodeEffect;
    [SerializeField] private GameObject smokeEffect;
    [SerializeField] private GameObject decoyEffect;
    private float explodeForce = 100f;

    public float speed = 10f;
    //private bool CollideGround = false;

    // Start is called before the first frame update
    void Start()
    {
        ParticleSystem ps = smokeEffect.GetComponent<ParticleSystem>();
        var main = ps.main;
        main.startLifetime = ExplodeTime;
       if (GrenadeRigidBody != null) {
            // **** cant find main cam ****
            //GrenadeRigidBody.AddForce(transform.forward *32f,ForceMode.Impulse); 
            switch (bombType) {
            case GrenadeData.BombType.BOMB :
                Explode = ExplodeBomb;
                break;
            case GrenadeData.BombType.FIRE :
                Explode = ExplodeFire;
                break;
            case GrenadeData.BombType.SMOKE :
                Explode = ExplodeSmoke;
                break;
            case GrenadeData.BombType.DECOY :
                Explode = ExplodeDecoy;
                break;
        }
            StartCoroutine(waitToDestroy());
            StartCoroutine(Explode());     
        }
    }
    private void Awake()
    {   
        GrenadeRigidBody = GetComponent<Rigidbody>();

    }
    // private void OnCollisionEnter(Collision other) {
    //     if (other.gameObject.tag == "Ground") {
    //          GrenadeRigidBody.velocity = new Vector3(0,0,0);
    //          CollideGround = true;
    //     }
    // }
    void update() {
        Debug.Log(GrenadeRigidBody.velocity);
    }
    public void Throw(Vector3 aim) {
       //GrenadeRigidBody.AddForce(transform.forward *32f,ForceMode.Impulse);
       //Debug.Log(aim.x + " " + aim.y + " " + aim.z);
       GrenadeRigidBody.velocity =  aim  * speed;
       //Debug.Log(aim.x + " " + aim.y + " " + aim.z);   
    }
    IEnumerator waitToDestroy()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }
    IEnumerator ExplodeBomb()
    {
        yield return new WaitForSeconds(lifeTime);
        Instantiate(explodeEffect, this.transform.position, Quaternion.identity);
        Collider[] colliders = Physics.OverlapSphere(transform.position, explodeRadius);

        foreach (Collider nearbyObject in colliders)
        {
            PlayerHitbox entityHit = nearbyObject.GetComponent<PlayerHitbox>();
            if (entityHit != null)
            {
                //Debug.Log(entityHit);
                entityHit.TakeDamage(damage);
            }
            EnemyHitbox enemyHit = nearbyObject.GetComponent<EnemyHitbox>();
            if (enemyHit != null)
            {
                //Debug.Log(entityHit);
                enemyHit.TakeDamage(damage);
            }
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explodeForce, transform.position, explodeRadius);
            }
        }       
    }
    

    IEnumerator ExplodeFire() {
        while(true) {
            yield return new WaitForSeconds(ExplodeTime);
            Instantiate(explodeEffect, this.transform.position, Quaternion.identity);
            Collider[] colliders = Physics.OverlapSphere(transform.position, explodeRadius);

            foreach (Collider nearbyObject in colliders) {
                PlayerHitbox entityHit = nearbyObject.GetComponent<PlayerHitbox>();
                if (entityHit != null)
                {
                entityHit.TakeDamage(damage);
                }
            }
        }
        
    }
    IEnumerator ExplodeSmoke() {
        yield return new WaitForSeconds(lifeTime);
        GameObject grenade = Instantiate(smokeEffect, this.transform.position, Quaternion.identity);
        grenade.transform.localScale = new Vector3(explodeRadius * 2, explodeRadius * 2, explodeRadius * 2);
        Destroy(grenade, ExplodeTime);
    }

    IEnumerator ExplodeDecoy() {
        yield return new WaitForSeconds(lifeTime);
        Instantiate(decoyEffect, this.transform.position, Quaternion.identity);
        Collider[] colliders = Physics.OverlapSphere(transform.position, explodeRadius);

        foreach (Collider nearbyObject in colliders)
        {
            EnemyBase entityHit = nearbyObject.GetComponent<EnemyBase>();
            if (entityHit != null) {
                Debug.Log(nearbyObject.tag);
                entityHit.NoiseAlert(transform.position);
            }
        }     
    }
    public void SetFloatData(List<float> ld)
    {
        damage = ld[0];  ExplodeTime = ld[1]; explodeRadius = ld[2]; 
    }
    public void SetLifeTime(float lt) {
        lifeTime = lt;
    }
    public void Setbombtype(GrenadeData.BombType bt) {
        //Debug.Log(bt);
        bombType = bt;
    }
}
