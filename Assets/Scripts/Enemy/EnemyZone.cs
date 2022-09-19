using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyZone : EnemyBase
{
    // Start is called before the first frame update
    public float distance;
    public bool stop = false;
    void Awake()
    {
       distance = 2;
       //distance = Random.Range(5,10); 
    }

    public override void walking() {
        Debug.Log("find");
        if (!playerIsInLOS) stop = false;
        if (!stop) {
            agent.SetDestination(player.transform.position);
            CheckingStop();
        } else {
            agent.SetDestination(transform.position);
        }
    }

    protected override void MakeMovementDecision() {
        Debug.Log("makemove");
    }

    protected override void AImove() {
        Debug.Log("AImove");
    }

    void CheckingStop() {
        if (playerIsInLOS && Vector3.Distance(transform.position,player.transform.position) <= distance) stop = true;
    }
}
