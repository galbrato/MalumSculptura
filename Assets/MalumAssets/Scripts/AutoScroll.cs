using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutoScroll : MonoBehaviour
{

    public float speed;
    float scroll;
    Scrollbar bar;
    

    // Start is called before the first frame update
    void Start()
    {
        scroll = -speed;
        bar = GetComponent<Scrollbar>();
    }

    // Update is called once per frame
    void Update()
    {
        if(bar.value <= 0.0f) scroll = speed;
        if(bar.value >= 1.0f) scroll = -speed;
    }

    private void FixedUpdate() {
        bar.value += scroll;
    }
}
