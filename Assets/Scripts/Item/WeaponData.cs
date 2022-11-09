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
    public float bulletSpeed;
    public float bulletLifetime;
    public float fireDelay;
    public float reloadDelay;
    public string fireSoundName;
    public string reloadSoundName;
}

