using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitbox : HitableObject
{
    [SerializeField] private EnemyBase Me;

    public override void TakeDamage(float damage)
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
