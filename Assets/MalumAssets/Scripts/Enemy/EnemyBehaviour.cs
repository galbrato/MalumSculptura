using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour {
    public bool debug = true;
	public bool Stop = false;
    public float SightRange = 10;
    
    private NavMeshAgent MyNav;
    private Animator MyAnimator;
    private Transform Player;
    private Animator StateMachine;
    private void Awake() {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        MyAnimator = GetComponent<Animator>();
        StateMachine = GetComponent<Animator>();
    }

    // Use this for initialization
    void Start () {
		MyNav = GetComponent<NavMeshAgent>();
		MyNav.destination = Player.transform.position; 
	}
	
	// Update is called once per frame
	void Update () {
        StateMachine.SetBool("CanSeePlayer", CanSeePlayer());

		if (Stop) {
			MyNav.enabled = false;
            MyAnimator.enabled = false;
		} else {
            MyNav.enabled = true;
            MyAnimator.enabled = true;
		}
		Stop = false;
	}

    private bool CanSeePlayer() {
        RaycastHit hit;
        Vector3 dir = Player.position - transform.position;
        if(!Physics.Raycast(transform.position, dir, out hit, SightRange)) {
            return false;
        }
        
        if (debug) Debug.DrawRay (transform.position, dir.normalized * hit.distance, Color.red);

        return hit.collider.CompareTag("Player");
    }
}
