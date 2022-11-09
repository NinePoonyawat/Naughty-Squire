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

    private GameObject explodeEffect;
    private float explodeForce = 100f;

    public float speed = 10f;

    // Start is called before the first frame update
    void Start()
    {
       if (GrenadeRigidBody != null) {
            // **** cant find main cam ****
            StartCoroutine(waitToDestroy());
            StartCoroutine(Explode());     
        } 
    }
    private void Awake()
    {   
        GrenadeRigidBody = GetComponent<Rigidbody>();
        switch (bombType) {
            case GrenadeData.BombType.BOMB :
                Explode = ExplodeBomb;
                break;
            case GrenadeData.BombType.FIRE :
                Explode = ExplodeFire;
                lifeTime = 10f;
                break;
            case GrenadeData.BombType.SMOKE :
                Explode = ExplodeSmoke;
                lifeTime = 10f;
                break;
        }
    }
    public void Throw(Vector3 aim) {
       GrenadeRigidBody.velocity =  aim * speed;   
    }
    IEnumerator waitToDestroy()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }
    IEnumerator ExplodeBomb()
    {
        while (true) {
            yield return new WaitForSeconds(ExplodeTime);
            Instantiate(explodeEffect, this.transform.position, Quaternion.identity);
            Collider[] colliders = Physics.OverlapSphere(transform.position, explodeRadius);

            foreach (Collider nearbyObject in colliders)
            {
                PlayerHitbox entityHit = nearbyObject.GetComponent<PlayerHitbox>();
                if (entityHit != null)
                {
                    Debug.Log(entityHit);
                    entityHit.TakeDamage(damage);
                }
                Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddExplosionForce(explodeForce, transform.position, explodeRadius);
                }
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
        yield return new WaitForSeconds(ExplodeTime);

        // create layer that Raycast can hit
        int LayerIgnoreRaycast = LayerMask.NameToLayer("Character");
        gameObject.layer = LayerIgnoreRaycast;
    }

    IEnumerator ExplodeDecoy() {
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
    public void SetFloatData(List<float> ld)
    {
        damage = ld[0]; lifeTime = ld[1] ; explodeRadius = ld[2]; 
    }
    public void Setbombtype(GrenadeData.BombType bt) {
        bombType = bt;
    }
}
