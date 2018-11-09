using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine;

public class KillPlayer : MonoBehaviour{
    NavMeshAgent mAgent;
    public float JumpScareDistance = 1f;
    Transform myHead;
    private FirstPersonController player;
    public AudioSource Grito;
    bool morreu = false;
    public GameObject PoseChorando;
    public GameObject PoseAtacando;

    // Start is called before the first frame update
    void Start(){
        //Invoke("doit", 0.1f);
        
    }

    private void OnEnable() {
        doit();
    }

    void doit() {
        GetComponent<BoxCollider>().enabled = false;

        PoseChorando.SetActive(false);
        PoseAtacando.SetActive(true);

        mAgent = GetComponent<NavMeshAgent>();
        mAgent.enabled = true;
        mAgent.SetDestination(Camera.main.transform.position);
        Lanterna.instance.LightOff();
        player = FindObjectOfType<FirstPersonController>();
        player.enabled = false;
        Transform[] childrens = gameObject.GetComponentsInChildren<Transform>();
        foreach (Transform item in childrens) {
            if (item.name == "Head") {
                myHead = item;
                break;
            }
        }
        Lanterna.instance.enabled = false;

    }

    // Update is called once per frame
    void Update(){
        if (morreu) return;
        //calculando posição do player
        Vector3 playerPosition = Camera.main.transform.position;
        playerPosition.y = transform.position.y;

        mAgent.SetDestination(playerPosition);
       
        //Calculadno posição da estatua
        Vector3 myPosition = transform.position;
        myPosition.y = transform.position.y;

        Camera.main.transform.LookAt(myHead);

        Vector3 dir = (myPosition - playerPosition);
        mAgent.speed = 3 + 1.5f*dir.magnitude;
        if (dir.magnitude <=JumpScareDistance ) {
            
            transform.position = (playerPosition + (dir.normalized * JumpScareDistance));
            transform.LookAt(playerPosition);
            //enabled = false;
            Lanterna.instance.LightOn();
            Lanterna.instance.seekMouse = false;
            mAgent.speed = 0;
            if(!morreu) Grito.Play();
            morreu = true;
        }
    }
    
}
