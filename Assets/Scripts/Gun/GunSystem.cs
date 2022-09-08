using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

namespace Weapon
{
    public class GunSystem : MonoBehaviour
    {
        [SerializeField] private int bulletPerMagazine;
        [SerializeField] private float damage;
        private int bulletLeftInMagazine;
        private bool isOutOfAmmo;

        private StarterAssetsInputs starterAssetsInputs;

        void Awake()
        {
            starterAssetsInputs = GetComponent<StarterAssetsInputs>();
            bulletLeftInMagazine = bulletPerMagazine;
            SetOutOfAmmo(false);
        }

        void Update()
        {
            if(starterAssetsInputs.reload)
            {
                starterAssetsInputs.shoot = false;
                bulletLeftInMagazine = bulletPerMagazine;
                starterAssetsInputs.reload = false;
                Debug.Log(bulletLeftInMagazine);
                SetOutOfAmmo(false);
            }
        }

        public void Shoot()
        {
            bulletLeftInMagazine--;
            if (bulletLeftInMagazine == 0)
            {
                SetOutOfAmmo(true);
            }
            Debug.Log("bullet :" + bulletLeftInMagazine + " / " + bulletPerMagazine);
        }

        void SetOutOfAmmo(bool newInput)
        {
            isOutOfAmmo = newInput;
        }

        public bool getOutOfAmmo()
        {
            return isOutOfAmmo;
        }

        public float GetDamage()
        {
            return damage;
        }
    }
}
