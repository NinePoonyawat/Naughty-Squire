using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;
using System;

namespace Weapon
{
    public class GunSystem : MonoBehaviour
    {
        [SerializeField] private int bulletPerMagazine;
        [SerializeField] private float damage;
        private int bulletLeftInMagazine;
        private bool isOutOfAmmo;

        private StarterAssetsInputs starterAssetsInputs;

        private float cooldownTime = 2f;
        private float cooldownTimeCount = 0f;
        private bool isCooldown = false;

        public event EventHandler OnOutOfAmmoEvent;
        public event EventHandler OnReloadEvent;

        void Awake()
        {
            starterAssetsInputs = GetComponent<StarterAssetsInputs>();
            bulletLeftInMagazine = bulletPerMagazine;
            isOutOfAmmo = (bulletLeftInMagazine == 0)? true:false;
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
            if(isCooldown)
            {
                UpdateCooldown();
            }
        }

        void UpdateCooldown()
        {
            cooldownTimeCount -= Time.deltaTime;
            if (cooldownTimeCount <= 0) isCooldown = false;
        }

        public void Shoot()
        {
            Debug.Log("shoot!");
            cooldownTimeCount = cooldownTime;
            isCooldown = true;
            bulletLeftInMagazine--;
            if (bulletLeftInMagazine <= 0)
            {
                OnOutOfAmmoEvent?.Invoke(this,EventArgs.Empty);
                SetOutOfAmmo(true);
            }
            Debug.Log("bullet :" + bulletLeftInMagazine + " / " + bulletPerMagazine);
        }

        public void Reload()
        {
            OnReloadEvent?.Invoke(this,EventArgs.Empty);
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
            Debug.Log("Weapon Changed : " + bulletLeftInMagazine + "/" + bulletPerMagazine + " dmg = " + newDamage);
        }

        void SetOutOfAmmo(bool newInput)
        {
            isOutOfAmmo = newInput;
        }

        public bool isShootable()
        {
            return !isOutOfAmmo && !isCooldown;
        }

        public float GetDamage()
        {
            return damage;
        }
    }
}
