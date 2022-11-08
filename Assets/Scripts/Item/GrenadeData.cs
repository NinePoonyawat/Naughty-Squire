using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GrenadeData  : ItemData
{
    [Header("Bomb Data")]
    public float ExplodeTime;

    public float lifeTime;
    public float duration;
    private float explodeRadius;
    public int damage;
    public enum BombType {
        BOMB,
        SMOKE,
        FIRE,
    }
}
