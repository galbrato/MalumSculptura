using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class interagivel : MonoBehaviour {
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

	//inimigo/ambiente interage com objeto
	public virtual void interacao2(){}

}
