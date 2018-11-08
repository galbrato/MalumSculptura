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
	private int teleportes = 0;//quantos teleportes a cryingStatue já fez;caso -1, ela não dará mais teleportes

	bool necessitaMudarPos = true;
	int nivelChoro = 0;//0=sem choro;1=com choro;2=puto




	protected override void comeco(){
		spawns =  FindObjectsOfType<cryingSpawn>();
		timer = maxTimer * 0.85f;
		mudarDePos(posInicial);
	}



	//player interage com a estátua
	public override void interacao2(){
		if(nivelChoro != 0){
			timer = maxTimer;
			nivelChoro = 0;
			necessitaMudarPos = true;
			textInteragir = "";
		}
	}

	void Update() {

		Debug.Log(teleportes+" "+timer);
		timer -= Time.deltaTime;

		if(timer <= maxTimer * 0.85f){

			if(nivelChoro == 0 ){
				textInteragir = "Enxugar lágrimas";
				nivelChoro = 1;
			}
			
			if(timer <= 0f){
				nivelChoro = 2;
				teleportes = -1;
				textInteragir = "";
				KillPlayer kill = GetComponent<KillPlayer>();
				kill.enabled = true;
				Debug.Log("Jump Scare e Matar Player");
			}
		}


		//mudar de posicao
		if(teleportes > 0 && necessitaMudarPos){
			piscaLanterna();
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

		if(!spawn.isVisible ){
			trans.position = spawn.gameObject.transform.position;
			teleportes++;
			necessitaMudarPos = false;
		}
	}




	private void piscaLanterna() {
		necessitaMudarPos = false;

        Invoke("piscaLanterna2", 1f);
    }
    private void piscaLanterna2(){
        Lanterna.instance.LightOff();

		Debug.Log("q");

		if(teleportes>1)
			mudarDePos();
		else if(teleportes==1){
			mudarDePos(posSegunda);
		}
		else if(teleportes==0){
			mudarDePos(posInicial);
		}
        Invoke("piscaLanterna3", 0.1f);
    }
    private void piscaLanterna3(){
        Lanterna.instance.LightOn();
    }
}

