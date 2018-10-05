using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour {
    [SerializeField] float Speed = 3.5f;
    [SerializeField] bool debug = true;
    public float SightRange = 10;
    
    private Transform Player;
    private Animator StateMachine;
    private NavMeshAgent mAgent;

    private void Awake() {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        mAgent = GetComponent<NavMeshAgent>();
        StateMachine = GetComponent<Animator>();
    }

	// Update is called once per frame
	void Update () {
        StateMachine.SetBool("CanSeePlayer", CanSeePlayer());
          mAgent.speed = 3.5f;

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

    public void Stop() {
        //StateMachine.SetBool("Stop", true);
        mAgent.speed = 0;
       
    }
    private void OnCollisionEnter(Collision collision) {
        Debug.Log("colidi com o " + collision.gameObject.name);
        if (collision.gameObject.CompareTag("Player")) {
            Debug.Log("BUuuuu, perdeu playba");
        }
    }

}
