using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.Gaming;

public class Lanterna : MonoBehaviour {

	private Light myLight;
	public int nRays = 8;
	public int nCircles = 3;
	public int addicionalAngle = 5;
	private EnemyBehaviour saveEnemy;
	public bool debug = false;
	void Start() {
		myLight = gameObject.GetComponent<Light>();
	}

	void Update () {
		Vector3 olhando;
		Ray ray;

		if ((!TobiiAPI.GetDisplayInfo ().IsValid) || (!TobiiAPI.GetUserPresence ().IsUserPresent ())) {
			ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		} else {
			olhando = new Vector3 (TobiiAPI.GetGazePoint ().Screen.x, TobiiAPI.GetGazePoint ().Screen.y, 0f);
			ray = Camera.main.ScreenPointToRay (olhando);

			//se tiver eyetracker, mas quiser pausar porque perdeu os olhos, fazer akie
			/*if (TobiiAPI.GetDisplayInfo ().IsValid) {
				Debug.Log ("Seus olhos não podem ser detectados");
			}*/
		}

		gameObject.transform.rotation = Quaternion.LookRotation (ray.direction);

		Vector3 dir = ray.direction;
		RayDetection (dir, myLight.range);
		for (int c = 1; c < nCircles; c++) {
            
			Vector3 dir2 = Quaternion.AngleAxis (((myLight.spotAngle + addicionalAngle) / (2 * nCircles)) *c, transform.up)*dir;
			int nRaysAux = 1 + ((nRays - 1) / (nCircles - 1)) * c;
            for (int i = 0; i < nRaysAux; i++) {
				//rotacionar
				Vector3 dir3 = Quaternion.AngleAxis (i * (360f / nRaysAux), transform.forward) * dir2;
				RayDetection (dir3, myLight.range);
				//mandar o inimigo ficar paradão
			}	
		}

	}

	string RayDetection(Vector3 direction, float maxDist){

        RaycastHit hit;
		if (Physics.Raycast(transform.position, direction, out hit, maxDist)) {
		    if(debug)Debug.DrawRay (transform.position, direction * hit.distance, Color.green);
            if (hit.collider.gameObject.tag == "Enemy"){
				if (!saveEnemy)
					saveEnemy = hit.collider.gameObject.GetComponent<EnemyBehaviour> ();
				saveEnemy.Stop = true;
			}
		}
		return "FODEU DETECTOU NADA";
	}
}
