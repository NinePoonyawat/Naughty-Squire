using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trapmine : MonoBehaviour
{
    [SerializeField] private float detectRadius;
    [SerializeField] private float explodeRadius;

    [SerializeField] private GameObject explodeEffect;

    [SerializeField] private float damage;

    public Color meshColor = Color.red;

    void Update()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectRadius);

        foreach (Collider nearbyObject in colliders)
        {
            if (nearbyObject.CompareTag("Enemy"))
            {
                Debug.Log("BOOM");
                Explode();
            }
        }
    }

    private void Explode()
    {
        Instantiate(explodeEffect, this.transform.position, Quaternion.identity);
        Collider[] colliders = Physics.OverlapSphere(transform.position, explodeRadius);

        foreach (Collider nearbyObject in colliders)
        {
            EnemyHitbox entityHit = nearbyObject.GetComponent<EnemyHitbox>();
            if (entityHit != null)
            {
                //Debug.Log(entityHit);
                entityHit.TakeDamage(damage);
            }
        }

        Destroy(this.gameObject);
    }

    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = meshColor;
        Gizmos.DrawSphere(transform.position, detectRadius);
        Gizmos.DrawSphere(transform.position, explodeRadius);
    }
}
