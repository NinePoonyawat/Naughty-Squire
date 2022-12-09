using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireCollision : MonoBehaviour
{
    // Start is called before the first frame update
    private float damage;
    private float explodeRadius;

    void Start() {
        StartCoroutine(loopCollider());
    }

    IEnumerator loopCollider() {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explodeRadius);

        foreach (Collider nearbyObject in colliders)
        {
            HitableObject entityHit = nearbyObject.GetComponent<HitableObject>();
            if (entityHit != null)
            {   
            entityHit.TakeDamage(damage);
            }
        }
        yield return new WaitForSeconds(1f);
        StartCoroutine(loopCollider());       
    }

    private void OnTriggerEnter(Collider hit)
    {
        HitableObject entityHit = hit.GetComponent<HitableObject>();
        if (entityHit != null)
        {
            entityHit.TakeDamage(damage);
        }
        //FindObjectOfType<AudioManager>().Play("PistolBulletHit");
    }
    private void SetDamage(float d) {
        damage = d;
    }
    private void SetExplodeRadius(float r) {
        explodeRadius = r;
    }
}
