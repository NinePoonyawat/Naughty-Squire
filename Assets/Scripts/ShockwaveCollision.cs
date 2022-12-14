using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockwaveCollision : MonoBehaviour
{
    // Start is called before the first frame update
    public float damage;
    public bool isDone = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
