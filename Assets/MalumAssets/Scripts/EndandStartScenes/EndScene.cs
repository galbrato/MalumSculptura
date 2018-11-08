using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class EndScene : MonoBehaviour{
    public Image telaPreta;
    public Text textoFinal;
    private byte alfaTelaPreta =(byte)0;
    private bool acabouJogo;
    
    void Start (){
        StartCoroutine(EscurecerTela());
    }
    void Update () {
        Debug.Log("a");
        if(acabouJogo){
            if(Input.GetMouseButton(1)){
                textoFinal.text = "ir cena inicial";
                //qual eh a cena menu??
                //SceneManager.LoadScene("cenaInicial");
            }
        }
    }
    //escurece imagem até ficar tudo preto
    IEnumerator EscurecerTela(){
        //desabilitando inimigo
        EnemyBehaviour eni1 = FindObjectOfType<EnemyBehaviour>();
        if(eni1 != null)
            eni1.enabled = false;
        cryingStatue eni2 = FindObjectOfType<cryingStatue>();
        if(eni2 != null)
            eni2.enabled = false;

        while(alfaTelaPreta < 255){
            if(alfaTelaPreta > 255){
                alfaTelaPreta = 255;
            }


            alfaTelaPreta=(byte)(1+alfaTelaPreta);
            telaPreta.color = new Color32((byte)0,(byte)0,(byte)0,alfaTelaPreta);
            yield return new WaitForSeconds(0.01f);
        }

        //printando testo
        textoFinal.gameObject.SetActive(true);

        //desativando inimigos
        if(eni1 != null)eni1.gameObject.SetActive(false);
        if(eni2 != null)eni2.gameObject.SetActive(false);
        Invoke("acabarJogo",0.5f);
    }

    void acabarJogo(){
        Debug.Log("alo");
        acabouJogo = true;
    }
}
