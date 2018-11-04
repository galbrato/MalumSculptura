using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.Gaming;

public class Lanterna : MonoBehaviour {

    public bool seekMouse = true;
    private Player jogador;
    private Light myLight;
    public int nRays = 8;
    public int nCircles = 3;
    public int addicionalAngle = 5;
    private EnemyBehaviour saveEnemy;
    public bool debug = false;

    public bool isTurnedOn = true;

    //variaveis da bateria
    public float MaxBaterry = 100f;
    public float BatteryDrainPerSecond = 1f;
    public float BatteryRechargePerSecond = 3f;
    public float ActualBattery = 100f;

    public float HoldDelay = 0.1f;
    private float HoldDelayCaunter;

    public float LightDecressPoint = 5;

    public static Lanterna instance = null;

    private float OriginalRange;

    private void Awake() {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    void Start() {
        HoldDelayCaunter = HoldDelay;
        myLight = gameObject.GetComponent<Light>();
        OriginalRange = myLight.range;
    	//jogador = FindObjectOfType<Player>().GetComponent<Player>();
    }


    void Update() {
        if (Input.GetButtonDown("Fire1")) {
            LightOff();
        }

        if (Input.GetButtonUp("Fire1") ) {
            LightOn();
        }

        if (!isTurnedOn) {
            Recharge();
        }

        if(isTurnedOn && ActualBattery > 0f) {
            ActualBattery -= BatteryDrainPerSecond * Time.deltaTime;
        }

        if(ActualBattery < 0) {
            LightOff();
        }

        if((ActualBattery/MaxBaterry)*100 < LightDecressPoint) {
            myLight.range = OriginalRange * (ActualBattery / (MaxBaterry * (LightDecressPoint/100)));
        } else {
            myLight.range = OriginalRange;
        }

        if (isTurnedOn == false) return;
        Vector3 olhando;
        Ray ray;

        if ((!TobiiAPI.GetDisplayInfo().IsValid) || (!TobiiAPI.GetUserPresence().IsUserPresent())) {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        } else {
            olhando = new Vector3(TobiiAPI.GetGazePoint().Screen.x, TobiiAPI.GetGazePoint().Screen.y, 0f);
            ray = Camera.main.ScreenPointToRay(olhando);
            //se tiver eyetracker, mas quiser pausar porque perdeu os olhos, fazer akie
            /*if (TobiiAPI.GetDisplayInfo ().IsValid) {
				Debug.Log ("Seus olhos não podem ser detectados");
			}*/
        }

        if (seekMouse) gameObject.transform.rotation = Quaternion.LookRotation(ray.direction);
        else gameObject.transform.rotation = Camera.main.transform.rotation;

        //jogador.dirVisao = ray.direction;
        //Debug.Log(ray.direction);
        Vector3 dir = (seekMouse ? ray.direction : Camera.main.transform.forward);
        RayDetection(dir, myLight.range);
        for (int c = 1; c < nCircles; c++) {

            Vector3 dir2 = Quaternion.AngleAxis(((myLight.spotAngle + addicionalAngle) / (2 * nCircles)) * c, transform.up) * dir;
            int nRaysAux = 1 + ((nRays - 1) / (nCircles - 1)) * c;
            for (int i = 0; i < nRaysAux; i++) {
                //rotacionar
                Vector3 dir3 = Quaternion.AngleAxis(i * (360f / nRaysAux), transform.forward) * dir2;
                RayDetection(dir3, myLight.range);
                //mandar o inimigo ficar paradão
            }
        }
    }

    string RayDetection(Vector3 direction, float maxDist) {

        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, maxDist)) {
            if (debug) Debug.DrawRay(transform.position, direction * hit.distance, Color.green);
            if (hit.collider.gameObject.tag == "Enemy") {
                if (!saveEnemy)
                    saveEnemy = hit.collider.gameObject.GetComponent<EnemyBehaviour>();
                saveEnemy.Stop();
            }
        }
        return "FODEU DETECTOU NADA";
    }


    public void LightOff() {
        myLight.enabled = false;
        isTurnedOn = false;
    }

    public void TrueLightOn() {
        myLight.enabled = true;
    }

    public void LightOn() {
        isTurnedOn = true;
        Invoke("TrueLightOn", 0.1f);
    }

    private void Recharge() {
        if (ActualBattery < MaxBaterry) {
            ActualBattery += BatteryRechargePerSecond * Time.deltaTime;
        }
    }

}
