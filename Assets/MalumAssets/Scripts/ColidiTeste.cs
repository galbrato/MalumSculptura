using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColidiTeste : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        Debug.Log("OnTriggerEnter: eu " + name + " colidi com " + other.name);
    }

    private void OnCollisionEnter(Collision collision) {
        Debug.Log("OnCollisionEnter: eu " + name + " colidi com " + collision.collider.name);
    }

}
