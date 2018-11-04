using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using UnityStandardAssets.Characters.FirstPerson;

public class EnemyBehaviour : MonoBehaviour {
    [SerializeField] float Speed = 3.5f;
    [SerializeField] bool debug = true;
    public float SightRange = 10;
    public bool stop;

    private bool EndGame;
    
    private Transform Player;
    private Animator StateMachine;
    private NavMeshAgent mAgent;

    Transform myHead;

    private void Awake() {
        EndGame = false;

        Player = GameObject.FindGameObjectWithTag("Player").transform;
        mAgent = GetComponent<NavMeshAgent>();
        StateMachine = GetComponent<Animator>();
        stop = false;
        
        Transform[] childrens = gameObject.GetComponentsInChildren<Transform>();
        foreach (Transform item in childrens) {
            if(item.name == "Head") {
                myHead = item;
                break;
            }
        }
        if (myHead == null) {
            Debug.LogError("ERRO, a estatua " +name+" não possui um objeto Head!");
            myHead = Instantiate(new GameObject("Head"), transform).transform;
            myHead.position = new Vector3(0f, 1f, 0f);
        }
    }

	// Update is called once per frame
	void Update () {
        StateMachine.SetBool("CanSeePlayer", CanSeePlayer());
        if (stop) {
            if(!EndGame)stop = false;
        } else {
            mAgent.speed = Speed;
        }

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
        stop = true;
        mAgent.speed = 0;

    }

    public void GameOver() {
        //SceneManager.LoadScene(0);
    }

    public void JumpScare() {
        GetComponentInChildren<AudioSource>().Play();
        Camera.main.transform.LookAt(myHead);
        Invoke("GameOver", 2);
        Lanterna.instance.LightOn();
    }

    private void OnCollisionEnter(Collision collision) {
        Debug.Log("colidi com o " + collision.gameObject.name);
        if (!stop && collision.gameObject.CompareTag("Player")) {
            Invoke("JumpScare", 2);
            Player.GetComponent<FirstPersonController>().enabled = false;
            Stop();
            EndGame = true;
            Lanterna.instance.LightOff();
        }
    }

}
