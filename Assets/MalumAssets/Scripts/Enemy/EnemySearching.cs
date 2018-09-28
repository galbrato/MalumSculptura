using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class EnemySearching : StateMachineBehaviour {
    private float counter;
    [SerializeField] float TimePrediction = 1;
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
        animator.SetBool("SearchingPlayer", true);
        counter = 0;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        counter += Time.deltaTime;
        if(counter < TimePrediction)mAgent.SetDestination(Player.position);
        if( (mAgent.destination - animator.transform.position).magnitude < mAgent.stoppingDistance+0.1f) {
            animator.SetBool("SearchingPlayer", false);
        }
    }
}
