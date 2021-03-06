﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartScene : MonoBehaviour{
    public Text textoUi;
    public AudioSource audioIntro;
    public string texto;
    private string newTexto;

    private bool audioIsOver;
    void Start (){     
        StartCoroutine(EscreverTela());
        StartCoroutine(waitForAudio());
        audioIntro.Play();
    }
    void Update () {
        if(audioIsOver)
        {
            textoUi.enabled = false;
        } else {
            Vector3 player = Camera.main.transform.position;
            Vector3 boku = transform.position;
            if((boku - player).magnitude > 10) {
                textoUi.enabled = false;
            } else {
                textoUi.enabled = true;

            }
        }
    }

    //waits for audio clip to end and then kill the text object
    IEnumerator waitForAudio() {
        yield return new WaitForSeconds(audioIntro.clip.length + 1.0f);
        audioIsOver = true;
    }

    IEnumerator EscreverTela(){

        for(int i=0;i<texto.Length;i++){

            newTexto=string.Concat(newTexto, texto[i] );
            textoUi.text = newTexto;
            yield return new WaitForSeconds(0.05f);
        }
    }
}
