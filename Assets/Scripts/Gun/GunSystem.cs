using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

namespace Weapon
{
    public class GunSystem : MonoBehaviour
    {
        [SerializeField] private int bulletPerMagazine;
        private int bulletLeftInMagazine;
        private bool isOutOfAmmo;

        private StarterAssetsInputs starterAssetsInputs;

        void Awake()
        {
            starterAssetsInputs = GetComponent<StarterAssetsInputs>();
            bulletLeftInMagazine = bulletPerMagazine;
        }

        void Update()
        {
            if(starterAssetsInputs.shoot && !isOutOfAmmo)
            {
                bulletLeftInMagazine--;
                if (bulletLeftInMagazine == 0)
                {
                    SetOutOfAmmo(true);
                }
                Debug.Log("bullet :" + bulletLeftInMagazine + " / " + bulletPerMagazine);
            }
            if(starterAssetsInputs.reload)
            {
                bulletLeftInMagazine = bulletPerMagazine;
                starterAssetsInputs.reload = false;
                Debug.Log(bulletLeftInMagazine);
                SetOutOfAmmo(false);
            }
        }

        void SetOutOfAmmo(bool newInput)
        {
            isOutOfAmmo = newInput;
        }

        public bool getOutOfAmmo()
        {
            return isOutOfAmmo;
        }
    }
}
