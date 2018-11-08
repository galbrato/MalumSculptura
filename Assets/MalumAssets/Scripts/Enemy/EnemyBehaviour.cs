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

    private int TriggerEnterCounter = 0;

    public GameObject PoseFingida;
    public GameObject PoseJumpScare;

    public AudioSource StepAudioSource;

    private void Awake() {

        lastPosition = transform.position;

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
            Lanterna.instance.LightOn();
            myHead = Instantiate(new GameObject("Head"), transform).transform;
            myHead.position = new Vector3(0f, 1f, 0f);
        }
    }

    private Vector3 lastPosition ;
	// Update is called once per frame
	void Update () {
        
        if ((lastPosition - transform.position).magnitude > 0.01f) {
            Debug.Log("CORRE");
            if (!StepAudioSource.isPlaying) {
                StepAudioSource.Play();
            }
        } else {
            Debug.Log("PARADINHA AI");
            StepAudioSource.Stop();
        }

        lastPosition = transform.position;

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
        //calculando posição do player
        Vector3 playerPosition = Camera.main.transform.position;
        playerPosition.y = transform.position.y;


        //Calculadno posição da estatua
        Vector3 myPosition = transform.position;
        myPosition.y = transform.position.y;


        Vector3 dir = (myPosition - playerPosition);

        transform.position = (playerPosition + (dir.normalized * 0.95f));
        transform.LookAt(playerPosition);

        PoseFingida.SetActive(false);
        PoseJumpScare.SetActive(true);

        Camera.main.transform.LookAt(myHead);
        GetComponentInChildren<AudioSource>().Play();
        Invoke("GameOver", 1);
        Lanterna.instance.LightOn();
    }
   
    private void OnTriggerEnter(Collider other) {
        
        Debug.Log("colidi com o " + other.gameObject.name);
        if (other.CompareTag("Porta")) {
            other.gameObject.GetComponent<SuperficieInteragivel>().interacao3(transform);
        }

        if (other.gameObject.CompareTag("Player")) {
            TriggerEnterCounter++;
        }
        if (!stop && TriggerEnterCounter == 1) {
            Player.GetComponent<FirstPersonController>().enabled = false;
            Stop();
            EndGame = true;
            Lanterna.instance.LightOff();

            mAgent.enabled = false;
            StateMachine.enabled = false;
           
            Invoke("JumpScare", 2);


        }
    }
    private void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            TriggerEnterCounter--;
        }

        if (other.CompareTag("Porta")) {
            other.gameObject.GetComponent<SuperficieInteragivel>().interacao3(transform);
        }
    }

}
