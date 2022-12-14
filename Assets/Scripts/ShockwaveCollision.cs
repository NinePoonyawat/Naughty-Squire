using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockwaveCollision : MonoBehaviour
{
    // Start is called before the first frame update
    public float explodeRadius = 6f;
    public float damage = 30;
    public bool isDone = false;
    
    void Awake()
    {
        Explode();
    }

    private void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explodeRadius);

        foreach (Collider nearbyObject in colliders)
        {
            PlayerHitbox entityHit = nearbyObject.GetComponent<PlayerHitbox>();
            if (entityHit != null)
            {
                Debug.Log(entityHit);
                entityHit.TakeDamage(damage);
            }
        }
    }

    private void OnParticleCollision(GameObject other) {
        HitableObject entityHit = other.GetComponent<HitableObject>();
        if (entityHit != null && other.tag == "Player" && !isDone)
        {
            Debug.Log("hit player");
            entityHit.TakeDamage(damage);
            isDone = true;
        }
    }
}
