using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitbox : MonoBehaviour
{
    [SerializeField] private EnemyHealth Me;

    [SerializeField] private float maxHealth;
    [SerializeField] private float health;
    public float damageRatio;

    void Awake()
    {
        health = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        Me.TakeDamage(damage*damageRatio);
        health -= damage;
        if (health <= 0)
        {
            Destroy(this);
        }
    }
}
