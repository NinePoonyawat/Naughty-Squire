using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    private bool isInventoryShowed = false;

    //[SerializeField] private UI_Inventory uiInventory;
    private Inventory inventory;

    public GameObject uiInventory;

    private CursorControl cursorControl;

    private void Awake() {
        //inventory = new Inventory();
        //uiInventory.SetInventory(inventory);

        cursorControl = GameObject.Find("Cursor").GetComponent<CursorControl>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) {
            if(isInventoryShowed) {
                Resume();
                cursorControl.DeActive();
            }
            else {
                Pause();
                cursorControl.Active();
            }
        }
    }

    public void Resume() {
        uiInventory.SetActive(false);
        isInventoryShowed = false;
    }

    public void Pause() {
        uiInventory.SetActive(true);
        isInventoryShowed = true;
    }
}
