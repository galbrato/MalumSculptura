using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class lockPickController : MonoBehaviour {

	int selected;
	Stack<int> order;
	pinController[] pins;
	public RectTransform cursor;
	float targetPosition;
	public float speed;
	Animator anim;
	FirstPersonController firstPerson;
	Lanterna lanterna;

	// Cycle between pins
	void CycleRight() {
		if(selected + 1 < pins.Length) selected++;
		// Set new cursor target position
		targetPosition = pins[selected].GetComponent<RectTransform>().anchoredPosition.x + 85;
		speed = (targetPosition - cursor.anchoredPosition.x)/5f;
	}

	void CycleLeft() {
		if(selected > 0) selected--;
		// Change cursor target position
		targetPosition = pins[selected].GetComponent<RectTransform>().anchoredPosition.x + 85;
		speed = (targetPosition - cursor.anchoredPosition.x)/5f;
	}

	void PushPin() {
		if(selected == order.Peek()) {
			pins[selected].PushSliderToTarget();
			order.Pop();
		} else {
			pins[selected].PushSlider();
		}
	}

	// Use this for initialization
	void Start () {
		selected = 0;
		speed = 0;
		pins = GetComponentsInChildren<pinController>();
		anim = GetComponent<Animator>();
		firstPerson= FindObjectOfType<FirstPersonController>();
		lanterna = FindObjectOfType<Lanterna>();

		// Building new random order
		order = new Stack<int>();
		int rand;
		while(order.Count != pins.Length){
			rand = (int) Random.Range(0f, pins.Length);
			if(!order.Contains(rand)){
				order.Push(rand);
			}
		}

		firstPerson.enabled = false;
		lanterna.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		// Checking win condition
		if(order.Count <= 0) {
			lanterna.enabled = true;
			firstPerson.enabled = true;
			Destroy(gameObject);
		}

		// If i'm not moving, allow moving the pick
		if(speed == 0){
			if(Input.GetAxisRaw("Horizontal") == -1) CycleLeft();
			if(Input.GetAxisRaw("Horizontal") == 1) CycleRight();
		}
		// If the current pin is idle, allow me to push it 
		if(Input.GetButtonDown("Fire1") && pins[selected].ready) anim.SetTrigger("Pushpin");
	}

	void FixedUpdate() {
		if(cursor.anchoredPosition.x == targetPosition) speed = 0;
		else cursor.anchoredPosition = new Vector2(cursor.anchoredPosition.x + speed, cursor.anchoredPosition.y);
	}
}
