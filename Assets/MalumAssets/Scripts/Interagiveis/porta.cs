using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class porta : interagivel {
	public float anguloMax;
	public float velA;//velocidade de abertura
	public float velF;//velocidade de fechamento
	public bool horario = true;//sentido da rotacao
	public Transform gira;//objeto que gira
	Canvas cv;
	private GameObject lpMg;
	public GameObject minigamePrefab;

	public state estado = state.trancado;
	public enum state {aberto,fechado,trancado,abrindo,fechando,paAbrindo,paFechando};

	private float anguloInicial;
	private float anguloAnterior;
	private int sentido= 1;

	public AudioSource audioOpen;
	public AudioSource audioClose;
	protected override void comeco(){
		
		cv = FindObjectOfType<Canvas>();
		
		anguloInicial = gira.eulerAngles.y;

		//evitando bugs com 0
		if(anguloInicial == 0)anguloInicial = 1;

		//arrumando variaveis segundo do movimentando
		if(!horario)
			sentido = -1;

		velA *= sentido;
		velF *= sentido;
		anguloMax *= sentido;
		anguloInicial *= sentido;
	}


	//interacao do ambiente / inimico com porta
	public override void interacao2(){
		//maquina de estado
		if(estado == state.aberto)
			estado = state.fechando;
		else if(estado == state.fechado)
			estado = state.abrindo;
		//else if(estado == state.fechando)
		//	estado = state.paAbrindo;
		//else if(estado == state.abrindo)
		//	estado = state.paFechando;
		else if(estado == state.paAbrindo)
			estado = state.abrindo;
		else if(estado == state.paFechando)
			estado = state.fechando;
		else if(estado == state.trancado){
			if (lpMg == null || lpMg.active == false) {
				lpMg = Instantiate(minigamePrefab) as GameObject;
				lpMg.transform.SetParent(cv.transform);
				RectTransform rec = lpMg.transform.GetComponent<RectTransform>();
				rec.localPosition = new Vector3(0,0,0);
				rec.offsetMin = new Vector3(0,0,0);
				rec.offsetMax = new Vector3(0,0,0);
				lpMg.GetComponent<lockPickController>().setDoor(this);
			}
		}

		//estado = state.aberto;
		//gira.eulerAngles = new Vector3(0,anguloMax * sentido ,0);

	}

	// void Start() {
	// }

	void Update(){
		//Debug.Log(estado);

		//arrumando rotacao y para ser menor que 360 e maior que 0
		if(gira.eulerAngles.y < 0){
			gira.eulerAngles = new Vector3(0,gira.eulerAngles.y + 360,0);
		}else if (gira.eulerAngles.y > 360){
			gira.eulerAngles = new Vector3(0,gira.eulerAngles.y %360,0);
		}

		anguloAnterior = gira.eulerAngles.y;


		//rotaciona 
		if(estado == state.abrindo){
			gira.Rotate(Vector3.up * Time.deltaTime * velA);

			//caso termine de abrir
			if( sentido*gira.eulerAngles.y  > anguloMax  && sentido*anguloAnterior <   anguloMax ){
				audioOpen.Play();
				estado = state.aberto;
				gira.eulerAngles = new Vector3(0,anguloMax * sentido ,0);
			}
		}
		else if(estado == state.fechando){
			gira.Rotate(Vector3.down * Time.deltaTime * velF);

			//caso termine de fechar
			if( sentido*gira.eulerAngles.y  <  anguloInicial && sentido*anguloAnterior >  anguloInicial){
				audioClose.Play();
				estado = state.fechado;
				gira.eulerAngles = new Vector3(0,anguloInicial * sentido,0);
			
			}
			
		}
	}

}
