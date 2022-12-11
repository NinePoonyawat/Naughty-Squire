using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChargingRun : StateMachineBehaviour
{
    //bool IsAttack = false;
   //  GameObject player;
   //  public NavMeshAgent agent;
    float timeTilNextMovement;
    EnemyBossHealth enemyboss;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       enemyboss = animator.GetComponent<EnemyBossHealth>();
       if (enemyboss != null) {
         enemyboss.DoCharge(timeTilNextMovement);
       }    
       timeTilNextMovement = 2f;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    //    if (timeTilNextMovement <= 0) animator.SetBool("Walk Forward",IsAttack);//Invoke("MakeMovementDecision",timeBetween);
    //     else {
    //         timeTilNextMovement -= Time.fixedDeltaTime;
    //     }
       if (Vector3.Distance(enemyboss.transform.position,enemyboss.player.transform.position) < 10) {
        enemyboss.SetIsAttack(true);
        //IsAttack = true;
       }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
      enemyboss.StopAllCoroutinesFunc();
    }
}
// }
// public class ChargingRunCoroutine : MonoBehaviour {
//     bool IsAttack = false;
//      public void DoCoroutine(float delay) {
//         StartCoroutine(Wait(delay));
//      }
//      public void StopAllCoroutinesFunc() {
//         StopAllCoroutines();
//     }
//     IEnumerator Wait(float delay) {
//         yield return new WaitForSeconds(delay);
//         GetComponent<Animator>().SetBool("Walk Forward",IsAttack);
//     }
//     public void SetIsAttack(bool isattack) {
//         IsAttack = isattack;
//     }
//  }  
