using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    private float damage;
    private Rigidbody bulletRigidBody;
    [SerializeField] private GameObject effectPrefab;

    private void Awake()
    {
        bulletRigidBody = GetComponent<Rigidbody>();
        Destroy(gameObject, 3f);
    }

    private void Start()
    {
        float speed = 20f;
        bulletRigidBody.velocity = transform.forward * speed;
    }

    private void OnTriggerEnter(Collider hit)
    {
        HitableObject entityHit = hit.GetComponent<HitableObject>();
        
        //Debug.Log("this bullet damage is " + damage);
        Debug.Log(entityHit);

        if (entityHit != null)
        {
            entityHit.TakeDamage(damage);
        }
        Instantiate(effectPrefab, transform.position, transform.rotation);
        FindObjectOfType<AudioManager>().Play("PistolBulletHit");
        Destroy(gameObject);
    }

    public void SetDamage(float newDamage)
    {
        damage = newDamage;
    }
}
