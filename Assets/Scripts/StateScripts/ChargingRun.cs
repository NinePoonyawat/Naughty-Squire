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
       timeTilNextMovement = 2f;
       if (enemyboss != null) {
          enemyboss.StopCoroutinesFunc();
          enemyboss.DoCharge(timeTilNextMovement);
       }
       //animator.SetBool("EnragedAttack",true);    
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    //    if (timeTilNextMovement <= 0) animator.SetBool("Walk Forward",IsAttack);//Invoke("MakeMovementDecision",timeBetween);
    //     else {
    //         timeTilNextMovement -= Time.fixedDeltaTime;
    //     }
    //Debug.Log(Vector3.Distance(enemyboss.transform.position,enemyboss.player.transform.position) );
       if (Vector3.Distance(enemyboss.transform.position,enemyboss.player.transform.position) < 10) {
        enemyboss.SetIsAttack(true);
        //IsAttack = true;
       }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

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
