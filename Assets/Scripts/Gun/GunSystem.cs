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

        private WeaponData currentData = null;
        private bool isArmed;

        private float cooldownTime = 0.5f;
        private float cooldownTimeCount = 0f;
        private bool isCooldown = false;
        private string fireSoundName;
        private string reloadSoundName;

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
            ItemGrid RhandItemGrid = GameObject.Find("UI/Grid-R-Hand").GetComponent<ItemGrid>();
            GameObject.Find("UI").SetActive(false);

            LhandItemGrid.weaponChangeEvent += setNewData;
            LhandItemGrid.onPickupWeaponEvent += disarm;

            RhandItemGrid.weaponChangeEvent += setNewData;
            LhandItemGrid.onPickupWeaponEvent += disarm;

            if (currentData == null) isArmed = false;
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
            cooldownTimeCount = cooldownTime;
            isCooldown = true;
            bulletLeftInMagazine--;
            currentData.Shoot();
            if (bulletLeftInMagazine <= 0)
            {
                OnOutOfAmmoEvent?.Invoke(this,EventArgs.Empty);
                SetOutOfAmmo(true);
            }
            Debug.Log("bullet :" + bulletLeftInMagazine + " / " + bulletPerMagazine);

            FindObjectOfType<AudioManager>().Play(fireSoundName);
        }

        public void Reload()
        {
            OnReloadEvent?.Invoke(this,EventArgs.Empty);
            starterAssetsInputs.shoot = false;
            starterAssetsInputs.reload = false;
            if (!isArmed) return;
            bulletLeftInMagazine = bulletPerMagazine;
            Debug.Log(bulletLeftInMagazine);
            SetOutOfAmmo(false);

            FindObjectOfType<AudioManager>().Play(reloadSoundName);
        }

        public void setNewData(WeaponData weaponData)
        {
            isArmed = true;
            currentData = weaponData;
            damage = weaponData.damage;
            bulletPerMagazine = weaponData.ammoCapacity;
            bulletLeftInMagazine = weaponData.ammoRemained;
            cooldownTime = weaponData.fireDelay;
            fireSoundName = weaponData.fireSoundName;
            reloadSoundName = weaponData.reloadSoundName;
            if (bulletLeftInMagazine > 0) SetOutOfAmmo(false);
            //Reload();
        }

        public void disarm()
        {
            isArmed = false;
        }

        void SetOutOfAmmo(bool newInput)
        {
            isOutOfAmmo = newInput;
        }

        public bool isShootable()
        {
            return isArmed && !isOutOfAmmo && !isCooldown;
        }

        public float GetDamage()
        {
            return damage;
        }
    }
}
