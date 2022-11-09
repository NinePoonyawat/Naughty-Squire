using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

[CreateAssetMenu]
public class MagazineData : ItemData
{
    [Header("Magazine Data")]
    public int ammoCapacity;
    public WeaponData availableWeapon;
    public ItemData refillTool;
}

