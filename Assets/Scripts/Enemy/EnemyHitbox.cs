using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitbox : HitableObject
{
    [SerializeField] private bool canDebuff = false;

    [Header("Settings")]
    [SerializeField] private EnemyBase Me;
    [SerializeField] private Transform brokenPosition;
    [SerializeField] private GameObject brokenParticle;
    [SerializeField] private GameObject droppedItem;

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
        if (health <= conditionHealth)
        {
            if (brokenParticle != null && brokenPosition != null)
            {
                Instantiate(brokenParticle, brokenPosition.transform.position, brokenPosition.transform.rotation, transform);
            }
            if (droppedItem != null)
            {
                Instantiate(droppedItem, transform.position, transform.rotation);
            }
            if (canDebuff)
            {
                Me.Slow(speedMultiply);
                Me.Fragile(damageMultiply);
            }
            conditionHealth = 0;
        }
        if (health <= 0 && canDestroy)
        {
            Instantiate(brokenParticle, brokenPosition.transform.position, brokenPosition.transform.rotation, Me.transform);
            Destroy(this.gameObject);
        }
    }
}
