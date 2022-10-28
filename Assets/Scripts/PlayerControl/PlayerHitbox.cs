using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerHitbox : HitableObject
{
    [Header("UI")]
    public TMP_Text hpText;

    public override void Awake()
    {
        health = maxHealth;
        canDestroy = false;

        hpText.text = health.ToString();
    }
    public override void TakeDamage(float damage)
    {
        if (health <= 0) return;

        health -= damage*damageRatio;
        hpText.text = health.ToString();
    }
}
