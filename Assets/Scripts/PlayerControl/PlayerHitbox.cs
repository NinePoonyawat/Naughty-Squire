using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerHitbox : HitableObject
{
    public float maxEnergy = 40;
    public float energy;
    public float maxStamina = 100;
    public float stamina;

    public float energyDrainStart = 30f;
    public float energyDrainInterval = 5f;

    [Header("UI")]
    public TMP_Text hpText;
    public TMP_Text energyText;

    public override void Awake()
    {
        health = maxHealth;
        energy = maxEnergy;

        canDestroy = false;

        hpText = GameObject.Find("UIHealth").GetComponent<TMP_Text>();
        energyText = GameObject.Find("UIEnergy").GetComponent<TMP_Text>();

        InvokeRepeating("decreaseEnergy", energyDrainStart, energyDrainInterval);
        
        UpdateUI();
    }

    public override void TakeDamage(float damage)
    {
        if (health <= 0) return;

        health -= damage*damageRatio;
        UpdateUI();
    }

    void decreaseEnergy()
    {
        if (energy <= 0) return;

        energy -= 1;
        UpdateUI();
    }

    public void Heal(float healAmount)
    {
        health += healAmount;
        if (health > maxHealth) health = maxHealth;
        UpdateUI();
    }

    public void Eat(float eatAmount)
    {
        energy += eatAmount;
        if (energy > maxEnergy) energy = maxEnergy;
        UpdateUI();
    }

    public void UpdateUI()
    {
        hpText.text = health.ToString();
        energyText.text = energy.ToString();
    }
}
