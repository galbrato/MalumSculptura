﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperficieInteragivel : interagivel{

    public porta Porta;
    public GameObject pai;
    private float anguloInicialPai;

    protected override void comeco(){
        trans = GetComponent<Transform>();
        anguloInicialPai = pai.GetComponent<Transform>().eulerAngles.y;
    }
    public override void interacao2(){
        if(Vector3.Distance(plaTrans.position, trans.position) > distMin)
			return;
        if(anguloInicialPai == 0 ){
            if(plaTrans.position.z > trans.position.z)
                Porta.interacao2(-1);
            else
                Porta.interacao2(1);
        }else if(anguloInicialPai == 90 ){
            if(plaTrans.position.x > trans.position.x)
                Porta.interacao2(-1);
            else
                Porta.interacao2(1);
        }else if( anguloInicialPai == 180){
            if(plaTrans.position.z > trans.position.z)
                Porta.interacao2(1);
            else
                Porta.interacao2(-1);
        }else if( anguloInicialPai == 270){
            if(plaTrans.position.x > trans.position.x)
                Porta.interacao2(1);
            else
                Porta.interacao2(-1);
        }
    }

    //caso estátua abra a porta
    public void colisaoEntrarEstatua(Transform other){
            Porta.estatuaColidindo = true;
        //caso porta nao aberta
        if(Porta.estado == porta.state.aberto)
            return;
        if(anguloInicialPai == 0 ){
            if(other.position.z > trans.position.z)
                Porta.EstatuaAbre(-1);
            else
                Porta.EstatuaAbre(1);
        }else if(anguloInicialPai == 90 ){
            if(other.position.x > trans.position.x)
                Porta.EstatuaAbre(-1);
            else
                Porta.EstatuaAbre(1);
        }else if( anguloInicialPai == 180){
            if(other.position.z > trans.position.z)
                Porta.EstatuaAbre(1);
            else
                Porta.EstatuaAbre(-1);
        }else if( anguloInicialPai == 270){
            if(other.position.x > trans.position.x)
                Porta.EstatuaAbre(1);
            else
                Porta.EstatuaAbre(-1);
        }
    }

    //estatua fechando porta
    public void colisaoSairEstatua(){
        Porta.estatuaColidindo = false;
        if(Porta.estado == porta.state.fechado)
            return;
        Porta.EstatuaFecha();
    }
}
