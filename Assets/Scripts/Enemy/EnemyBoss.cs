using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss : EnemyBase
{
    // Start is called before the first frame update
    private float degreesPerSecond = 1f;
    private int DirRotate = 1;
    Vector3 currentRandomPos;

    // Update is called once per frame
    void Update()
    {
        slider.value = CalculateHealth();
        CheckLOS();
        if (FleeAble && EnemyState != State.Flee) CheckFlee();
        switch(EnemyState) {
            case EnemyBase.State.Idle:
                GetComponent<Animator>().SetBool("Walk Forward",false);
                break;
            case EnemyBase.State.Walk:
                GetComponent<Animator>().SetBool("Walk Forward",true);
                break;
            case EnemyBase.State.Attack:
                GetComponent<Animator>().SetTrigger("PunchTrigger");
                GetComponent<Animator>().SetBool("Walk Forward",false);
                break;
        }
    }

    protected override void MakeMovementDecision() {
        DirRotate = Random.Range(-1,2);
        timeTilNextMovement = 2f;
    }

    protected override void AImove() {
        //agent.velocity = Vector3.zero;
        float angle = transform.rotation.eulerAngles.y;
        if (timeTilNextMovement <= 0) MakeMovementDecision();//Invoke("MakeMovementDecision",timeBetween);
        else {
            timeTilNextMovement -= Time.fixedDeltaTime;
        }
        angle += degreesPerSecond * DirRotate;
        transform.rotation =  Quaternion.Euler(transform.rotation.eulerAngles.x, angle, transform.rotation.eulerAngles.z);
        
    }
}
