using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Relogio : interagivel {
	public float tempoDeJogo = 100f;//tempo da fase
	float tempo;

	public float tempoAtivoMax = 100f;//tempo até necessitar dar corda no relogio
	
	float tempoAtivo ;

	public float DandoCordaTempo;//tempo necessario para player interagir com relogio
	private float dandoCordaT;

	private UnityStandardAssets.Characters.FirstPerson.FirstPersonController  plaFpc;


	protected override void comeco() {
		tempo = tempoDeJogo;
		tempoAtivo = tempoAtivoMax;
		dandoCordaT = -100;//caso -100 esta desativado o timer

		plaFpc = pla.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController >();	

		if(DandoCordaTempo < 0f)
			Debug.Log("coloque o dandoCordaTempo do relogio para um valor POSITIVO, obg");
	}
	
	//player da corda no relógio
	private  void daCorda(){
		tempoAtivo =  tempoAtivoMax;
		dandoCordaT = -100;//caso valor igual a -100, esta desativado
		plaFpc.enabled = !plaFpc.enabled;
	}

	public override void interacao(){
		//caso ativo o timer dando corda esteja ativo, evite interagir
		if(dandoCordaT != -100)
			return;

		dandoCordaT = DandoCordaTempo;
		plaFpc.enabled = !plaFpc.enabled;
	}
	void Update () {

		//caso relogio ativo
		if(tempoAtivo > 0f){
			tempoAtivo -= Time.deltaTime; 
			tempo -= Time.deltaTime;

			//termino do jogo
			if(tempo <= 0f){
				Debug.Log("acaba o jogo");
			}
		}

		//dando corda no relogio
		if(dandoCordaT <= 0f){
			if(dandoCordaT != -100)//caso -100 eh o neutro(sem timer)
				daCorda();
		}else{
			//continua o timer
			dandoCordaT -= Time.deltaTime;
		}

	}
}
