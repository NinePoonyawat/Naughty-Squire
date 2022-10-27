using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitbox : HitableObject
{
    void Start()
    {
        canDestroy = false;
    }
}
