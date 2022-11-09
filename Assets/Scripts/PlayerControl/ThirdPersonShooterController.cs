using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using Cinemachine;
using StarterAssets;
using Weapon;
using System;

namespace Player
{
    public class ThirdPersonShooterController : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera aimVirtualCamera;
        [SerializeField] private GunSystem gunSystem;
        [SerializeField] private GrenadeThrower grenadeThrower;
        [SerializeField] private float normalSensitivity;
        [SerializeField] private float aimSensitivity;
        [SerializeField] private LayerMask aimColliderLayerMask = new LayerMask();
        [SerializeField] private Transform debugTransform;
        [SerializeField] private Transform pfBulletProjectile;
        [SerializeField] private Transform pfGrenade;
        [SerializeField] private Transform spawnBulletPosition;

        private ThirdPersonController thirdPersonController;
        private StarterAssetsInputs starterAssetInputs;

        private bool isUpdate = true;
        private bool isInventoryOpen = false;

        public event OnShootEvent OnShoot;
        public delegate void OnShootEvent(float recoil);


        [SerializeField] private InventoryManager inventoryManager;

        private void Awake()
        {
            thirdPersonController = GetComponent<ThirdPersonController>();
            starterAssetInputs = GetComponent<StarterAssetsInputs>();
        }

        void Start()
        {
            inventoryManager.OnInventoryOpen += Pause;
            inventoryManager.OnInventoryClose += Resume;
        }
        // Update is called once per frame
        void Update()
        {
            if (starterAssetInputs.shoot && isInventoryOpen)
            {
                starterAssetInputs.shoot = false;
            }
            
            if (!isUpdate) return;

            Vector3 mouseWorldPosition = Vector3.zero;
            Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
            Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayerMask))
            {
                //debugTransform.position = raycastHit.point;
                mouseWorldPosition = raycastHit.point;
            }

            // if (starterAssetInputs.aim)
            // {
            //     aimVirtualCamera.gameObject.SetActive(true);
            //     // thirdPersonController.SetSensitivity(aimSensitivity);
            //     // thirdPersonController.SetRotateOnMove(false);

            //     Vector3 worldAimTarget = mouseWorldPosition;
            //     worldAimTarget.x = transform.position.y;
            //     Vector3 aimDirection = (worldAimTarget - transform.position).normalized;

            //     transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);
            // }
            // else
            // {
            //     aimVirtualCamera.gameObject.SetActive(false);
            //     // thirdPersonController.SetSensitivity(normalSensitivity);
            //     // thirdPersonController.SetRotateOnMove(true);
            //}

            if (starterAssetInputs.shoot && gunSystem.isShootable())
            {
                Vector3 aimDir = (mouseWorldPosition - spawnBulletPosition.position).normalized;
                GameObject bullet = Instantiate(pfBulletProjectile, spawnBulletPosition.position, Quaternion.LookRotation(aimDir, Vector3.up)).gameObject;
                bullet.SendMessage("SetDamage",gunSystem.GetDamage());
                OnShoot?.Invoke(2f);
                gunSystem.Shoot();
                starterAssetInputs.shoot = false;
            } else if (Input.GetMouseButtonDown(0) && grenadeThrower.isThrowable()) {
                Vector3 aimDir = (mouseWorldPosition - spawnBulletPosition.position).normalized;
                GameObject grenade = Instantiate(pfGrenade, spawnBulletPosition.position, Quaternion.LookRotation(aimDir, Vector3.up)).gameObject;
                grenade.SendMessage("SetData",grenadeThrower.getfloatdata());
                grenade.SendMessage("Setbombtype",grenadeThrower.getbombtype());
                if (Input.GetMouseButtonUp(0)) {
                    grenade.SendMessage("Throw", aimDir);
                }
            }
            if (starterAssetInputs.shoot && !gunSystem.isShootable()) starterAssetInputs.shoot = false;
        }

        public void Resume(object o,EventArgs e)
        {
            isUpdate = true;
            isInventoryOpen = false;
        }

        public void Pause(object o,EventArgs e)
        {
            isUpdate = false;
            isInventoryOpen = true;
        }

        public GunSystem GetGunSystem()
        {
            return gunSystem;
        }
    }
}
