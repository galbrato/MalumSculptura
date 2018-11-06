using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ponteiro1 : MonoBehaviour {
	[HideInInspector]
	public Relogio relogio;
	private Transform trans;

	private float cicloHora;

	//funcao de start chamada depois de achar relogio
	public void comeco(Relogio rel,float ciclo){
		relogio = rel;
		trans = GetComponent<Transform>();
		cicloHora = ciclo;
	}
	
	void Update(){
		trans.localEulerAngles = new Vector3(trans.localEulerAngles.x,90 + (relogio.tempo)/cicloHora* 30 , trans.localEulerAngles.z);

	}
}
