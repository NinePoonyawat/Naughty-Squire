using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class ShootingAttack : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state

    //Rigidbody rb;
    EnemyBossHealth enemyboss;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       
       enemyboss = animator.GetComponent<EnemyBossHealth>();
       if (enemyboss != null) enemyboss.DoShooting(1f);
       //animator.GetComponent<EnemyBossHealth>().DoCoroutine(1f,projectile,agent);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
      enemyboss.lookatPosition();
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
      enemyboss.StopAllCoroutinesFunc();
       //animator.GetComponent<ShootingAttackCoroutine>().StopAllCoroutinesFunc();
    }



}
// public class ShootingAttackCoroutine : MonoBehaviour {
//      public void DoCoroutine(float delay,Rigidbody projectile,NavMeshAgent rb) {
//         StartCoroutine(AttackMove(delay,projectile,rb));
//      }
//     public void StopAllCoroutinesFunc() {
//         StopAllCoroutines();
//     }
//     IEnumerator AttackMove(float delay,Rigidbody projectile,NavMeshAgent rb) {
//         yield return new WaitForSeconds(delay);
//         Rigidbody rc = Instantiate(projectile, rb.transform.position, Quaternion.identity).GetComponent<Rigidbody>();
//         rc.AddForce(rb.transform.forward *32f,ForceMode.Impulse);
//         rc.AddForce(rb.transform.up *8f,ForceMode.Impulse);
//         StartCoroutine(AttackMove(delay,projectile,rb));
//     }
//  }  
