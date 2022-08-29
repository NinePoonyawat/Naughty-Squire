using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;

namespace Player
{
    public class ThirdPersonShooterController : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera aimVirtualCamera;
        [SerializeField] private float normalSensitivity;
        [SerializeField] private float aimSensitivity;

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
            if (starterAssetInputs.aim)
            {
                aimVirtualCamera.gameObject.SetActive(true);
                thirdPersonController.SetSensitivity(aimSensitivity);
            }
            else
            {
                aimVirtualCamera.gameObject.SetActive(false);
                thirdPersonController.SetSensitivity(normalSensitivity);
            }
        }
    }
}
