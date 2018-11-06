using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine;

public class Stamina : MonoBehaviour{
    public float StaminaDuration = 4;
    public float ActualStamina;
    FirstPersonController mController;
    // Start is called before the first frame update
    void Start()
    {
        mController = GetComponent<FirstPersonController>();
        ActualStamina = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (mController.m_IsWalking && ActualStamina < StaminaDuration) {
            ActualStamina += Time.deltaTime;
        }
        if (!mController.m_IsWalking) {
            ActualStamina -= Time.deltaTime;
        }
        if(ActualStamina < 0) {
            mController.m_IsWalking = true;
        }
    }
}
