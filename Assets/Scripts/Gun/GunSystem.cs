using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapon
{
    public class GunSystem : MonoBehaviour
    {
        [Header("Gun Stat")]
        [SerializeField]
        private int damage;

        [SerializeField]
        private float

                timeBetweenShooting,
                spread,
                range,
                reloadTime,
                timeBetweenShots;

        [SerializeField]
        private int

                magazineSize,
                bulletPerTap;

        [SerializeField]
        private bool allowButtonHold;

        private int

                bulletsLeft,
                bulletShots;

        private bool

                shooting,
                readyToShoot,
                reloading;

        //Reference
        public Camera cam;

        public Transform attackPoint;

        public RaycastHit rayHit;

        public LayerMask whatIsEnemy;

        void Awake()
        {
            bulletsLeft = magazineSize;
            readyToShoot = true;
        }

        private void Update()
        {
            MyInput();
        }

        private void MyInput()
        {
            if (allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
            else shooting = Input.GetKeyDown(KeyCode.Mouse0);

            if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading) Reload();

            if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
            {
                bulletShots = bulletPerTap;
                Shoot();
            }
        }

        private void Shoot()
        {
            readyToShoot = false;

            //Spread
            float x = Random.Range(-spread, spread);
            float y = Random.Range(-spread, spread);

            Vector3 direction = cam.transform.forward + new Vector3(x,y,0);

            //Raycast
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out rayHit, range, whatIsEnemy))
            {
                Debug.Log(rayHit.collider.name);

                if (rayHit.collider.CompareTag("Enemy"))
                {
                    rayHit.collider.GetComponent<ShootingAi>().TakeDamage(damage);
                }
            }

            bulletsLeft--;
            bulletShots--;
            Invoke("ResetShot",timeBetweenShooting);

            if (bulletShots > 0 && bulletsLeft > 0)
                Invoke("Shoot",timeBetweenShots);
        }

        private void ResetShot()
        {
            readyToShoot = true;
        }

        private void Reload()
        {
            reloading = true;
            Invoke("ReloadFinished", reloadTime);
        }
    }
}
