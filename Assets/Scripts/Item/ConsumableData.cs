using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

[CreateAssetMenu]
public class ConsumableData : ItemData
{
    public enum ConsumableType { CONSUME, UTILITY };

    [Header("Consumable Data")]
    public ConsumableType consumableType;
    
    [Header("Consume")]
    public int healthRecover = 0;
    public int energyRecover = 0;
    public string consumeSoundName;

}
