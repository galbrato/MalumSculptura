using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.Gaming;

public class Temp : MonoBehaviour {
	GazeAware x;

	void  Start() {
		x = gameObject.GetComponent<GazeAware> ();
	}

	void Update () {
		if(x.HasGazeFocus) {
			Debug.Log (gameObject.tag);
		}
	}
}
