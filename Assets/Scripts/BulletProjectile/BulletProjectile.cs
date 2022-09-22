using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    private float damage;
    private Rigidbody bulletRigidBody;

    private void Awake()
    {
        bulletRigidBody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        float speed = 10f;
        bulletRigidBody.velocity = transform.forward * speed;
    }

    private void OnTriggerEnter(Collider hit)
    {
        EnemyHitbox entityHit = hit.GetComponent<EnemyHitbox>();
        
        //Debug.Log("this bullet damage is " + damage);
        //Debug.Log(entityHit);

        if (entityHit != null)
        {
            entityHit.TakeDamage(damage);
        }
        Destroy(gameObject);
    }

    public void SetDamage(float newDamage)
    {
        damage = newDamage;
    }
}
