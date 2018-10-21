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

		//interagir com objeto
		if(Input.GetMouseButton(0) ||Input.GetMouseButtonDown(0) ){
			RaycastHit hit;
			Physics.Raycast(trans.position, dirVisao, out hit, 100f);
			//Debug.DrawRay(trans.position, dirVisao * 10, Color.yellow);
			GameObject gmO = hit.collider.gameObject;

			if (gmO != null) {
				Debug.Log("PEI");
            	interagivel inte = gmO.GetComponent<interagivel>();
				if(inte != null){
					if(Input.GetMouseButton(0))
						inte.interagir();
					else	
						inte.interagir2();
				}
			}
		}
	}
}
