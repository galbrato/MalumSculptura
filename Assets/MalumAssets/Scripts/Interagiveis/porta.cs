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
	public enum state {aberto,fechado,trancado,abrindo,fechando};

	private float anguloInicial;
	private float anguloAnterior;
	private int sentido= 1;

	public AudioSource audioAbrindo;//ruído abrindo a porta
	public AudioSource audioFechando;//ruído fechando a porta
	public AudioSource audioFechar;//pancada da porta fechadondo
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
		atualizarEstado(estado);

	}
    public void Abrir() {
        atualizarEstado(state.abrindo);
        audioAbrindo.Play();
    }
    public void Fechar() {
        atualizarEstado(state.fechando);
        audioFechando.Play();
    }

	//interacao do ambiente / inimico com porta
	public override void interacao2(){
		//maquina de estado
		if(estado == state.aberto){
            Fechar();
		}
		else if(estado == state.fechado){
            Abrir();

        } else if(estado == state.trancado){
			if (lpMg == null || lpMg.activeSelf == false) {
				lpMg = Instantiate(minigamePrefab) as GameObject;
				lpMg.transform.SetParent(cv.transform);
				RectTransform rec = lpMg.transform.GetComponent<RectTransform>();
				rec.localPosition = new Vector3(0,0,0);
				rec.offsetMin = new Vector3(0,0,0);
				rec.offsetMax = new Vector3(0,0,0);
				lpMg.GetComponent<lockPickController>().setDoor(this);
			}
		}
	}



	void Update(){

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
				atualizarEstado(state.aberto);
				gira.eulerAngles = new Vector3(0,anguloMax * sentido ,0);
			}
		}
		else if(estado == state.fechando){
			gira.Rotate(Vector3.down * Time.deltaTime * velF);

			//caso termine de fechar
			if( sentido*gira.eulerAngles.y  <  anguloInicial && sentido*anguloAnterior >  anguloInicial){
				audioFechar.Play();
				atualizarEstado(state.fechado);
				gira.eulerAngles = new Vector3(0,anguloInicial * sentido,0);
			
			}
			
		}
	}


	public void atualizarEstado(state valor){
		estado = valor;
		if(estado == state.aberto){
            textInteragir = "Fechar";
		}else if(estado == state.fechado){
            textInteragir = "Abrir";
		}else if(estado == state.trancado){
			textInteragir = "Destrancar";
		}else{
			textInteragir = "";
		}
	}



}
