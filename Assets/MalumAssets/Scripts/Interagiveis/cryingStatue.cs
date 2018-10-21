using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cryingStatue : interagivel {
	public float maxTimer;

	float timer;

	cryingSpawn[] spawns;
	cryingSpawn spawnAtual;

	bool necessitaMudarPos = false;
	bool puto = false;

	public float distSpawn;//distancia minimamente longe do player para spawnar num ponto

	MeshRenderer mesh;

	private Vector3 posObservar;

	[HideInInspector]
	public bool observado = false;

	protected override void comeco(){
		spawns =  FindObjectsOfType<cryingSpawn>();
		mesh = gameObject.GetComponent<MeshRenderer>();
		timer = maxTimer;
		posObservar = new Vector3(trans.position.x,  trans.position.y,  trans.position.z ) ;

		//definindo posicao inicial
		bool aux = true;
		for(int i=0;i<20 && aux;i++){
			spawnAtual = spawns[ Random.Range(0,spawns.Length)];
			aux = (Vector3.Distance(spawnAtual.gameObject.transform.position, plaTrans.position) < distSpawn);
		}
		trans.position = spawnAtual.gameObject.transform.position;
	}

	public void setMaxTimer(float novoValor){
		maxTimer = novoValor;
	}

	//player interage com a estátua
	public void interagiracao(){
		timer = maxTimer;
		necessitaMudarPos = true;
	}

	void Update() {
		observado = mesh.isVisible;
        timer -= Time.deltaTime;

		if(timer <= 0f){
			puto = true;
		}

		//matar player
		if(observado){
			if(puto)
				Debug.Log("Jump Scare e Matar Player");
		}
		else{
			//mudar de posicao
			if(necessitaMudarPos){
				necessitaMudarPos = false;
				mudarDePos();
			}

			//olhando para o player
			posObservar.x = plaTrans.position.x;
			posObservar.z = plaTrans.position.z;
 			this.transform.LookAt( posObservar ) ;
			trans.eulerAngles = new Vector3(0f,trans.eulerAngles.y,0f);
		}

    }

	void mudarDePos(){
		cryingSpawn oldSpawn = spawnAtual;

		bool aux = true;
		for(int i=0;i<20 && aux;i++){

			spawnAtual = spawns[ Random.Range(0,spawns.Length)];

			//procurando spawn diferente do atual
			aux = (spawnAtual == oldSpawn);
			//procura spawn com distancia minima
			aux |= (Vector3.Distance(spawnAtual.gameObject.transform.position, plaTrans.position) < distSpawn);
			//procurando spawn nao observado(precisa ser modificado)
			aux |= (observado);

		}

		trans.position = spawnAtual.gameObject.transform.position;
	}

	void LateUpdate(){
		observado = false;
	}
}
