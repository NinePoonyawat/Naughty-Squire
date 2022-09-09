using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : EnemyBase
{
    private float degreesPerSecond = 1f;
    private int DirRotate = 1;

    protected override void MakeMovementDecision() {
        DirRotate = Random.Range(-1,2);
        timeTilNextMovement = 2f;
    }

    protected override void AImove() {
        agent.velocity = Vector3.zero;
        float angle = transform.rotation.eulerAngles.y;
        if (timeTilNextMovement <= 0) MakeMovementDecision();//Invoke("MakeMovementDecision",timeBetween);
        else {
            timeTilNextMovement -= Time.fixedDeltaTime;
        }
        //Debug.Log(Time.deltaTime);   
        //transform.Rotate(0,degreesPerSecond * Time.deltaTime, 0);
        // if (angle > 45) turnright = false;
        // else if (angle < 305) turnright = true;
        // if (turnright) angle += degreesPerSecond;
        // else angle -= degreesPerSecond; 
        // Debug.Log(angle);
        angle += degreesPerSecond * DirRotate;
            
        transform.rotation =  Quaternion.Euler(transform.rotation.eulerAngles.x, angle, transform.rotation.eulerAngles.z);
    }
}
