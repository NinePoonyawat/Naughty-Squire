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
       //Physics.IgnoreCollision(player.GetComponent<Collider>(), GetComponent<Collider>());
    }

    public override void walking() {
        if (!playerIsInLOS) EnemyState = State.Idle;
        if (EnemyState != State.Attack) {
            agent.SetDestination(player.transform.position);
            CheckJumping();
            StopCoroutine("Jump");
            timeBetweenAttacks -= Time.deltaTime;
        } else {
            Debug.Log("NOT yet");
            //Invoke("Jump",timeBetweenAttacks);
            StartCoroutine("Jump");
            // if (NavMesh.SamplePosition(player.transform.position, out NavMeshHit hit, 1f, agent.areaMask)) {
            // Debug.Log("WARP");
            // agent.Warp(hit.position);
            // }
        }
    }

    protected override void MakeMovementDecision() {
        Debug.Log("makemove");
    }

    protected override void AImove() {
        //Debug.Log("AImove");
    }

    public void CheckJumping() {
        if (playerIsInLOS && Vector3.Distance(transform.position,player.transform.position) <= distance && timeBetweenAttacks < 0) EnemyState = State.Attack;
    }

    // void OnCollisionEnter(Collision collision)
    // {
    //   if (collision.gameObject.tag == "Player")
    //   {
    //     Debug.Log("COLLIDE IGNORE");
    //     Physics.IgnoreCollision(player.GetComponent<Collider>(), GetComponent<Collider>());
    //   }
    // }

    public IEnumerator Jump() {
        Debug.Log("JUMP");
        Vector3 startingPosition = transform.position;
        Vector3 endingPosition = player.transform.position - transform.forward;
        

        for (float time =0; time < 1; time += Time.deltaTime * JumpSpeed) {
            transform.position = Vector3.Lerp(startingPosition, endingPosition, time) + Vector3.up * HeightCurve.Evaluate(time);
            transform.rotation = Quaternion.Slerp(transform.rotation, 
                Quaternion.LookRotation(endingPosition - transform.position),time);

            yield return null;
        }
        // Collider[] colliders = Physics.OverlapSphere(transform.position,radius);
        // foreach (Collider hit in colliders) {
        //     //Debug.Log(hit.tag);
        //     if (hit.tag == "Player") {
        //         // Debug.Log("BOMB");
        //         // Rigidbody rb = hit.GetComponent<Rigidbody>();
        //         // if (rb != null) {
        //         //     rb.AddExplosionForce(power,transform.position,radius);
        //         // }
        //         // Vector3 hitDirection = hit.transform.position - transform.position;
        //         // hitDirection = hitDirection.normalized;
        //         // Debug.Log(hitDirection);
        //         //Quaternion angle = hit.transform.rotation;
        //         //Debug.Log(angle);
        //         //Debug.Log(player.transform.rotation);
        //         //Vector3 knockbackPosition = angle.eulerAngles.normalized;//new Vector3(angle.x,angle.y,angle.z);
        //         //Debug.Log(knockbackPosition);
        //         //hit.transform.position += knockbackPosition * -10;
        //     }
        // }
        // if (NavMesh.SamplePosition(player.transform.position, out NavMeshHit hit, 1f, agent.areaMask)) {
        //      Debug.Log("WARP");
        //      agent.Warp(hit.position);
        // }
        EnemyState = State.Idle;
        timeBetweenAttacks = 2f;
    }
}
