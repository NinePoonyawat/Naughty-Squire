using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

[CreateAssetMenu]
public class WeaponData : ItemData
{

    [SerializeField] private GameObject gunGO;
    public int damage;
    public int ammoCapacity;
    public int ammoRemained;
    public float bulletSpeed;
    public float bulletLifetime;
    public float fireDelay;
    public List<MagazineData> availableMagazine;

    private void Awake()
    {
        ammoRemained = ammoCapacity;
    }

    public void Shoot()
    {
        ammoRemained--;
    }
}

