using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
using Weapon;
using System;

public class Crosshair : MonoBehaviour
{
    [SerializeField] CrosshairSection CrosshairUp,CrosshairDown,CrosshairLeft,CrosshairRight;
    [SerializeField] ThirdPersonShooterController thirdPersonShooterController;
    [SerializeField] InventoryManager inventoryManager;
    GunSystem gunSystem;

    private void Awake()
    {
        gunSystem = thirdPersonShooterController.GetGunSystem();

        thirdPersonShooterController.OnShoot += ExpandCrosshair;

        gunSystem.OnOutOfAmmoEvent += OutOfAmmo;
        gunSystem.OnReloadEvent += Reload;

        inventoryManager.OnInventoryOpen += CloseCrosshair;
        inventoryManager.OnInventoryClose += OpenCrosshair;
    }

    public void ExpandCrosshair(float recoil)
    {
        CrosshairUp.Recoil(recoil);
        CrosshairDown.Recoil(recoil);
        CrosshairLeft.Recoil(recoil);
        CrosshairRight.Recoil(recoil);
    }

    public void OutOfAmmo(object o,EventArgs e)
    {
        CrosshairUp.OutOfAmmo();
        CrosshairDown.OutOfAmmo();
        CrosshairLeft.OutOfAmmo();
        CrosshairRight.OutOfAmmo();
    }

    public void Reload(object o,EventArgs e)
    {
        CrosshairUp.Reload();
        CrosshairDown.Reload();
        CrosshairLeft.Reload();
        CrosshairRight.Reload();
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
