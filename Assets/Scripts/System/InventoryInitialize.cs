using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryInitialize : MonoBehaviour
{
    [SerializeField] private InventoryLoadoutSystem inventoryLoadoutSystem;
    [SerializeField] private InventoryController inventoryController;

    public void Start()
    {
        List<ItemData> itemData = inventoryLoadoutSystem.GetItemData();
        foreach (ItemData item in itemData)
        {
            if (item == null) continue;
            if (!inventoryController.FillItem(item)) Debug.Log("no");
        }
    }
}
