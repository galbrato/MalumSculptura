using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour {
	public bool Stop = false;
	private NavMeshAgent MyNav;
	public GameObject Player;
	// Use this for initialization
	void Start () {
		MyNav = GetComponent<NavMeshAgent>();
		MyNav.destination = Player.transform.position; 
	}
	
	// Update is called once per frame
	void Update () {
		if (Stop) {
			MyNav.enabled = false;
		} else {
			MyNav.enabled = true;
			MyNav.destination = Player.transform.position;
		}
		Stop = false;
	}
}
