using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyJump : EnemyBase
{
    float JumpSpeed;
    public float power = 10f;
    public float radius = 5f;
    public float upforce = 1f;
    public float distance;
    public AnimationCurve HeightCurve;
    // Start is called before the first frame update
    void Awake()
    {
       JumpSpeed = 1 ;
       distance = 4;
       timeBetweenAttacks = 2f;
       EnemyState = State.Idle;
    }

    public override void walking() {
        if (!playerIsInLOS) EnemyState = State.Idle;
        if (EnemyState != State.Attack) {
            agent.SetDestination(player.transform.position);
            CheckJumping();
        } else {
            Debug.Log("NOT yet");
            //Invoke("Jump",timeBetweenAttacks);
            StartCoroutine("Jump");
        }
    }

    protected override void MakeMovementDecision() {
        Debug.Log("makemove");
    }

    protected override void AImove() {
        //Debug.Log("AImove");
    }

    public void CheckJumping() {
        if (playerIsInLOS && Vector3.Distance(transform.position,player.transform.position) <= distance) EnemyState = State.Attack;
    }

    public IEnumerator Jump() {
        Debug.Log("JUMP");
        Vector3 startingPosition = transform.position;

        for (float time =0; time < 1; time += Time.deltaTime * JumpSpeed) {
            transform.position = Vector3.Lerp(startingPosition, player.transform.position, time) + Vector3.up * HeightCurve.Evaluate(time);
            transform.rotation = Quaternion.Slerp(transform.rotation, 
                Quaternion.LookRotation(player.transform.position - transform.position),time);

            yield return null;
        }
        Collider[] colliders = Physics.OverlapSphere(transform.position,radius);
        foreach (Collider hit in colliders) {
            //Debug.Log(hit.tag);
            if (hit.tag == "Player") {
                Debug.Log("BOMB");
                Rigidbody rb = hit.GetComponent<Rigidbody>();
                if (rb != null) {
                    rb.AddExplosionForce(power,transform.position,radius);
                }
            }
        }
        // if (NavMesh.SamplePosition(player.transform.position, out NavMeshHit hit, 1f, agent.areaMask)) {
        //     Debug.Log("WARP");
        //     agent.Warp(hit.position);
        // }
        EnemyState = State.Idle;
    }
}
