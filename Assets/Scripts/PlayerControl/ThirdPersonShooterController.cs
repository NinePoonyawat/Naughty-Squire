using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using Cinemachine;
using StarterAssets;
using Weapon;

namespace Player
{
    public class ThirdPersonShooterController : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera aimVirtualCamera;
        [SerializeField] private GunSystem gunSystem;
        [SerializeField] private float normalSensitivity;
        [SerializeField] private float aimSensitivity;
        [SerializeField] private LayerMask aimColliderLayerMask = new LayerMask();
        [SerializeField] private Transform debugTransform;
        [SerializeField] private Transform pfBulletProjectile;
        [SerializeField] private Transform spawnBulletPosition;

        private ThirdPersonController thirdPersonController;
        private StarterAssetsInputs starterAssetInputs;

        private void Awake()
        {
            thirdPersonController = GetComponent<ThirdPersonController>();
            starterAssetInputs = GetComponent<StarterAssetsInputs>();
        }
        // Update is called once per frame
        void Update()
        {
            Vector3 mouseWorldPosition = Vector3.zero;
            Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
            Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayerMask))
            {
                //debugTransform.position = raycastHit.point;
                mouseWorldPosition = raycastHit.point;
            }
            if (starterAssetInputs.aim)
            {
                aimVirtualCamera.gameObject.SetActive(true);
                thirdPersonController.SetSensitivity(aimSensitivity);
                thirdPersonController.SetRotateOnMove(false);

                Vector3 worldAimTarget = mouseWorldPosition;
                worldAimTarget.x = transform.position.y;
                Vector3 aimDirection = (worldAimTarget - transform.position).normalized;

                transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);
            }
            else
            {
                aimVirtualCamera.gameObject.SetActive(false);
                thirdPersonController.SetSensitivity(normalSensitivity);
                thirdPersonController.SetRotateOnMove(true);
            }

            if (starterAssetInputs.shoot && !gunSystem.getOutOfAmmo())
            {
                Vector3 aimDir = (mouseWorldPosition - spawnBulletPosition.position).normalized;
                Instantiate(pfBulletProjectile, spawnBulletPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));
                starterAssetInputs.shoot = false;
            }
        }
    }
}
