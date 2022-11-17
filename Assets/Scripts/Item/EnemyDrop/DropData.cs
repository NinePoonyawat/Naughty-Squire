using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DropData : ScriptableObject
{
    [Header("Loot Drop")]
    public ItemDropRate[] items;
    public float poolRate;
}
