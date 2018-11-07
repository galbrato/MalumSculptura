using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ponteiro1 : MonoBehaviour {
	[HideInInspector]
	public Relogio relogio;
	private float umCiclo;

	//funcao de start chamada depois de achar relogio
	public void comeco(int divisaoDoCiclo, Relogio rel){
		relogio = rel;
		umCiclo = relogio.tempoDeJogo / divisaoDoCiclo;
	}
	
	void Update(){
		transform.localEulerAngles = new Vector3(transform.localEulerAngles.x,((relogio.tempo) % umCiclo / umCiclo) * 360, transform.localEulerAngles.z);

	}
}
