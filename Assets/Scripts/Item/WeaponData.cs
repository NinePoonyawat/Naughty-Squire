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
    public float bulletSpeed;
    public float bulletLifetime;
    public float fireDelay;
    public InventoryItem equippedMagazine;
    public List<MagazineData> availableMagazine;

    private void Awake()
    {
        ammoRemained = ammoCapacity;
    }
}

