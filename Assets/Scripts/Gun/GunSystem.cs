using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;
using System;
using TMPro;

namespace Weapon
{
    public class GunSystem : MonoBehaviour
    {
        [SerializeField] private GameObject[] Guns;
        [SerializeField] private int bulletPerMagazine = 0;
        [SerializeField] private float damage;
        private int bulletLeftInMagazine = 0;
        private bool isOutOfAmmo;
        private int prevBulletLeffInMagazine;

        private StarterAssetsInputs starterAssetsInputs;
        [SerializeField] private InventoryManager inventoryManager;
        private bool isInventoryOpen = false;

        public WeaponData currentData = null;
        private bool isArmed;
        public bool isSetBullet = false;

        private float cooldownTime = 0.5f;
        private float cooldownTimeCount = 0f;
        private bool isCooldown = false;
        private string fireSoundName;
        private string reloadSoundName;

        [SerializeField] private TMP_Text ammoText;

        public event EventHandler OnOutOfAmmoEvent;
        public event EventHandler OnReloadEvent;

        void Awake()
        {
            starterAssetsInputs = GetComponent<StarterAssetsInputs>();
            inventoryManager = GetComponent<InventoryManager>();
            bulletLeftInMagazine = bulletPerMagazine;
            isOutOfAmmo = (bulletLeftInMagazine == 0)? true:false;
            foreach(GameObject Gun in Guns) Gun.SetActive(false);
        }

        void Start()
        {
            ItemGrid LhandItemGrid = GameObject.Find("UI/UIInventory/Grid-L-Hand").GetComponent<ItemGrid>();
            ItemGrid RhandItemGrid = GameObject.Find("UI/UIInventory/Grid-R-Hand").GetComponent<ItemGrid>();
            //GameObject.Find("UI/UIInventory").SetActive(false);

            LhandItemGrid.weaponChangeEvent += setNewData;
            LhandItemGrid.onPickupWeaponEvent += disarm;

            RhandItemGrid.weaponChangeEvent += setNewData;
            RhandItemGrid.onPickupWeaponEvent += disarm;

            inventoryManager.OnInventoryOpen += openInventory;
            inventoryManager.OnInventoryClose += closeInventory;

            if (currentData == null) isArmed = false;

            SetAmmoText(bulletLeftInMagazine,bulletPerMagazine);
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
            SetAmmoText(bulletLeftInMagazine,bulletPerMagazine);
            if (bulletLeftInMagazine <= 0)
            {
                OnOutOfAmmoEvent?.Invoke(this,EventArgs.Empty);
                SetOutOfAmmo(true);
            }
            FindObjectOfType<AudioManager>().Play(fireSoundName);
            foreach(GameObject Gun in Guns)
            {
                Animator gunAnim = Gun.GetComponentInChildren<Animator>();
                gunAnim.SetTrigger("Fire");
            }
        }

        public void Reload()
        {
            if (currentData == null) return;
            OnReloadEvent?.Invoke(this,EventArgs.Empty);
            starterAssetsInputs.shoot = false;
            starterAssetsInputs.reload = false;
            if (!isArmed) return;
            bulletLeftInMagazine = bulletPerMagazine;
            SetAmmoText(bulletLeftInMagazine,bulletPerMagazine);
            SetOutOfAmmo(false);

            isCooldown = true;
            cooldownTimeCount = currentData.reloadDelay;

            FindObjectOfType<AudioManager>().Play(reloadSoundName);
            foreach(GameObject Gun in Guns)
            {
                Animator gunAnim = Gun.GetComponentInChildren<Animator>();
                gunAnim.SetTrigger("Reload");
            }
        }

        public void setNewData(InventoryItem weaponItem)
        {
            WeaponData weaponData = weaponItem.itemData as WeaponData;
            if (weaponData == null) return;
            if (weaponData.name == "Glock") Guns[0].SetActive(true);
            if (weaponData.name == "Sharp") Guns[1].SetActive(true);
            isArmed = true;
            currentData = weaponData;
            damage = weaponData.damage;
            bulletPerMagazine = weaponItem.ammoCapacity;
            if (!isSetBullet)
            {
                bulletLeftInMagazine = weaponItem.ammoRemained;
            }
            else isSetBullet = false;
            cooldownTime = weaponData.fireDelay;
            fireSoundName = weaponData.fireSoundName;
            reloadSoundName = weaponData.reloadSoundName;
            if (bulletLeftInMagazine > 0) SetOutOfAmmo(false);
            SetAmmoText(bulletLeftInMagazine,bulletPerMagazine);
            //Reload();

            if (bulletLeftInMagazine <= 0 && bulletPerMagazine > 0)
            {
                OnOutOfAmmoEvent?.Invoke(this,EventArgs.Empty);
                SetOutOfAmmo(true);
                Reload();
            }
        }

        public void SetBulletLeftInMagazine(int newBulletLeftInMagazine)
        {
            Debug.Log("sett " + newBulletLeftInMagazine);
            bulletLeftInMagazine = newBulletLeftInMagazine;
        }

        public void openInventory(object o,EventArgs e)
        {
            isInventoryOpen = true;
        }

        public void closeInventory(object o,EventArgs e)
        {
            isInventoryOpen = false;
        }

        public void disarm()
        {
            foreach(GameObject Gun in Guns) Gun.SetActive(false);
            isArmed = false;
            currentData = null;
            damage = 0;
            prevBulletLeffInMagazine = bulletLeftInMagazine;
            bulletPerMagazine = 0;
            bulletLeftInMagazine = 0;
            SetAmmoText(bulletLeftInMagazine,bulletPerMagazine);
        }

        public void SetAmmoText(int ammoRemain,int maxAmmo)
        {
            ammoText.SetText(ammoRemain + " / " + maxAmmo);
        }

        void SetOutOfAmmo(bool newInput)
        {
            isOutOfAmmo = newInput;
        }

        public bool isShootable()
        {
            return isArmed && !GetIsOutOfAmmo() && !isCooldown && !isInventoryOpen;
        }

        public float GetDamage()
        {
            return damage;
        }

        public bool GetIsOutOfAmmo()
        {
            return bulletLeftInMagazine == 0;
        }

        public int GetBulletLeftInMagazine()
        {
            return prevBulletLeffInMagazine;
        }
    }
}
