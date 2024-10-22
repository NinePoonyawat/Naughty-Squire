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
        [SerializeField] private Transform BombGrenade;
        [SerializeField] private Transform FireGrenade;
        [SerializeField] private Transform SmokeGrenade;
        [SerializeField] private Transform DecoyGrenade;
        [SerializeField] private Transform spawnBulletPosition;

        private ThirdPersonController thirdPersonController;
        private StarterAssetsInputs starterAssetInputs;

        private bool isUpdate = true;
        private bool isInventoryOpen = false;
        private bool isbuttonleftDown = false;
        private bool isbuttonrightDown = false;
        public float timecount;


        public event OnShootEvent OnShoot;
        public delegate void OnShootEvent(float recoil);
        public Camera playerCamera;


        [SerializeField] private InventoryManager inventoryManager;
        private InventoryController inventoryController;

        private void Awake()
        {
            thirdPersonController = GetComponent<ThirdPersonController>();
            starterAssetInputs = GetComponent<StarterAssetsInputs>();
        }

        void Start()
        {
            inventoryManager.OnInventoryOpen += Pause;
            inventoryManager.OnInventoryClose += Resume;

            inventoryController = inventoryManager.inventoryController;
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
            // Debug.Log("first : " + (mouseWorldPosition - spawnBulletPosition.position));
            // Debug.Log("second : " + (mouseWorldPosition));
            // Debug.Log("third : " + spawnBulletPosition.position);
            Vector3 aimDir = (mouseWorldPosition - spawnBulletPosition.position).normalized;
            if (starterAssetInputs.shoot && gunSystem.isShootable())
            {
                //Vector3 aimDir = Input.mousePosition;
                //Vector3 aimDir = playerCamera.transform.forward;           
                GameObject bullet = Instantiate(pfBulletProjectile, spawnBulletPosition.position, Quaternion.LookRotation(aimDir, Vector3.up)).gameObject;
                bullet.SendMessage("SetDamage",gunSystem.GetDamage());
                OnShoot?.Invoke(2f);
                gunSystem.Shoot();
                starterAssetInputs.shoot = false;
            }

            if (Input.GetMouseButtonDown(0) && grenadeThrower.isLeftThrowable() || isbuttonleftDown)
            {
                isbuttonleftDown = true;
                timecount += Time.deltaTime;
                // Debug.Log(grenadeThrower.getbombtype(true));
                if (timecount > grenadeThrower.getlifetime()) {
                    CreateandThrowGrenade(aimDir, timecount,true,false);
                    timecount = 0; isbuttonleftDown = false;
                }
                //Vector3 aimDir = Input.mousePosition.normalized;
                //Vector3 aimDir = playerCamera.transform.forward;
            }

            if (Input.GetMouseButtonUp(0) && isbuttonleftDown && grenadeThrower.isLeftThrowable())
            {
                CreateandThrowGrenade(aimDir, timecount,true,true);
                timecount = 0; isbuttonleftDown = false;
            }

            if (Input.GetMouseButtonDown(1) && grenadeThrower.isRightThrowable() || isbuttonrightDown)
            {
                isbuttonrightDown = true;
                timecount += Time.deltaTime;
                if (timecount > grenadeThrower.getlifetime()) {
                    CreateandThrowGrenade(aimDir, timecount,false,false);
                    timecount = 0; isbuttonrightDown = false;
                }
                //Vector3 aimDir = Input.mousePosition.normalized;
                //Vector3 aimDir = playerCamera.transform.forward;
                
            }
            if (Input.GetMouseButtonUp(1) && isbuttonrightDown && grenadeThrower.isRightThrowable()) {
                CreateandThrowGrenade(aimDir, timecount,false,true);
                timecount = 0; isbuttonrightDown = false;
            }

            if (starterAssetInputs.shoot && !gunSystem.isShootable()) starterAssetInputs.shoot = false;
        }
        private void CreateandThrowGrenade(Vector3 aimDir, float timecount, bool isLeft, bool thowable) {
            // Debug.Log("Throw");
            // Debug.Log(isLeft);
            // Debug.Log(grenadeThrower.getbombtype(isLeft));
            GameObject grenade;
            if (grenadeThrower.getbombtype(isLeft) == GrenadeData.BombType.BOMB) {
                grenade = Instantiate(BombGrenade, spawnBulletPosition.transform.position, Quaternion.LookRotation(aimDir, Vector3.up)).gameObject;
            } else if (grenadeThrower.getbombtype(isLeft) == GrenadeData.BombType.SMOKE) {
                grenade = Instantiate(SmokeGrenade, spawnBulletPosition.transform.position, Quaternion.LookRotation(aimDir, Vector3.up)).gameObject;
            } else if (grenadeThrower.getbombtype(isLeft) == GrenadeData.BombType.FIRE) {
                grenade = Instantiate(FireGrenade, spawnBulletPosition.transform.position, Quaternion.LookRotation(aimDir, Vector3.up)).gameObject;
            } else {
                grenade = Instantiate(DecoyGrenade, spawnBulletPosition.transform.position, Quaternion.LookRotation(aimDir, Vector3.up)).gameObject;
            }
            grenade.SendMessage("SetFloatData",grenadeThrower.getfloatdata());
            grenade.SendMessage("SetLifeTime",Mathf.Max(grenadeThrower.getlifetime() - timecount, 0));
            grenade.SendMessage("Setbombtype",grenadeThrower.getbombtype(isLeft));
            if (thowable) grenade.SendMessage("Throw", aimDir);
            if (isLeft) grenadeThrower.disarmleft();
            else grenadeThrower.disarmright();

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
