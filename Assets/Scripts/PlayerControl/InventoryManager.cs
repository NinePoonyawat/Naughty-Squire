using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;
using System;

[DisallowMultipleComponent]
public class InventoryManager : MonoBehaviour
{
    private bool isInventoryShowed = false;

    public GameObject uiInventory;
    public InventoryController inventoryController;
    [SerializeField] private CursorControl cursorControl;

    public event EventHandler OnInventoryOpen;
    public event EventHandler OnInventoryClose;

    private bool isGameEnd;

    private void Awake()
    {
        uiInventory = GameObject.Find("UIInventory");
        inventoryController = GameObject.Find("InventoryController").GetComponent<InventoryController>();
        cursorControl = GameObject.Find("Cursor").GetComponent<CursorControl>();
        Resume();
    }

    private void Update()
    {
        if (isGameEnd) return;
        if (Input.GetKeyDown(KeyCode.E))
        {
            if(isInventoryShowed)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume() {
        inventoryController.SetOpen(false);
        isInventoryShowed = false;
        OnInventoryClose?.Invoke(this,EventArgs.Empty);
        cursorControl.DeActive();
    }

    public void Pause() {
        inventoryController.SetOpen(true);
        isInventoryShowed = true;
        OnInventoryOpen?.Invoke(this, EventArgs.Empty);
        cursorControl.Active();
    }

    public void GameEnd()
    {
        inventoryController.SetOpen(false);
        isInventoryShowed = false;
        isGameEnd = true;
    }
}
