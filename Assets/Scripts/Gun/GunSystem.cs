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
        
        [SerializeField]
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

        //public ParticleSystem muzzleFlash;

        void Awake()
        {
            // test mag and reload time
            magazineSize = 25;
            reloadTime = 1;
            bulletsLeft = magazineSize;
            readyToShoot = true;
            cam = Camera.main;
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
                Debug.Log("Shoot!");
                bulletShots = bulletPerTap;
                Shoot();
            }
        }

        private void Shoot()
        {   
            readyToShoot = false;
            //muzzleFlash.Play();
            //Spread
            float x = Random.Range(-spread, spread);
            float y = Random.Range(-spread, spread);
            Debug.Log("fire!");

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
            Debug.Log("reload");
            reloading = true;
            Invoke("ReloadFinished", reloadTime);
        }

        private void ReloadFinished() {
            bulletsLeft = magazineSize;
            reloading = false;
        }

    }
}
