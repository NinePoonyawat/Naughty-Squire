using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private InventoryController inventoryController;
    public Camera playerCamera;
    public PickableItem _pointItem;

    [Header("Parameters")]
    public float pickupRange = 5f;

    void Start()
    {
        inventoryController = GameObject.Find("InventoryController").GetComponent<InventoryController>();
        playerCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
    }
    void Update()
    {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, pickupRange))
        {
            PointingItem(hit.transform.gameObject);
        }
    }

    private void PointingItem (GameObject pointed)
    {
        PickableItem pointItem = pointed.GetComponent<PickableItem>();

        if (_pointItem != null && _pointItem != pointItem)
        {
            _pointItem.Glow(false);
            _pointItem = pointItem;
        }
        if (pointItem != null)
        {
            pointItem.Glow(true);
            if (Input.GetKeyDown(KeyCode.F))
            {
                PickupItem(pointItem);
            }
            _pointItem = pointItem;
        }
    }

    private void PickupItem (PickableItem pickItem)
    {
        if(inventoryController.FillItem(pickItem.itemData))
        {
            pickItem.Picked();
        }
        FindObjectOfType<AudioManager>().Play("InventoryInteract");
    }
        
}
