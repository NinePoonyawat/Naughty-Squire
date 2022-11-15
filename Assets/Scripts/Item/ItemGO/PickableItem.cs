using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableItem : InteractableItem
{
    [Header("Settings")]
    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] private bool isGlow = false;
    [SerializeField] private GameObject auraPrefab;
    [SerializeField] private GameObject aura;

    [Header("Item Data")]
    public ItemData itemData;

    void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();

        if (itemData.itemMesh != null)
        {
            meshFilter.mesh = itemData.itemMesh;
        }
        GetComponent<MeshCollider>().sharedMesh = null;
        GetComponent<MeshCollider>().sharedMesh = meshFilter.mesh;
    }

    public override void Glow(bool open)
    {
        if (!isGlow && open) {
            aura = Instantiate(auraPrefab, transform.position, Quaternion.identity, transform);
            isGlow = true;
        }
        if (isGlow && !open)
        {
            Destroy(aura);
            isGlow = false;
        }
    }

    public override void Interacted()
    {
        if (aura != null) Destroy(aura);
        Destroy(gameObject);
    }

}
