using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Relogio : interagivel {
	public float tempoDeJogo = 100f;//tempo da fase
	[HideInInspector]
	public float tempo;

	public float tempoAtivoMax = 100f;//tempo até necessitar dar corda no relogio
	
	float tempoAtivo ;

	public float DandoCordaTempo;//tempo necessario para player interagir com relogio
	private float dandoCordaT;

	private UnityStandardAssets.Characters.FirstPerson.FirstPersonController  plaFpc;
	public Ponteiro1 pontP;//transform com objeto ponteiro pequeno
	public Ponteiro1  pontG;//transform com objeto ponteiro grande
	protected override void comeco() {
		//colocando valores iniciais
		tempo = tempoDeJogo;
		tempoAtivo = tempoAtivoMax;
		dandoCordaT = -100;//caso -100 esta desativado o timer

		plaFpc = pla.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController >();	

		if(DandoCordaTempo < 0f)
			Debug.Log("coloque o dandoCordaTempo do relogio para um valor POSITIVO, obg");


		pontP.comeco(6, GetComponent<Relogio>());
		pontG.comeco(6*60, GetComponent<Relogio>());
	}
	
	//player da corda no relógio
	private  void daCorda(){
		tempoAtivo =  tempoAtivoMax;
		dandoCordaT = -100;//caso valor igual a -100, esta desativado
		plaFpc.enabled = !plaFpc.enabled;
		pontG.ativo = true;
		pontP.ativo = true;
	}

	public override void interacao(){
		//caso ativo o timer dando corda esteja ativo, evite interagir
		if(dandoCordaT != -100)
			return;

		dandoCordaT = DandoCordaTempo;
		plaFpc.enabled = !plaFpc.enabled;
	}
	void Update () {

	print(tempo);
	if(pontP == null){
		print(gameObject.name +":((");
	}
		//caso relogio ativo
		if(tempoAtivo > 0f){
			tempoAtivo -= Time.deltaTime; 
			tempo -= Time.deltaTime;


			//termino do jogo
			if(tempo <= 0f){
				Debug.Log("acaba o jogo");
			}
		}else{
			pontG.ativo = false;
			pontP.ativo = false;
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
