using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EsbranquecerTela : MonoBehaviour{
        public Image telaPreta;
       private byte alfaTelaPreta =(byte)255;


    void Start(){
        StartCoroutine(EmbranquecerTela());
    }



    //embranquece imagem até ficar tudo picar normal
    IEnumerator EmbranquecerTela(){
        telaPreta.color = new Color32((byte)0,(byte)0,(byte)0,alfaTelaPreta);
        while(alfaTelaPreta >= 1){
            //Debug.Log(alfaTelaPreta);
            alfaTelaPreta=(byte)(-1+alfaTelaPreta);
            telaPreta.color = new Color32((byte)0,(byte)0,(byte)0,alfaTelaPreta);
            yield return new WaitForSeconds(0.008f);
        }

    }
}
