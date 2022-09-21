using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemywithMove : EnemyBase
{
    Vector3 currentRandomPos;

    private void Awake() {
        currentRandomPos = transform.position;
    }

    protected override void MakeMovementDecision() {
        timeTilNextMovement = 3f;
        currentRandomPos = new Vector3(transform.position.x, transform.position.y, transform.position.z + Random.Range(-4,5));

    }
    
    protected override void AImove() {
        if (timeTilNextMovement <= 0) {
            agent.velocity = Vector3.zero;
            MakeMovementDecision();
        }
        else {
            timeTilNextMovement -= Time.fixedDeltaTime;
        }
        agent.SetDestination(currentRandomPos);
    }
    
}
