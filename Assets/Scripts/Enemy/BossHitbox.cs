using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHitbox : HitableObject
{
    [SerializeField] protected EnemyBossHealth Me;
    // Start is called before the first frame update
    void Start()
    {
        Me = gameObject.GetComponentInParent(typeof(EnemyBossHealth)) as EnemyBossHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void TakeDamage(float damage)
    { 
        //if (health <= 0) return;
        
        Me.TakeDamage(damage*damageRatio);
    }
}
