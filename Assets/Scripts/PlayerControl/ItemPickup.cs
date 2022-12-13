using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private InventoryController inventoryController;
    public Camera playerCamera;
    public InteractableItem _pointItem;

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
        InteractableItem pointItem = pointed.GetComponent<InteractableItem>();
        BulletRecognize bulletRecognize = pointed.GetComponent<BulletRecognize>();

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
                InteractItem(pointItem,bulletRecognize);
            }
            _pointItem = pointItem;
        }
    }

    private void InteractItem (InteractableItem interactItem, BulletRecognize bulletRecognize)
    {
        PickableItem pickItem = interactItem as PickableItem;
        if(pickItem != null && inventoryController.FillItem(pickItem.itemData, bulletRecognize))
        {
            pickItem.Interacted();
            FindObjectOfType<AudioManager>().Play("InventoryInteract");
        }
        ObjectiveItem objective = interactItem as ObjectiveItem;
        if (objective != null)
        {
            objective.Interacted();
        }
    }
        
}
