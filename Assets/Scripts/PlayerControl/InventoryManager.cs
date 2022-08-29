using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    private bool isInventoryShowed = false;

    //[SerializeField] private UI_Inventory uiInventory;
    private Inventory inventory;

    public GameObject uiInventory;

    private void Awake() {
        //inventory = new Inventory();
        //uiInventory.SetInventory(inventory);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if(isInventoryShowed) {
                Resume();
            }
            else {
                Pause();
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
