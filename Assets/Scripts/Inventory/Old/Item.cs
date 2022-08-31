using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public enum ItemType {
        Gun,
        Knife,
        Consumable,
        Ammo,
        Magazine,
    }

    public ItemType itemType;
    public int amount;
}
