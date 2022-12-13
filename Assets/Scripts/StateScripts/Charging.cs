using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Charging : StateMachineBehaviour
{
    bool isStun = false;
      //  GameObject player;
      //  public NavMeshAgent agent;
    float timeTilNextMovement;
    public float StopDistance;
    EnemyBossHealth enemyboss;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       //player = GameObject.FindGameObjectWithTag("Player");
       enemyboss = animator.GetComponent<EnemyBossHealth>();    
       timeTilNextMovement = 10f;
       if (enemyboss != null) {
         enemyboss.StopCoroutinesFunc();
         enemyboss.DoChargeLaser(timeTilNextMovement);
       }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
      if (Vector3.Distance(enemyboss.transform.position,enemyboss.player.transform.position) < StopDistance) {
        enemyboss.agent.ResetPath();
        //IsAttack = true;
       } else enemyboss.agent.SetDestination(enemyboss.player.transform.position);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
// public class ChargingCoroutine : MonoBehaviour {
//      public void DoCoroutine(float delay) {
//         StartCoroutine(Wait(delay));
//      }
//      public void StopAllCoroutinesFunc() {
//         StopAllCoroutines();
//     }
//     IEnumerator Wait(float delay) {
//         yield return new WaitForSeconds(delay);
//         GetComponent<Animator>().SetTrigger("ddf"); // demo
//     }
//  }  
