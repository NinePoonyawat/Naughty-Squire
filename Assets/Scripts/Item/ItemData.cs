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
    public int durable = 1;
    public int weight = 1;
    public int value = 1;
}
