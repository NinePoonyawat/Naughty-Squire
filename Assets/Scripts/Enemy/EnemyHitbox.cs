using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitbox : HitableObject
{
    [SerializeField] private bool canDebuff = false;

    [Header("Settings")]
    [SerializeField] private EnemyBase Me;
    [SerializeField] private GameObject brokenParticle;

    [Header("Broken Debuff")]
    [Tooltip("current speed * Speed Multiply")]
    [SerializeField] private float speedMultiply = 1;
    [Tooltip("current overall damage ratio * Damage Multiply")]
    [SerializeField] private float damageMultiply = 1;
    
    void Start()
    {
        Me = gameObject.GetComponentInParent(typeof(EnemyBase)) as EnemyBase;
    }

    public override void TakeDamage(float damage)
    { 
        if (health <= 0) return;

        Me.TakeDamage(damage*damageRatio);
        health -= damage*damageRatio;
        if (health <= 0)
        {
            if (brokenParticle != null) Instantiate(brokenParticle, transform.position, transform.rotation, transform);

            if (canDebuff)
            {
                Me.Slow(speedMultiply);
                Me.Fragile(damageMultiply);
            }
            if (canDestroy) Destroy(this.gameObject);
        }
    }
}
