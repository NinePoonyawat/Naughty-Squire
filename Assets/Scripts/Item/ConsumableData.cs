using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

[CreateAssetMenu]
public class ConsumableData : ItemData
{
    public enum ConsumableType { HEAL, UTILITY };

    [Header("Consumable Data")]
    public ConsumableType consumableType;
    
    [Header("Heal")]
    public int healthRecover = 0;    
}
