using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;
using System;

[DisallowMultipleComponent]
public class InventoryManager : MonoBehaviour
{
    private bool isInventoryShowed = false;

    //[SerializeField] private UI_Inventory uiInventory;
    private Inventory inventory;

    public GameObject uiInventory;
    public GameObject inventoryController;
    [SerializeField] private CursorControl cursorControl;

    public event EventHandler OnInventoryOpen;
    public event EventHandler OnInventoryClose;

    private void Awake()
    {
        uiInventory = GameObject.Find("UIInventory");
        inventoryController = GameObject.Find("InventoryController");
        cursorControl = GameObject.Find("Cursor").GetComponent<CursorControl>();
        //Resume();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) {
            if(isInventoryShowed) {
                Resume();
                cursorControl.DeActive();
                OnInventoryClose?.Invoke(this,EventArgs.Empty);
            }
            else {
                Pause();
                cursorControl.Active();
                OnInventoryOpen?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public void Resume() {
        uiInventory.SetActive(false);
        inventoryController.SetActive(false);
        isInventoryShowed = false;
    }

    public void Pause() {
        uiInventory.SetActive(true);
        inventoryController.SetActive(true);
        isInventoryShowed = true;
    }
}
