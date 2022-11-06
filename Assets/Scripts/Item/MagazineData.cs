using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

[CreateAssetMenu]
public class MagazineData : ItemData
{
    [Header("Magazine Data")]
    public int ammoCapacity;
    public int ammoRemained;
    public List<WeaponData> availableWeapon;

    private void Awake()
    {
        ammoRemained = ammoCapacity;
    }
}

