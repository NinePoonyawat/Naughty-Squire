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

    [Header("After Die Menu")]
    [SerializeField] private AfterDieMenu afterDieMenu;

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
        health -= damage*damageRatio;
        if (health <= 0)
        {
            afterDieMenu.Dead();
        }
        UpdateUI();
    }

    void decreaseEnergy()
    {
        if (energy <= 0) return;

        energy -= 1;
        UpdateUI();
    }

    public bool Consume(ConsumableData consumeData)
    {
        int healAmount = consumeData.healthRecover;
        int eatAmount = consumeData.energyRecover;
        if (healAmount > 0 && eatAmount > 0 && health < maxHealth && energy < maxEnergy)
        {
            Heal(healAmount);
            Eat(eatAmount);
            return true;
        }
        else if (healAmount > 0 && health < maxHealth)
        {
            Heal(healAmount);
            return true;
        }
        else if (eatAmount > 0 && energy < maxEnergy)
        {
            Eat(eatAmount);
            return true;
        }
        return false;
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
