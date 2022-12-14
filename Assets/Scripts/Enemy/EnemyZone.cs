using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyZone : Enemy
{
    // Start is called before the first frame update
    [Header("Bomb Shooter")]
    [SerializeField] private Transform pfBulletProjectile;
    public Transform firePoint;
    public Rigidbody projectile;
    public Rigidbody projectileBomb;
    public bool IsBombBullet;

    public float ShootAngle;
    //public bool stop = false;
    // void Awake()
    // {
    //     StopDistance = 7;
    //     timeBetweenAttacks = 1f;
    //    //distance = Random.Range(5,10); 
    // }

    // public override void walking() {
    //     //Debug.Log("find");
    //     if (!playerIsInLOS) EnemyState = State.Idle;
    //     if (EnemyState != State.Attack) {
    //         agent.SetDestination(player.transform.position);
    //         CheckAttacking();
    //     } else {
    //         Attack();
    //     }
    // }

    // protected override void MakeMovementDecision() {
    //     Debug.Log("makemove");
    // }

    // protected override void AImove() {
    //     //Debug.Log("AImove");
    // }

    protected override IEnumerator AttackMove(float delay) {
        yield return new WaitForSeconds(delay);
        if (!IsBombBullet) {
            Debug.Log("SHOOOTTTT");
            //GameObject bullet = Instantiate(pfBulletProjectile, transform.position, Quaternion.identity).gameObject;
            Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward *32f,ForceMode.Impulse);
            rb.AddForce(transform.up *8f,ForceMode.Impulse);
        } else {
            Debug.Log(ShootAngle);
            Debug.Log("player : " + player.transform.position + " " + "enemy : " + transform.position);
            Vector3 direction = (player.transform.position - transform.position).normalized;
            //float swAngle = Vector2.Angle(new Vector2(transform.position.x,transform.position.z),new Vector2(player.transform.position.x,player.transform.position.z));
            float distance = Vector2.Distance(new Vector2(player.transform.position.x,player.transform.position.z), new Vector2(transform.position.x,transform.position.z));
            Debug.Log(distance);
            float speed =  Mathf.Pow(distance*0.98f / Mathf.Sin(2*Mathf.Deg2Rad*ShootAngle),0.5f);
            //transform.LookAt(new Vector3(direction.x,direction.y + distance * Mathf.Sin(Mathf.Deg2Rad*ShootAngle),direction.z));
            Rigidbody rb = Instantiate(projectileBomb, firePoint.position, Quaternion.identity).GetComponent<Rigidbody>();
            //rb.velocity = new Vector3(projection * Mathf.Sin(Mathf.Deg2Rad*swAngle), 
            //   speed * Mathf.Sin(Mathf.Deg2Rad*ShootAngle), projection * Mathf.Cos(Mathf.Deg2Rad*swAngle));
            rb.velocity = new Vector3(direction.x,direction.y + speed * Mathf.Sin(Mathf.Deg2Rad*ShootAngle),direction.z) * speed;
            //rb.AddForce(direction*speed, ForceMode.Impulse);
        }
    }


    

    // void ResetAttack() {
    //     alreadyAttacked = false;
    // }

}
