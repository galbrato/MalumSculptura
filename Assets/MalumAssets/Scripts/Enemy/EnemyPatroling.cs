using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class EnemyPatroling : StateMachineBehaviour {
    private bool firstEnter = true;
    private NavMeshAgent mAgent;
    private List<Transform> StatueSpots;
    private Transform DestinationSpot;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (firstEnter) {
            mAgent = animator.gameObject.GetComponent<NavMeshAgent>();
            firstEnter = false;

            StatueSpots = new List<Transform>();

            GameObject[] Spots = GameObject.FindGameObjectsWithTag("StatueSpot");
            if(Spots.Length < 1) {
                Debug.LogError("ERRO, esta faltando objetos na cena com a tag StatueSpot");
                animator.enabled = false;
            } else {
                foreach (GameObject s in Spots) {
                    StatueSpots.Add(s.transform);
                }
            }
        }
        DestinationSpot = StatueSpots[Random.Range(0, StatueSpots.Count)];
        mAgent.SetDestination(DestinationSpot.position);
    }

    private void Awake() {
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if ((animator.transform.position - DestinationSpot.position).magnitude < 2f) {
            DestinationSpot = StatueSpots[Random.Range(0, StatueSpots.Count)];
            mAgent.SetDestination(DestinationSpot.position);
        }
        mAgent.SetDestination(DestinationSpot.position);
        Debug.Log("my destination:" + DestinationSpot.position);
    }

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        
	//
	}

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	}
}
