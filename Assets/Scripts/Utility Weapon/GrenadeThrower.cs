using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;
using System;

public class GrenadeThrower : MonoBehaviour {
    private delegate IEnumerator ExplodeBehavior();
    private ExplodeBehavior Explode ;
    private float lifeTime;
    [SerializeField] private float explodeRadius;

    [SerializeField] private GameObject explodeEffect;

    [SerializeField] private float damage;
    [SerializeField] private float explodeForce = 100f;
    private Rigidbody GrenadeRigidBody;

    public GrenadeData.BombType bombType;

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
                break;
            case GrenadeData.BombType.SMOKE :
                Explode = ExplodeSmoke;
                break;
        }
    }
    
    void Start()
    {
        if (GrenadeRigidBody != null) {
            GrenadeRigidBody.velocity = transform.forward * speed;
        }
    }
    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag == "Ground") {
            StartCoroutine(waitToDestroy());
            StartCoroutine(Explode());
        }
    }
    
    IEnumerator ExplodeDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Explode();
    }
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