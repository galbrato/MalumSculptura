using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cryingStatue : interagivel {
	public float maxTimer;

	public float tempoEntreTeleportes;
	float timer;
	bool cooldown = false;
	cryingSpawn[] spawns;
	cryingSpawn spawnAtual;

	public MeshRenderer posInicial;//posição onde será teleportada pela primeira vez a cryingStatue
	public MeshRenderer posSegunda;//posição que será teleportada pela segunda vez a cryingStatue

	bool necessitaMudarPos = false;
	int nivelChoro = 0;//0=sem choro;1=com choro;2=puto
	public Transform posVoid;//posicao caso fora da cena

    //variaveis para iniciar a estatua amaldiçoada
    public EnemyBehaviour CursedStatue;
    private bool FirstTime = true;

	private Relogio relogio;
	public bool ativadaUmaVez = false;
	[HideInInspector]
	private bool ativadaDuasVezes = false;
	private bool ativadoCorotina = false;
	private BoxCollider m_colider;

	protected override void comeco(){
        if(CursedStatue == null) {
            Debug.LogError("ME DA REFERENCIA PARA A ESTATUA AMALDIÇOADA");
        }
		spawns =  FindObjectsOfType<cryingSpawn>();
		m_colider = GetComponent<BoxCollider>();
		EntrarVoid();
	}



	//player interage com a estátua
	public override void interacao2(){
		timer = maxTimer;
		nivelChoro = 0;
		necessitaMudarPos = true;
		textInteragir = "";
		m_colider.enabled = false;

	}

	void Update() {
		//Debug.Log(teleportes);
		timer -= Time.deltaTime;

		if(timer <= maxTimer * 0.85f){

			if(nivelChoro == 0 ){
				textInteragir = "Enxugar lágrimas";
				nivelChoro = 1;
			}
			
			if(timer <= 0f){
				nivelChoro = 2;
				textInteragir = "";
				KillPlayer kill = GetComponent<KillPlayer>();
				kill.enabled = true;
				Debug.Log("Jump Scare e Matar Player");
			}
		}


		//mudar de posicao
		if(necessitaMudarPos){
			piscaLanterna();
		}


    }

	void mudarDePos(){

		cryingSpawn oldSpawn = spawnAtual;
		timer = maxTimer;

		bool aux = true;
		for(int i=0;i<30 && aux;i++){
			
			spawnAtual = spawns[ Random.Range(0,spawns.Length)];

//			Transform oq = spawnAtual.gameObject.GetComponent<Transform>();
			//procurando spawn diferente do atual
			aux = (spawnAtual == oldSpawn);
			//procura spawn com distancia minima
			aux |= (spawnAtual.obsertado());

		}

		textInteragir = "Enxugar lágrimas";
		necessitaMudarPos = false;
		trans.position = spawnAtual.gameObject.transform.position;
		spawnAtual.teleporte();
		m_colider.enabled = true;
	}

	//tenta teleportar crying statue caso spawn já definido
	public void mudarDePos(MeshRenderer spawn){


		textInteragir = "Enxugar lágrimas";
		timer = maxTimer;
		//Debug.Log(spawn.gameObject.transform.position);
		trans.position = spawn.gameObject.transform.position;
		
		//Debug.Log("oque");
		necessitaMudarPos = false;
		spawn.gameObject.GetComponent<cryingSpawn>().teleporte();

		if(ativadaDuasVezes)
			StartCoroutine(mudarDePosicaoPeriodicamente());
		
		m_colider.enabled = true;
	}


	public void atualizarCooldown(){
		cooldown = false;
	}
	public void piscaLanterna() {

		if(cooldown == true)
			return;
		
		cooldown = true;
		Invoke("atualizarCooldown",1.2f);
		necessitaMudarPos = false;


        Invoke("piscaLanterna2", 1f);
    }
    private void piscaLanterna2(){
        Lanterna.instance.LightOff();


		//caso termina primeiro teleporte, ela irá na hora na nova posicao
		if(!ativadaUmaVez){
			mudarDePos(posInicial);
			ativadaUmaVez = true;
		}
		else if(!ativadaDuasVezes){
			mudarDePos(posSegunda);
			ativadaDuasVezes = true;
		}else{
		//caso contrário vá para o void

			EntrarVoid();
		}
        Invoke("piscaLanterna3", 0.1f);
    }
    private void piscaLanterna3(){
        Lanterna.instance.LightOn();
    }


IEnumerator mudarDePosicaoPeriodicamente() {

    while(nivelChoro != 2) {
        mudarDePos();

        yield return new WaitForSeconds(tempoEntreTeleportes);
    }
}


    private void EntrarVoid() {

        timer = maxTimer * 10;//só para ter certeza

        trans.position = posVoid.position;

        if (ativadaUmaVez && ativadaDuasVezes && (!ativadoCorotina)) {
            StartCoroutine(mudarDePosicaoPeriodicamente());
			ativadoCorotina = true;
            if (FirstTime) {
                FirstTime = false;
                CursedStatue.enabled = true;
            }
        }
	}
}

