using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class EndScene : MonoBehaviour{
    public Text textoUi;

    public string texto = "Parabéns, você sobreviveu esta noite.  Convide seus amigos para este desafio.";
    private string newTexto;

    void Start (){
        StartCoroutine(EscreverTela());

    }
    void Update () {


        if(Input.GetMouseButton(1)){
            SceneManager.LoadScene("MenuInicial");
            //qual eh a cena menu??
            //SceneManager.LoadScene("cenaInicial");
        }
    }

    IEnumerator EscreverTela(){

        for(int i=0;i<texto.Length;i++){

            newTexto=string.Concat(newTexto, texto[i] );
            textoUi.text = newTexto;
            yield return new WaitForSeconds(0.085f);
        }
    }
}
