using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class HitableObject : MonoBehaviour
{
    [Header("Essentials")]
    [SerializeField] protected float maxHealth = 10;
    [SerializeField] protected float health;
    [SerializeField] protected float damageRatio = 1;

    [SerializeField] protected bool canDestroy = true;

    public virtual void Awake()
    {
        health = maxHealth;
    }

    public virtual void TakeDamage(float damage)
    {
        if (health <= 0) return;

        health -= damage*damageRatio;

        if (health <= 0 && canDestroy)
        {
            Destroy(this.gameObject);
        }
    }

    public float getHealth()
    {
        return health;
    }

    public float getMaxHealth()
    {
        return maxHealth;
    }
}
