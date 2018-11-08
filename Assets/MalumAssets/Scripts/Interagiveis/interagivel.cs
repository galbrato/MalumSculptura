using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class interagivel : MonoBehaviour {
	[HideInInspector]
	public string textInteragir ;//texto caso player aponte para objeto
	public float distMin;//distancia minima para player interagir
	protected Player pla;//gameObject jogador
	protected Transform plaTrans;//gameObject do jogador
	protected Transform trans;//transform do obj

	void Start(){
		pla = FindObjectOfType<Player>();
		plaTrans = pla.transform;
		trans = this.transform;
		comeco();
	}

	//player interage com objeto(botao pressionado)
	public void interagir(){
		if(Vector3.Distance(plaTrans.position, trans.position) > distMin)
			return;
		interacao();
	}

	//player interage com objeto(um click)
	public void interagir2(){
		if(Vector3.Distance(plaTrans.position, trans.position) > distMin)
			return;

		interacao2();
	}
	protected virtual void comeco(){}
	public virtual void interacao(){}
	public virtual void interacao2(){}

	//mostrar texto caso player aponte para objeto
	public void apontado(Text txt){
		if(Vector3.Distance(plaTrans.position, trans.position) > distMin)
			return;
		txt.text = textInteragir;
	}
}
