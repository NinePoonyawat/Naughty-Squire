using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitbox : MonoBehaviour
{
    [SerializeField] private EnemyHealth Me;

    [SerializeField] private float maxHealth;
    [SerializeField] private float health;
    [SerializeField] private float damageRatio;

    void Awake()
    {
        health = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        //Debug.Log("i take " + damage*damageRatio + " dmg.");
        Me.TakeDamage(damage*damageRatio);
        health -= damage*damageRatio;
        if (health <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
