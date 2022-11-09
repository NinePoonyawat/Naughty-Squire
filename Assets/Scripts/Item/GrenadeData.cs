using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GrenadeData : ItemData
{
    [Header("Bomb Data")]
    public float ExplodeTime;

    public float lifeTime;
    public float explodeRadius;
    public int damage;

    public BombType bombtype;
    public enum BombType {
        BOMB,
        SMOKE,
        FIRE,
    }
}
