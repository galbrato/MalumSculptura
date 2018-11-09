using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyClock : MonoBehaviour
{

    void Start(){
        GameObject rel =FindObjectOfType<Relogio>().gameObject;
        if(rel != null)
            Destroy(rel.gameObject);
    }

}
