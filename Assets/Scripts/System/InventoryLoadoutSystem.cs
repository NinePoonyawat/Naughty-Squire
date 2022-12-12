using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class InventoryLoadoutSystem : ScriptableObject
{
    [SerializeField] private List<ItemData> itemData = new List<ItemData>();

    public void ClearData()
    {
        for(int i = 0; i != itemData.Count; i++)
        {
            itemData[i] = null;
        }
    }

    public List<ItemData> GetItemData()
    {
        return itemData;
    }

    public void RemoveItem(ItemData itemtoRemove)
    {
        itemData.Remove(itemtoRemove);
    }

    public void AddItem(ItemData itemToAdd)
    {
        itemData.Add(itemToAdd);
    }
}
