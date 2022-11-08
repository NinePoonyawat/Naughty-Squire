using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;
using System;

public class GrenadeThrower : MonoBehaviour {

    private delegate IEnumerator ExplodeBehavior();
    private ExplodeBehavior Explode ;
    private float lifeTime = 1f;
    [SerializeField] private float explodeRadius = 3;

    [SerializeField] private GameObject explodeEffect;

    [SerializeField] private float damage = 10;
    [SerializeField] private float explodeForce = 100f;
    private Rigidbody GrenadeRigidBody;

    public GrenadeData.BombType bombType = GrenadeData.BombType.BOMB;

    public float speed = 20f;

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
    
    void Start()
    {
        if (GrenadeRigidBody != null) {
            // **** cant find main cam ****
            GrenadeRigidBody.velocity =  new Vector3(1,1,1) * speed;       
        }
    }
    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag == "Ground") {
            StartCoroutine(waitToDestroy());
            StartCoroutine(Explode());
        }
    }
    
    // IEnumerator ExplodeDelay(float delay)
    // {
    //     yield return new WaitForSeconds(delay);
    //     Explode();
    // }
    IEnumerator waitToDestroy()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }
    IEnumerator ExplodeBomb()
    {
        while (true) {
            yield return new WaitForSeconds(0.5f);
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
            yield return new WaitForSeconds(1f);
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
        yield return new WaitForSeconds(0f);

        // create layer that Raycast can hit
        int LayerIgnoreRaycast = LayerMask.NameToLayer("Character");
        gameObject.layer = LayerIgnoreRaycast;
    }

    IEnumerator ExplodeDecoy() {
        while(true) {
            yield return new WaitForSeconds(0.5f);
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
}