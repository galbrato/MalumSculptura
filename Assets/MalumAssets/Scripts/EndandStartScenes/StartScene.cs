using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartScene : MonoBehaviour{
    public Text textoUi;
    public AudioSource audioIntro;
    public string texto = "Bem vindo a mansão, ficarei a noite fora, tente sobreviver nela. Ah, além disso, recomendo dar corda no relógio.";
    private string newTexto;
    void Start (){     

        StartCoroutine(EscreverTela());
        audioIntro.Play();
    }
    void Update () {
        
        //comecar jogo
        if(Input.GetMouseButton(1)){
            SceneManager.LoadScene("sasaki");
        }
    }



    IEnumerator EscreverTela(){

        for(int i=0;i<texto.Length;i++){

            newTexto=string.Concat(newTexto, texto[i] );
            textoUi.text = newTexto;
            yield return new WaitForSeconds(0.05f);
        }
    }
}
