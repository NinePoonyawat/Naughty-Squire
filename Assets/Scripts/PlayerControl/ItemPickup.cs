using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private InventoryController inventoryController;

    [Header("Parameters")]
    public float pickupRange = 5f;

    void Start()
    {
        inventoryController = GameObject.Find("InventoryController").GetComponent<InventoryController>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, pickupRange))
            {
                PickupItem(hit.transform.gameObject);
            }
        }
    }

    private void PickupItem (GameObject picked)
    {
        PickableItem pickItem = picked.GetComponent<PickableItem>();

        if (pickItem != null)
        {
            if(inventoryController.InsertItem(pickItem.itemData))
            {
                Destroy(picked.gameObject);
            }
        }
    }
        
}
