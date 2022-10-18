using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitbox : HitableObject
{
    [SerializeField] private EnemyBase Me;
    public bool isBroken;
    

    public override void TakeDamage(float damage)
    {
        //Debug.Log(this.gameObject + " take " + damage*damageRatio + " dmg.");
        
        if (health <= 0) return;

        Me.TakeDamage(damage*damageRatio);
        health -= damage*damageRatio;
        if (health <= 0)
        {
            isBroken = true;
            if (canDestroy) Destroy(this.gameObject);
        }
    }
}
