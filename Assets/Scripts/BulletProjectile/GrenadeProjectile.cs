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

    [SerializeField] private GameObject fireEffect;
    private float explodeForce = 100f;

    public float speed = 10f;
    //private bool CollideGround = false;

    // Start is called before the first frame update
    void Start()
    {
        
        
       if (GrenadeRigidBody != null) {
            // **** cant find main cam ****
            //GrenadeRigidBody.AddForce(transform.forward *32f,ForceMode.Impulse); 
            switch (bombType) {
            case GrenadeData.BombType.BOMB :
                Explode = ExplodeBomb; StartCoroutine(waitToDestroy());
                break;
            case GrenadeData.BombType.FIRE :
                ParticleSystem pfire = fireEffect.GetComponent<ParticleSystem>();
                //FireCollision fc = fireEffect.GetComponent<FireCollision>();
                if (pfire != null) {
                    //fc.SendMessage("SetDamage",damage);
                    var main = pfire.main;
                    main.startLifetime = ExplodeTime;
                    var tex = pfire.textureSheetAnimation;
                    tex.cycleCount = (int)ExplodeTime;
                }
                Explode = ExplodeFire;
                break;
            case GrenadeData.BombType.SMOKE :
                ParticleSystem psmoke = smokeEffect.GetComponent<ParticleSystem>();
                if (psmoke != null) {
                    var main = psmoke.main;
                    main.startLifetime = ExplodeTime;
                    var tex = psmoke.textureSheetAnimation;
                    tex.cycleCount = (int)ExplodeTime;
                }
                Explode = ExplodeSmoke;
                break;
            case GrenadeData.BombType.DECOY :
                Explode = ExplodeDecoy; StartCoroutine(waitToDestroy());
                break;
        }            
            StartCoroutine(Explode());     
        }
    }
    private void Awake()
    {   
        GrenadeRigidBody = GetComponent<Rigidbody>();

    }
    private bool isFireorSmoke() {
        return bombType == GrenadeData.BombType.SMOKE || bombType == GrenadeData.BombType.FIRE;
    }
    private void OnCollisionEnter(Collision other) {
        if (isFireorSmoke() && other.gameObject.tag == "Ground") {
            StartCoroutine(waitToDestroy());
        }
    }
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
            HitableObject entityHit = nearbyObject.GetComponent<HitableObject>();
            if (entityHit != null)
            {   
                if (entityHit.tag == "Boss") {
                    entityHit.TakeDamage(damage*0.5f);
                }
                else entityHit.TakeDamage(damage);
            }
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explodeForce, transform.position, explodeRadius);
            }
        }       
    }
    

    IEnumerator ExplodeFire() {
        yield return new WaitForSeconds(lifeTime);
        GameObject grenade = Instantiate(fireEffect, this.transform.position, Quaternion.identity);
        grenade.SendMessage("SetDamage",damage);
        grenade.SendMessage("SetExplodeRadius",explodeRadius);
        grenade.transform.localScale = new Vector3(explodeRadius, explodeRadius, explodeRadius);
        Destroy(grenade, ExplodeTime);
        // while(true) {
        //     yield return new WaitForSeconds(ExplodeTime);
        //     Instantiate(explodeEffect, this.transform.position, Quaternion.identity);
        //     Collider[] colliders = Physics.OverlapSphere(transform.position, explodeRadius);

        //     foreach (Collider nearbyObject in colliders) {
        //         PlayerHitbox entityHit = nearbyObject.GetComponent<PlayerHitbox>();
        //         if (entityHit != null)
        //         {
        //         entityHit.TakeDamage(damage);
        //         }
        //     }
        // }
        
    }
    IEnumerator ExplodeSmoke() {
        yield return new WaitForSeconds(lifeTime);
        GameObject grenade = Instantiate(smokeEffect, this.transform.position, Quaternion.identity);
        grenade.transform.localScale = new Vector3(explodeRadius, explodeRadius, explodeRadius);
        Destroy(grenade, ExplodeTime);
    }

    IEnumerator ExplodeDecoy() {
        yield return new WaitForSeconds(lifeTime);
        Instantiate(decoyEffect, this.transform.position, Quaternion.identity);
        Collider[] colliders = Physics.OverlapSphere(transform.position, explodeRadius);

        foreach (Collider nearbyObject in colliders)
        {
            Debug.Log(nearbyObject.tag);
            EnemyBase entityHit = nearbyObject.GetComponent<EnemyBase>();
            if (entityHit != null) {
                Debug.Log(nearbyObject.tag);
                entityHit.NoiseAlert(transform.position);
            }
            if (nearbyObject.tag == "Boss") {
                EnemyBossHealth bossHit = nearbyObject.GetComponentInParent(typeof(EnemyBossHealth)) as EnemyBossHealth;
                bossHit.DoStun();
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
