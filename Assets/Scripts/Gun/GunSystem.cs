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
            SetOutOfAmmo(true);
        }

        void Start()
        {
            ItemGrid LhandItemGrid = GameObject.Find("UI/Grid-L-Hand").GetComponent<ItemGrid>();
            GameObject.Find("UI").SetActive(false);
            LhandItemGrid.placeItemEvent += setNewData;
        }

        void Update()
        {
            if(starterAssetsInputs.reload)
            {
                Reload();
            }
        }

        public void Shoot()
        {
            bulletLeftInMagazine--;
            if (bulletLeftInMagazine <= 0)
            {
                SetOutOfAmmo(true);
            }
            Debug.Log("bullet :" + bulletLeftInMagazine + " / " + bulletPerMagazine);
        }

        public void Reload()
        {
            starterAssetsInputs.shoot = false;
            bulletLeftInMagazine = bulletPerMagazine;
            starterAssetsInputs.reload = false;
            Debug.Log(bulletLeftInMagazine);
            SetOutOfAmmo(false);
        }

        public void setNewData(float newDamage,int maxBullet, int remainBullet)
        {
            damage = newDamage;
            bulletPerMagazine = maxBullet;
            bulletLeftInMagazine = remainBullet;
            if (remainBullet > 0) SetOutOfAmmo(false);
            //Reload();
            Debug.Log("Weapon Changed : " + bulletLeftInMagazine + "/" + bulletPerMagazine);
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
