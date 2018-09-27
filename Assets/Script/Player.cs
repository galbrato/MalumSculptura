using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	private Transform trans;

	[HideInInspector]
	public Vector3 dirVisao;

	void Start () {
		trans = GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Space) ){

			RaycastHit hit;
			Physics.Raycast(trans.position, dirVisao, out hit, 100f);

			Debug.DrawRay(trans.position, dirVisao * 10, Color.yellow);
			if (hit.collider.gameObject != null) {
				GameObject gmO = hit.collider.gameObject;
            	interagivel inte = gmO.GetComponent<interagivel>();
				if(inte != null)
					inte.interagir();
			}
		}
	}
}
