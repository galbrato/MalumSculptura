using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	private Transform trans;
	[HideInInspector]
	public Vector3 dirVisao;

	private bool getmousebutton;
	private bool getmousebuttondown;
	void Start () {
		trans = GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {

		getmousebutton = Input.GetMouseButton(0);
		getmousebuttondown = Input.GetMouseButtonDown(0);
		//interagir com objeto
		if(getmousebutton ||getmousebuttondown ){
			RaycastHit hit;
			Physics.Raycast(trans.position, dirVisao, out hit, 100f);
			//Debug.DrawRay(trans.position, dirVisao * 10, Color.yellow);
			GameObject gmO = hit.collider.gameObject;
			if (gmO != null) {
            	interagivel inte = gmO.GetComponent<interagivel>();
				if(inte != null){
					if(getmousebutton){
						inte.interagir();
					}
					if(getmousebuttondown){
						inte.interagir2();
					}
				}
			}
		}
	}
}
