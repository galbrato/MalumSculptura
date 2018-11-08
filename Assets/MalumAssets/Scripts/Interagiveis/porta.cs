using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class porta : MonoBehaviour {
	public SuperficieInteragivel superficie1;
	[HideInInspector]
	public bool trancadoPorEstatua = false;
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

	
	//funcao chamada por superficieInteragivel quando player interage
	public void interacao2(int id){

		if(estado == state.fechado)
			sentido = id;

		velA = Mathf.Abs(velA)*sentido;
		velF = Mathf.Abs(velF)*sentido;
		if(sentido == 1)
			anguloMax = anguloInicial + variacaoAng;
		else{
			anguloMax = anguloInicial - variacaoAng;
			if(anguloMax < 0)
				anguloMax = 360 + anguloMax;
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
		//Debug.Log(anguloMax+" "+anguloInicial+" "+gira.eulerAngles.y);
	

		if(gira.eulerAngles.y> 360)
			gira.position = new Vector3(gira.eulerAngles.x,(gira.eulerAngles.y)%360,gira.eulerAngles.z);
		else if(gira.eulerAngles.y < 0){
			float novoAngulo = gira.eulerAngles.y;
			while(novoAngulo < 0){
				novoAngulo+=360;
			}
			gira.position = new Vector3(gira.eulerAngles.x,novoAngulo,gira.eulerAngles.z);
		}

		//rotaciona 
		if(estado == state.abrindo){
			gira.Rotate(Vector3.up * Time.deltaTime * velA);

			//caso termine de abrir
			if( Mathf.Abs(gira.eulerAngles.y - anguloMax) <  5){
				atualizarEstado(state.aberto);
			}
		}
		else if(estado == state.fechando){
			gira.Rotate(Vector3.down * Time.deltaTime * velF);

			//caso termine de fechar
			if( Mathf.Abs(gira.eulerAngles.y - anguloInicial) <  5){
				audioFechar.Play();
				if(trancadoPorEstatua){
					trancadoPorEstatua = false;
					atualizarEstado(state.trancado);
				}
				atualizarEstado(state.fechado);
			
			}
			
		}
	}


	public void atualizarEstado(state valor){
		estado = valor;
		string textoSuperficie;
		if(trancadoPorEstatua){
			textoSuperficie = "";
		}
		else if(estado == state.aberto){
            textoSuperficie = "Fechar";
		}else if(estado == state.fechado){
            textoSuperficie = "Abrir";
		}else if(estado == state.trancado){
			textoSuperficie = "Destrancar";
		}else{
			textoSuperficie = "";
		}
		superficie1.textInteragir = textoSuperficie;
	}



	//cryingSpawn chama esta funcao quando cryingStatue teleporta para a sala
	public void interacao4(){
		atualizarEstado(state.trancado);
		gira.eulerAngles = new Vector3(gira.eulerAngles.x,anguloInicial,gira.eulerAngles.z);
	}

	//funcao chamada por superficieInteragivel quando estatua interage(abrindo porta)
	public void EstatuaAbre(int id){

		sentido = id;
		velA = Mathf.Abs(velA)*sentido;
		velF = Mathf.Abs(velF)*sentido;
		if(sentido == 1)
			anguloMax = anguloInicial + variacaoAng;
		else{
			anguloMax = anguloInicial - variacaoAng;
			if(anguloMax < 0)
				anguloMax = 360 + anguloMax;
		}
		if(anguloMax == 0)anguloMax = 1;
		if(anguloMax == 360)anguloMax = 359;
		anguloInicial = Mathf.Abs(anguloInicial);
	
		if(estado==state.fechado || estado==state.fechado)
			Abrir();

	}


}
