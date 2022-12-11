using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnragedRun : StateMachineBehaviour
{
    bool IsAttack = false;
    EnemyBossHealth enemyboss;
    public float attackDistance;
    // GameObject player;
    // public NavMeshAgent agent;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       //player = GameObject.FindGameObjectWithTag("Player");
       enemyboss = animator.GetComponent<EnemyBossHealth>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Vector3.Distance(enemyboss.agent.transform.position,enemyboss.player.transform.position) < attackDistance) {
            enemyboss.agent.ResetPath();
            animator.SetTrigger("Attacking");
        } else {
            enemyboss.agent.SetDestination(enemyboss.player.transform.position);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       
    }
}
