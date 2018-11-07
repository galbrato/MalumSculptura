using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {
	private Transform trans;
	[HideInInspector]
	public Vector3 dirVisao;

	private bool getmousebutton;
	private bool getmousebuttondown;
	private Text txtObjInteragivel;
	void Start () {
		trans = GetComponent<Transform>();
		txtObjInteragivel = GameObject.Find("txtObjInteragivel").GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		txtObjInteragivel.text = "";
		getmousebutton = Input.GetMouseButton(1);
		getmousebuttondown = Input.GetMouseButtonDown(1);

		//lancando um raycast, dectando objeto interagivel
		RaycastHit hit;
		Physics.Raycast(trans.position, Camera.main.transform.forward, out hit, 100f);

		GameObject gmO = hit.collider.gameObject;
		if (gmO != null) {
         	interagivel inte = gmO.GetComponent<interagivel>();
			if(inte != null){

				//mostrando texto do objeto interagivel
				inte.apontado( txtObjInteragivel);
				//interagindo
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
