using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
using Weapon;
using System;

public class Crosshair : MonoBehaviour
{
    GameObject crosshairGO;
    [SerializeField] CrosshairSection crosshair;
    [SerializeField] ThirdPersonShooterController thirdPersonShooterController;
    [SerializeField] InventoryManager inventoryManager;
    GunSystem gunSystem;

    private void Awake()
    {
        inventoryManager = GameObject.Find("Player").GetComponent<InventoryManager>();
        gunSystem = thirdPersonShooterController.GetGunSystem();

        thirdPersonShooterController.OnShoot += ExpandCrosshair;

        gunSystem.OnOutOfAmmoEvent += OutOfAmmo;
        gunSystem.OnReloadEvent += Reload;

        inventoryManager.OnInventoryOpen += CloseCrosshair;
        inventoryManager.OnInventoryClose += OpenCrosshair;
    }

    private void Start()
    {
        crosshairGO = transform.GetChild(0).gameObject;
        crosshair = crosshairGO.GetComponent<CrosshairSection>();

        crosshairGO.SetActive(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            if (crosshair.name == "Crosshair-Circle") ChangeCrosshair(CrosshairType.Dot);
            else ChangeCrosshair((CrosshairType) crosshairGO.transform.GetSiblingIndex() + 1); 
        }
    }

    private void ChangeCrosshair(CrosshairType crosshairType)
    {
        crosshairGO.SetActive(false);

        crosshairGO = transform.GetChild((int) crosshairType).gameObject;
        crosshair = crosshairGO.GetComponent<CrosshairSection>();

        crosshairGO.SetActive(true);
    }

    public void ExpandCrosshair(float recoil)
    {
        crosshair.Recoil(recoil);
    }

    public void OutOfAmmo(object o,EventArgs e)
    {
        crosshair.OutOfAmmo();
    }

    public void Reload(object o,EventArgs e)
    {
        crosshair.Reload();
    }

    public void OpenCrosshair(object o,EventArgs e)
    {
        gameObject.SetActive(true);
    }

    public void CloseCrosshair(object o,EventArgs e)
    {
        gameObject.SetActive(false);
    }
}

public enum CrosshairType
{
    Dot = 0,
    Fine = 1,
    Duplex = 2,
    Circle = 3
}
