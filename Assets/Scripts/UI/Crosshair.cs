using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

public class Crosshair : MonoBehaviour
{
    [SerializeField] GameObject CrosshairUp,CrosshairDown,CrosshairLeft,CrosshairRight;
    [SerializeField] ThirdPersonShooterController thirdPersonShooterController;

    private void Start()
    {
        thirdPersonShooterController.OnShoot += ExpandCrosshair;
    }

    private void ExpandCrosshair(float recoil)
    {

    }
}
