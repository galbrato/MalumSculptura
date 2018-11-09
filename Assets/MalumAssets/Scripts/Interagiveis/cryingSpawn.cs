using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cryingSpawn : MonoBehaviour {
    public porta[] portas;

    public MeshRenderer[] meshPortas;
    private MeshRenderer selff;

    void Start(){
        selff = GetComponent<MeshRenderer>();
    }
    public bool obsertado(){
        if(selff.isVisible)
            return true;
        

        for(int i = 0;i<meshPortas.Length;i++){
            if(meshPortas[i].isVisible){
                return true;
            }
        }
         for(int i = 0;i<portas.Length;i++){
            if(portas[i].selfRender.isVisible){
                return true;
            }

        }

        return false;
    }


    public void teleporte(){
        for(int i = 0;i<portas.Length;i++){
            portas[i].interacao4();
           // Debug.Log(name);

        }
    }
}
