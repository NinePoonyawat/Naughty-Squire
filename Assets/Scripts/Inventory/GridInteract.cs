using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GridInteract : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    InventoryController inventoryController;

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("pointer enter");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("pointer exit");
    }

    private void Awake()
    {
        inventoryController = FindObjectOfType(typeof(InventoryController)) as InventoryController;
    }
    
}
