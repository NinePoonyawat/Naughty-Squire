using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyRepos : EnemyBase
{
    private float degreesPerSecond = 1f;
    private int DirRotate = 1;
    private Vector3 spawnPos;

    void Awake() {
        this.spawnPos = transform.position;
        timeTilNextMovement = 4f;
        //nextmovecooldown = 2f;
    }

    protected override void MakeMovementDecision() {
        DirRotate = Random.Range(-1,2);
        timeTilNextMovement = 4f;
    }

    protected override void AImove() {
        agent.SetDestination(spawnPos);
        float distance = Vector3.Distance(spawnPos,transform.position);
        if (distance <= 1f) {
            Debug.Log("Y");
            Rotate();
        }
    }

    private void Rotate() {
        agent.velocity = Vector3.zero;
        float angle = transform.rotation.eulerAngles.y;
        if (timeTilNextMovement <= 0) MakeMovementDecision();//Invoke("MakeMovementDecision",timeBetween);
        else {
            timeTilNextMovement -= Time.fixedDeltaTime;
            angle += degreesPerSecond * DirRotate;
        }
            
        transform.rotation =  Quaternion.Euler(transform.rotation.eulerAngles.x, angle, transform.rotation.eulerAngles.z);
    }
}
