using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ItemData : ScriptableObject
{
    public int width = 1;
    public int height = 1;
    public bool isTwoHanded = false;

    public Sprite itemIcon;
    public GameObject itemModel;
}
