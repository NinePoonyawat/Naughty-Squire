using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterDieMenu : MonoBehaviour
{
    [SerializeField] private GameObject menu;
    [SerializeField] private InventoryManager inventoryManager;
    [SerializeField] private MenuController menuController;

    void Start()
    {
        inventoryManager = GameObject.Find("Player").GetComponent<InventoryManager>();
        menuController = GameObject.Find("Menu").GetComponent<MenuController>();
    }

    public void Dead()
    {
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        menu.SetActive(true);

        inventoryManager.GameEnd();
        menuController.GameEnd();
    }
}
