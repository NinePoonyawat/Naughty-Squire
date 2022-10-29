using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyJump : EnemyBase
{
    float JumpSpeed;
    bool Jumping;
    public float power = 10f;
    public float radius = 5f;
    public float upforce = 1f;
    public float distance;
    public AnimationCurve HeightCurve;    
    public float ShootAngle = 30;

    // Start is called before the first frame update
    void Awake()
    {
       JumpSpeed = 1 ;
       StopDistance = 4;
       timeBetweenAttacks = 10f;
       EnemyState = State.Idle;
       //Physics.IgnoreCollision(player.GetComponent<Collider>(), GetComponent<Collider>());
    }

    // public override void walking()
    // {
    //     CheckAttacking();
    //     if (!playerIsInLOS) EnemyState = State.Idle;
    //     Debug.Log("STOPWALK!");
    //     agent.SetDestination(player.transform.position); 
    //     StopCoroutine("Jump");
    //     if (EnemyState == State.Attack) Attack();
    // }

    // public override void walking() {
    //     if (!playerIsInLOS) EnemyState = State.Idle;
    //     if (!Jumping && EnemyState != State.Attack) {
    //         agent.SetDestination(player.transform.position);
    //         //Debug.Log("attacking check");
    //         CheckAttacking();
    //         StopCoroutine("Jump");
    //         timeBetweenAttacks -= Time.deltaTime;
    //     } else {
    //         Debug.Log("NOT yet");
    //         Jumping = true;
    //         //Invoke("Jump",timeBetweenAttacks);
    //         StartCoroutine("Jump");
    //         // if (NavMesh.SamplePosition(player.transform.position, out NavMeshHit hit, 1f, agent.areaMask)) {
    //         // Debug.Log("WARP");
    //         // agent.Warp(hit.position);
    //         // }
    //     }
    // }

    protected override void MakeMovementDecision() {
        Debug.Log("makemove");
    }

    protected override void AImove() {
        //Debug.Log("AImove");
    }

    protected override void CheckAttacking() {
        if (playerIsInLOS && Vector3.Distance(transform.position,player.transform.position) <= StopDistance) EnemyState = State.Attack;
    }

    protected override void AttackMove()
    {
        Debug.Log("JUMP!!");
        Vector3 direction = (player.transform.position - transform.position).normalized;
        float distance = Vector3.Distance(player.transform.position, transform.position);
        float speed =  Mathf.Pow(distance*0.98f / Mathf.Sin(2*Mathf.Deg2Rad*ShootAngle),0.5f);
        Rigidbody rb = agent.GetComponent<Rigidbody>();
        rb.velocity = new Vector3(direction.x,direction.y + distance * Mathf.Sin(Mathf.Deg2Rad*ShootAngle),direction.z) * speed;
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
        Jumping = false;
        EnemyState = State.Idle;
        timeBetweenAttacks = 2f;
    }
}
