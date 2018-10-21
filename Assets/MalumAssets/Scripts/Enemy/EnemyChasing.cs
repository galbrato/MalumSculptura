using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class EnemyChasing : StateMachineBehaviour {
    private bool firstEnter = true;
    private Transform Player;
    private NavMeshAgent mAgent;
	
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (firstEnter) {
            Player = GameObject.FindGameObjectWithTag("Player").transform;
            mAgent = animator.gameObject.GetComponent<NavMeshAgent>();
            firstEnter = false;
        }
        mAgent.enabled = true;
    }

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if(mAgent.isActiveAndEnabled)mAgent.SetDestination(Player.position);
        if ((Player.position - animator.transform.position).magnitude < 0.1f) {
            Debug.Log("BUUU!, Morreu player");
            SceneManager.LoadScene(0);
        }
	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	//override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}
}
