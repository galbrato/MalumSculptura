using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class porta : MonoBehaviour {
	public SuperficieInteragivel superficie1;
	public SuperficieInteragivel superficie2;
	public float variacaoAng;
	private float anguloMax;
	public float velA;//velocidade de abertura
	public float velF;//velocidade de fechamento

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

	[HideInInspector]
	public MeshRenderer selfRender;
	private void Start(){
		selfRender = GetComponent<MeshRenderer>();
		cv = FindObjectOfType<Canvas>();
		
		anguloInicial = gira.eulerAngles.y;

		if(anguloInicial < 1)anguloInicial = 1;
		//evitando bugs com 0
		anguloMax = (anguloMax+anguloInicial)%360;
		if(anguloMax < 1)anguloMax = 1;

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

	
	//funcao chamada por superficieInteragivel
	public void interacao3(int id){

		if(estado == state.fechado)
			sentido = id;

			velA = Mathf.Abs(velA)*sentido;
			velF = Mathf.Abs(velF)*sentido;
			if(sentido == 1)
				anguloMax = anguloInicial + variacaoAng;
			else{
				anguloMax = (anguloInicial + variacaoAng+180)%360;
			}
			if(anguloMax == 0)anguloMax = 1;
			if(anguloMax == 360)anguloMax = 359;
			anguloInicial = Mathf.Abs(anguloInicial);
	
		
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
		
		anguloAnterior = gira.eulerAngles.y;


		//rotaciona 
		if(estado == state.abrindo){
			gira.Rotate(Vector3.up * Time.deltaTime * velA);

			//caso termine de abrir
			if( sentido*gira.eulerAngles.y  > anguloMax * sentido  && sentido*anguloAnterior <   anguloMax * sentido ){
				atualizarEstado(state.aberto);
				gira.eulerAngles = new Vector3(0,anguloMax  ,0);
			}
		}
		else if(estado == state.fechando){
			gira.Rotate(Vector3.down * Time.deltaTime * velF);

			//caso termine de fechar
			if( sentido*gira.eulerAngles.y  <  anguloInicial *sentido && sentido*anguloAnterior >  anguloInicial* sentido){
				audioFechar.Play();
				atualizarEstado(state.fechado);
				gira.eulerAngles = new Vector3(0,anguloInicial ,0);
			
			}
			
		}
	}


	public void atualizarEstado(state valor){
		estado = valor;
		string textoSuperficie;
		if(estado == state.aberto){
            textoSuperficie = "Fechar";
		}else if(estado == state.fechado){
            textoSuperficie = "Abrir";
		}else if(estado == state.trancado){
			textoSuperficie = "Destrancar";
		}else{
			textoSuperficie = "";
		}
		superficie1.textInteragir = textoSuperficie;
		superficie2.textInteragir = textoSuperficie;
	}



	//cryingSpawn chama esta funcao quando cryingStatue teleporta para a sala
	public void interacao4(){
		atualizarEstado(state.trancado);
		gira.eulerAngles = new Vector3(gira.eulerAngles.x,anguloInicial,gira.eulerAngles.z);
	}
}
