using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

[CreateAssetMenu]
public class WeaponData : ItemData
{
    [Header("Weapon Data")]
    [SerializeField] private GameObject gunGO;
    public int damage;
    public int ammoCapacity;
    public int ammoRemained;
    public float bulletSpeed;
    public float bulletLifetime;
    public float fireDelay;
    public string fireSoundName;
    public string reloadSoundName;
    public MagazineData equippedMagazine;
    public List<MagazineData> availableMagazine;

    private void Awake()
    {
        ammoRemained = ammoCapacity;
    }

    public void EquipMagazine(MagazineData magazine)
    {
        equippedMagazine = magazine;
    }

    public MagazineData GetMagazine()
    {
        return equippedMagazine;
    }

    public void Shoot()
    {
        if(equippedMagazine == null) return;
        if(equippedMagazine.ammoRemained <= 0) return;

        ammoRemained--;
    }
}

