using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cryingStatue : interagivel {
	public float maxTimer;

	float timer;

	cryingSpawn[] spawns;
	cryingSpawn spawnAtual;

	public MeshRenderer posInicial;//posição onde será teleportada pela primeira vez a cryingStatue
	public MeshRenderer posSegunda;//posição que será teleportada pela segunda vez a cryingStatue
	private int teleportes = 0;//quantos teleportes a cryingStatue já fez

	bool necessitaMudarPos = true;
	int nivelChoro = 0;//0=sem choro;1=com choro;2=puto

	public  Renderer m_Renderer;
	public Material texturaChoro;
	public Material texturaSemChoro;
	MeshRenderer mesh;


	[HideInInspector]
	public bool observado = false;

	protected override void comeco(){
		spawns =  FindObjectsOfType<cryingSpawn>();
		mesh = gameObject.GetComponent<MeshRenderer>();
		timer = maxTimer * 0.75f;
	}

	public void setMaxTimer(float novoValor){
		maxTimer = novoValor;
	}


	//player interage com a estátua
	public override void interacao2(){

		timer = maxTimer;
		nivelChoro = 0;
		necessitaMudarPos = true;
		m_Renderer.material = texturaSemChoro;
		textInteragir = "";
	}

	void Update() {
		observado = mesh.isVisible;

		timer -= Time.deltaTime;

		if(timer <= maxTimer * 0.75f){
			//mudando testura
			if(nivelChoro == 0 && !observado){
				m_Renderer.material = texturaChoro;
				textInteragir = "Enxugar lágrimas";
				nivelChoro = 1;
			}
			
			if(timer <= 0f && nivelChoro == 1){
				nivelChoro = 2;
			}
		}

		//matar player
		if(observado){
			if(nivelChoro == 2 && Vector3.Distance(trans.position, plaTrans.position) < distMin*1.5)
				Debug.Log("Jump Scare e Matar Player");
		}
		else{
			//mudar de posicao
			if(necessitaMudarPos){
				if(teleportes>1)
					mudarDePos();
				else if(teleportes==1){
					mudarDePos(posSegunda);
				}
				else if(teleportes==0){
					mudarDePos(posInicial);
				}
			}

		}

    }

	void mudarDePos(){
		cryingSpawn oldSpawn = spawnAtual;

		bool aux = true;
		for(int i=0;i<30 && aux;i++){
			
			spawnAtual = spawns[ Random.Range(0,spawns.Length)];

			Transform oq = spawnAtual.gameObject.GetComponent<Transform>();
			//procurando spawn diferente do atual
			aux = (spawnAtual == oldSpawn);
			//procura spawn com distancia minima
			aux |= (spawnAtual.obsertado());

		}
		if(aux == false){
			necessitaMudarPos = false;
			trans.position = spawnAtual.gameObject.transform.position;
			spawnAtual.teleporte();
			teleportes++;
		}
	}

	//tenta teleportar crying statue caso spawn já definido
	void mudarDePos(MeshRenderer spawn){

		if(!spawn.isVisible && !observado){
			trans.position = spawn.gameObject.transform.position;
			teleportes++;
			necessitaMudarPos = false;
		}
	}
	void LateUpdate(){
		observado = false;
	}
}

