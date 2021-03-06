﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Relogio : interagivel {

	public float tempoDeJogo = 30f;//tempo da fase
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
	
	public AudioClip[] windingClip;
	private AudioSource audioWinding;
	private AudioSource audioTick;
	public AudioClip tickClip;

	[HideInInspector]
	public AudioSource audioBatidaHora;
	public AudioClip batidaClip;
	//int horaAtual ;
	bool aconteceuBatidaFinal = false;

	private cryingStatue estatuaChorando;
	protected override void comeco() {
      
		if(pontP == null){
			print(gameObject.name +"não apresenta script ponteiro");
		}else if(pontG == null)
			print(gameObject.name +"não apresenta script ponteiro");

		//colocando valores iniciais
		tempo = 0;
		tempoAtivo = tempoAtivoMax/2;
		//horaAtual = (int)tempoDeJogo/60 -1;

		plaFpc = pla.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController >();	

		pontP.comeco(GetComponent<Relogio>(),tempoDeJogo/6);
		pontG.comeco(GetComponent<Relogio>(),tempoDeJogo/(6*12));

		//colocando os audios
		audioWinding = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
		audioTick = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
		audioTick.clip = tickClip;
		audioBatidaHora = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
		audioBatidaHora.clip = batidaClip;

		textInteragir = "Dar Corda";
		audioTick.Play();

		estatuaChorando = FindObjectOfType<cryingStatue>();
	}

	public override void interacao(){
 

		if(Vector3.Distance(plaTrans.position, trans.position) > distMin)
			return;

		if(pdDarCorda1 == false || pdDarCorda2 == false)
			return;
			
		pdDarCorda2 = false;

		//caso primeira vez que da corda no relogio
		if(!estatuaChorando.ativadaUmaVez)
			estatuaChorando.piscaLanterna();

		//nao caso chegue no valor maximo
		if(!(tempoAtivo + forcaDarCorda> tempoAtivoMax)){
			audioWinding.clip = windingClip[(int) Random.Range(0f, windingClip.Length -1) ]; 
			audioWinding.Play();
		}else{//caso chegue no valor maximo
			//audioWindingMax.Play();
			textInteragir = "";
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
			tempo += Time.deltaTime;
		}else{
			audioTick.enabled = false;
		}


		//termino do jogo
		if(tempo >= tempoDeJogo && !aconteceuBatidaFinal){
			//audioBatidaHora.Play();
			FindObjectOfType<EscurecerTela>().enabled = true;
			audioWinding.Stop();
			audioTick.Stop();
			aconteceuBatidaFinal = true;
		}

		//mudar de hora
		//if(horaAtual != (int)tempo/60){
		//	horaAtual = (int)tempo/60;
		//	audioBatidaHora.Play();
		//}
	}




	IEnumerator cooldownDarCordaMax() 	{
		pdDarCorda1 = false;
	    yield return new WaitForSeconds(8f);
		pdDarCorda1 = true;
		textInteragir = "Dar Corda";
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
