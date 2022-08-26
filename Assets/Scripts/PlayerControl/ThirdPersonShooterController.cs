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

        private StarterAssetsInputs starterAssetInputs;

        private void Awake()
        {
            starterAssetInputs = GetComponent<StarterAssetsInputs>();
        }
        // Update is called once per frame
        void Update()
        {
            if (starterAssetInputs.aim)
            {
                aimVirtualCamera.gameObject.SetActive(true);
            }
            else
            {
                aimVirtualCamera.gameObject.SetActive(false);
            }
        }
    }
}
