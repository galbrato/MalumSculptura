using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Relogio : interagivel {
	public float tempoDeJogo = 100f;//tempo da fase
	[HideInInspector]
	public float tempo;

	public float tempoAtivoMax = 100f;//tempo até necessitar dar corda no relogio
	public float forcaDarCorda = 5;//quanto tempoAtivo é ganho quando de da corda
	float tempoAtivo ;

	bool pdDarCorda1 = true;//cooldown para dar corda caso chegue no tempoAtivoMax
	bool pdDarCorda2 = true;//cooldown para dar corda para sair som de forma certa

	private UnityStandardAssets.Characters.FirstPerson.FirstPersonController  plaFpc;
	public Ponteiro1 pontP;//transform com objeto ponteiro pequeno
	public Ponteiro1  pontG;//transform com objeto ponteiro grande
	
	public AudioClip[] winding;
	public AudioSource audioWinding;
	public AudioSource audioTick;
	//public AudioSource audioWindingMax;
	public AudioSource audioBatidaHora;
	int horaAtual ;

	protected override void comeco() {

		if(pontP == null){
			print(gameObject.name +"não apresenta script ponteiro");
		}else if(pontG == null)
			print(gameObject.name +"não apresenta script ponteiro");

		//colocando valores iniciais
		tempo = tempoDeJogo;
		tempoAtivo = 10;//tempoAtivoMax - 1;
		horaAtual = (int)tempoDeJogo/60 -1;

		plaFpc = pla.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController >();	

		pontP.comeco(6, GetComponent<Relogio>());
		pontG.comeco(6*60, GetComponent<Relogio>());
	}

	public override void interacao(){
		if(pdDarCorda1 == false || pdDarCorda2 == false)
			return;
			
		pdDarCorda2 = false;


		//caso chegue no valor maximo
		if(!(tempoAtivo + forcaDarCorda> tempoAtivoMax)){
			audioWinding.clip = winding[(int) Random.Range(0f, winding.Length -1) ]; 
			audioWinding.Play();
		}else{
			//audioWindingMax.Play();
			StartCoroutine(cooldownDarCordaMax());
			tempoAtivo = tempoAtivoMax - forcaDarCorda;
		}

		//aplicando cooldowns
		StartCoroutine(cooldownDarCorda());
	}

	void Update () {

		//caso relogio ativo
		if(tempoAtivo > 0f){
			tempoAtivo -= Time.deltaTime; 
			tempo -= Time.deltaTime;
		}else{
			audioTick.enabled = false;
		}


		//termino do jogo
		if(tempo <= 0f){
			Debug.Log("acaba o jogo");
		}

		//mudar de hora
		if(horaAtual != (int)tempo/60){
			horaAtual = (int)tempo/60;
			audioBatidaHora.Play();
		}
	}




	IEnumerator cooldownDarCordaMax() 	{
		pdDarCorda1 = false;
	    yield return new WaitForSeconds(8f);
		pdDarCorda1 = true;
	}

	IEnumerator cooldownDarCorda(){
	    yield return new WaitForSeconds(1f);
		tempoAtivo+=forcaDarCorda;

		//relogio fazendo som de tick denovo
		if(tempoAtivo > 0)
			audioTick.enabled = true;
			
		pdDarCorda2 = true;
	}
}
