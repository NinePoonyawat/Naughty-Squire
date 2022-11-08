using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

[CreateAssetMenu]
public class ConsumableData : ItemData
{
    public enum ConsumableType {HEAL};

    [Header("Consumable Data")]
    public ConsumableType consumableType;
    public int healthRecover;
}
