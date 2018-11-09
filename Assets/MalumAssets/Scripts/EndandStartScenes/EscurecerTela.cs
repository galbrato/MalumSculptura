using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EscurecerTela : MonoBehaviour{
    private byte alfaTelaPreta =(byte)0;
    public Image telaPreta;
       public GameObject somRelogio;
    void Start (){
        StartCoroutine(EsccurecerTela());  
        DontDestroyOnLoad(somRelogio); 
    }

    //escurece imagem até ficar tudo preto
    IEnumerator EsccurecerTela(){
        //desabilitando inimigo
        EnemyBehaviour eni1 = FindObjectOfType<EnemyBehaviour>();
        if(eni1 != null)
            eni1.enabled = false;
        cryingStatue eni2 = FindObjectOfType<cryingStatue>();
        if(eni2 != null)
            eni2.enabled = false;

        telaPreta.color = new Color32((byte)0,(byte)0,(byte)0,alfaTelaPreta);
        while(alfaTelaPreta < 255){
            if(alfaTelaPreta > 255){
                alfaTelaPreta = 255;
            }


            alfaTelaPreta=(byte)(1+alfaTelaPreta);
            telaPreta.color = new Color32((byte)0,(byte)0,(byte)0,alfaTelaPreta);
            yield return new WaitForSeconds(0.01f);
        }


        //desativando inimigos
        if(eni1 != null)eni1.gameObject.SetActive(false);
        if(eni2 != null)eni2.gameObject.SetActive(false);
        //Invoke("acabarJogo",0.5f);
        SceneManager.LoadScene("EndScene");
    }

    void acabarJogo(){
        GameObject.Find("FirstPersonCharacter").SetActive(false);
    }
}
