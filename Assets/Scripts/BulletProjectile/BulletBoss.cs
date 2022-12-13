using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBoss : MonoBehaviour
{
    private float damage;
    private Rigidbody bulletRigidBody;
    public float speed = 20f;
    [SerializeField] private GameObject effectPrefab;
    // Start is called before the first frame update
    void Awake() {
        bulletRigidBody = GetComponent<Rigidbody>();
        Destroy(gameObject, 3f);
    }
    
    void Start()
    {
        bulletRigidBody.velocity = transform.forward * speed;
        damage = 20f;
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider hit)
    {
        HitableObject entityHit = hit.GetComponent<HitableObject>();
        
        //Debug.Log("this bullet deal " + damage + " to " + entityHit);
        if (entityHit != null)
        {
            entityHit.TakeDamage(damage);
        }
        Instantiate(effectPrefab, transform.position, transform.rotation);
        //FindObjectOfType<AudioManager>().Play("PistolBulletHit");
        Destroy(gameObject);
    }
}
