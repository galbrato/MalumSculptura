using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pinController : MonoBehaviour {

	float sliderHeightMin;
	float sliderHeightMax;
	float sliderHeightTarget;
	float sliderFallSpeed;
	RectTransform pin;
	RectTransform slider;

	// public for debugging only
	public float sliderHeight;
	// public for debugging only
	public float gravity = 0.3f;
	public bool ready;

	public bool tocarSomDestravamento = false; 
	public AudioSource audioClick;//audio quando pin entra na posicao certa

	void UpdateSlider() {
		if(sliderHeightTarget > 0 && sliderHeight < sliderHeightTarget) {
			sliderFallSpeed = 0f;
			sliderHeight = sliderHeightTarget;
			if(tocarSomDestravamento){
				audioClick.Play();
				tocarSomDestravamento = false;
			}
		}
		if(sliderHeight < sliderHeightMin){
			sliderFallSpeed = 0f;
			sliderHeight = sliderHeightMin;
			ready = true;
		}
		if(sliderHeight > sliderHeightMax){
			sliderFallSpeed = 0f;
			sliderHeight = sliderHeightMax;
		}
		slider.anchoredPosition = new Vector2(slider.anchoredPosition.x, sliderHeight);
	}

	public void PushSlider() {
		sliderFallSpeed = 0;
		sliderHeight = sliderHeightMax;
		ready = false;
	}

	public void PushSliderToTarget() {
		sliderFallSpeed = 0;
		gravity += 0.2f;
		sliderHeight = sliderHeightMax;
		ready = false;
		tocarSomDestravamento = true;
		sliderHeightTarget = pin.rect.height/2f;
	}

	// Use this for initialization
	void Start () {
		// finding Slider slider component
		pin = GetComponent<RectTransform>();
		slider = pin.GetChild(0).GetComponent<RectTransform>();

		// finding Slider slider min and max height
		sliderHeightMax = pin.rect.height - slider.rect.height/2f;
		sliderHeightMin = slider.rect.height/2f;

		// setting starting position for slider
		sliderHeight = sliderHeightMin;

		sliderHeightTarget = -100;
		
	}
	
	// Update is called once per frame
	void Update () {
		// Updates slider transform
		UpdateSlider();
	}

	void FixedUpdate() {
		// slider falls after some time
		sliderFallSpeed += gravity;
		sliderHeight -= sliderFallSpeed;
	}
}