using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ponteiro1 : MonoBehaviour {
	[HideInInspector]
	public bool ativo = true;
	[HideInInspector]
	public Relogio relogio;
	private Transform trans;
	private float umCiclo;

	//funcao de start chamada depois de achar relogio
	public void comeco(int divisaoDoCiclo, Relogio rel){
		relogio = rel;
		trans = GetComponent<Transform>();
		umCiclo = relogio.tempoDeJogo / divisaoDoCiclo;
	}
	
	void Update(){
		if(!ativo)
			return;

		trans.localEulerAngles = new Vector3(trans.localEulerAngles.x,trans.localEulerAngles.y,((relogio.tempo) % umCiclo / umCiclo) * 360);

	}
}
