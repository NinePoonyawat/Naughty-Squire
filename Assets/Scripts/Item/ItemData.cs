using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ItemData : ScriptableObject
{
    [Header("Settings")]
    public int width = 1;
    public int height = 1;
    public bool isTwoHanded = false;
    public Sprite itemIcon;
    public Sprite alternateIcon;
    public Mesh itemMesh;
    public string soundName;

    [Header("Data")]
    public string name = "Example Name";
    public string description = "";
    public int durable = 0;
    public int weight = 1;
    public int value = 1;
    public int[] quantity = new int[3];

    public bool equals(ItemData other)
    {
        return name == other.name;
    }

    public int GetSize()
    {
        return width * height;
    }
}
