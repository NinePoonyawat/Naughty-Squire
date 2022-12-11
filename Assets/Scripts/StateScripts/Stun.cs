using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stun : StateMachineBehaviour
{
    float timeTilNextMovement;
    EnemyBossHealth enemyboss;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       enemyboss = animator.GetComponent<EnemyBossHealth>();    
       timeTilNextMovement = 2f;
       if (enemyboss != null) {

         enemyboss.DoStunWait(timeTilNextMovement);
       }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       enemyboss.StopAllCoroutinesFunc();
    }
}


// public class StunCoroutine : MonoBehaviour {
//      public void DoCoroutine(float delay) {
//         StartCoroutine(Wait(delay));
//      }
//      public void StopAllCoroutinesFunc() {
//         StopAllCoroutines();
//     }
//     IEnumerator Wait(float delay) {
//         yield return new WaitForSeconds(delay);
//         GetComponent<Animator>().SetBool("isStun",false);
//     }
//  } 
