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
                readyToShoots,
                reloading;

        //Reference
        public Camera cam;

        public Transform attackPoint;

        public RaycastHit rayHit;

        public LayerMask whatIsEnemy;

        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
        }
    }
}
