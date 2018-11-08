using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class colisorPortaEstatua : MonoBehaviour{
    public SuperficieInteragivel superficie;
    private void OnTriggerEnter(Collider other) {
        

        if (other.CompareTag("Enemy")) {
            superficie.colisaoEntrarEstatua(other.gameObject.transform);
        }
    }
    private void OnTriggerExit(Collider other) {

        if (other.CompareTag("Enemy")) {
            superficie.colisaoSairEstatua();
        }
    }
}
