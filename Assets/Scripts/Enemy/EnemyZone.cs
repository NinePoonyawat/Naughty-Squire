using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyZone : EnemyBase
{
    // Start is called before the first frame update
    public float distance;
    public Rigidbody projectile;
    //public bool stop = false;
    void Awake()
    {
       distance = 7;
       //distance = Random.Range(5,10); 
    }

    public override void walking() {
        //Debug.Log("find");
        if (!playerIsInLOS) EnemyState = State.Idle;
        if (EnemyState != State.Attack) {
            agent.SetDestination(player.transform.position);
            CheckAttacking();
        } else {
            Attack();
        }
    }

    protected override void MakeMovementDecision() {
        Debug.Log("makemove");
    }

    protected override void AImove() {
        //Debug.Log("AImove");
    }

    void CheckAttacking() {
        if (playerIsInLOS && Vector3.Distance(transform.position,player.transform.position) <= distance) EnemyState = State.Attack;
    }

    void Attack() {
        agent.SetDestination(transform.position);
        transform.LookAt(player.transform);

        if (!alreadyAttacked) {
            Debug.Log("FIRE!!!++++");
            
            Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward *32f,ForceMode.Impulse);
            rb.AddForce(transform.up *8f,ForceMode.Impulse);
            
            alreadyAttacked = true;
            Invoke("ResetAttack",timeBetweenAttacks);
        }

    }

    void ResetAttack() {
        alreadyAttacked = false;
    }

}