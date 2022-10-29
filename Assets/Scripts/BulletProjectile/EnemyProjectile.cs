using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [Header("Essential")]
    public float damage = 10f;
    enum ProjectileType {BULLET, BOMB};
    [SerializeField] private ProjectileType projectileType;
    
    [Header("Bullet Type")]
    public float lifetime = 3f;
    
    [Header("Bomb Type")]
    public float explodeDelay = 1f;
    public float explodeRadius = 3f;
    public float explodeForce = 100f;
    public GameObject explodeEffect;
    public Color meshColor;


    private void Awake()
    {
        if (projectileType == ProjectileType.BOMB)
        {
            StartCoroutine(ExplodeDelay(explodeDelay));
        }
        if (projectileType == ProjectileType.BULLET)
        {
            Destroy(gameObject, lifetime);
        }
    }

    IEnumerator ExplodeDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Explode();
    }

    private void Explode()
    {
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

        Destroy(gameObject);
    }
    void OnDrawGizmos()
    {
        Gizmos.color = meshColor;
        Gizmos.DrawSphere(transform.position, explodeRadius);
    }
}
