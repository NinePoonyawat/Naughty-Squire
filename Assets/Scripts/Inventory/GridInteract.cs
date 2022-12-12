using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GridInteract : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] InventoryController inventoryController;
    [SerializeField] LoadoutInventoryController loadoutInventoryController;
    [SerializeField] ItemGrid itemGrid;

    [SerializeField] bool isLoadoutInventory = false;

    private void Awake()
    {
        inventoryController = FindObjectOfType(typeof(InventoryController)) as InventoryController;
        if (isLoadoutInventory) loadoutInventoryController = GameObject.Find("LoadoutInventoryController").GetComponent<LoadoutInventoryController>();
        itemGrid = GetComponent<ItemGrid>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isLoadoutInventory) inventoryController.selectedItemGrid = itemGrid;
        else loadoutInventoryController.selectedItemGrid = itemGrid;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isLoadoutInventory) inventoryController.selectedItemGrid = null;
        else loadoutInventoryController.selectedItemGrid = null;
    }
    
}
