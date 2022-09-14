using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

[CreateAssetMenu]
public class WeaponData : ItemData
{
    public int damage;
    public int ammoCapacity;
    public int ammoRemained;

    private void Awake()
    {
        ammoRemained = ammoCapacity;
    }
}
