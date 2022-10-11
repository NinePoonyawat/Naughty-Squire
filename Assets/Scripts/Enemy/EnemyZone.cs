using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyZone : EnemyBase
{
    // Start is called before the first frame update
    public Rigidbody projectile;
    //public bool stop = false;
    void Awake()
    {
       StopDistance = 7;
       timeBetweenAttacks = 0.5f;
       //distance = Random.Range(5,10); 
    }

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

    protected override void MakeMovementDecision() {
        Debug.Log("makemove");
    }

    protected override void AImove() {
        //Debug.Log("AImove");
    }

    protected override void AttackMove() {
        Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
        rb.AddForce(transform.forward *32f,ForceMode.Impulse);
        rb.AddForce(transform.up *8f,ForceMode.Impulse);
    }


    

    void ResetAttack() {
        alreadyAttacked = false;
    }

}
