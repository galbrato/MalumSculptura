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
    private cryingStatue saveChorona;
    public bool debug = false;

    public bool isTurnedOn = true;

    //variaveis da bateria
    public float BaterryDuration = 60f;
    public float BaterryRecoveryDuration = 30f;
    public float BaterryCounter;

    public float LightDecressPoint = 5;

    public static Lanterna instance = null;

    private AudioSource LigarSound;
    private AudioSource DesligarSound;

    private float OriginalRange;
    private void Awake() {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        AudioSource[] ads = GetComponents<AudioSource>();
        LigarSound = ads[0];
        DesligarSound = ads[1];
    }


    void Start() {
        BaterryCounter = BaterryDuration;
        myLight = gameObject.GetComponent<Light>();
        OriginalRange = myLight.range;
    	//jogador = FindObjectOfType<Player>().GetComponent<Player>();
    }


    void Update() {
        if (Input.GetButtonDown("Fire1")) {
            LightOff();
        }

        if (Input.GetButtonUp("Fire1")) {
            LightOn();
        }

        if (!isTurnedOn) {
            Recharge();
        }

        if(isTurnedOn && BaterryCounter > 0f) {
            BaterryCounter -= Time.deltaTime;
        }

        if(BaterryCounter < 0) {
            LightOff();
        }

        if(BaterryCounter < LightDecressPoint) {
            myLight.range = OriginalRange * (BaterryCounter / LightDecressPoint);
        } else {
            myLight.range = OriginalRange;
        }

        if (isTurnedOn == false) return;
        Look();
        Detect();
    }

    void Look() {
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
    }


    void Detect() { 
        Vector3 dir = Camera.main.transform.forward;
        List<Collider> cols = ConeShowerDetection(transform, dir, myLight.range, myLight.spotAngle + addicionalAngle, nCircles, nRays);
        foreach (Collider col in cols) {
            if (col.CompareTag("Enemy")) {
                if (saveEnemy == null || saveEnemy.name != col.name) {
                    saveEnemy = col.gameObject.GetComponent<EnemyBehaviour>();
                }
                saveEnemy.Stop();
            } else if (col.CompareTag("Atormentada")) {
                if (saveChorona == null || saveChorona.name != col.name) {
                    saveChorona = col.gameObject.GetComponent<cryingStatue>();
                }
                //saveChorona.isVisible = true;
            }
        }
    }


    Collider RayDetection(Vector3 origin ,Vector3 direction, float maxDist) {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, maxDist)) {
            if (debug) Debug.DrawRay(origin, direction * hit.distance, Color.green);
            return hit.collider;
        }
        return null;
    }


    public List<Collider> ConeShowerDetection(Transform origin, Vector3 direction, float range, float angle, int nCircles, int nRays) {
        List<Collider> cols = new List<Collider>();
        for (int c = 1; c < nCircles; c++) {

            Vector3 dir2 = Quaternion.AngleAxis((( angle ) / (2 * nCircles)) * c, origin.up) * direction;
            int nRaysAux = 1 + ((nRays - 1) / (nCircles - 1)) * c;
            for (int i = 0; i < nRaysAux; i++) {
                //rotacionar
                Vector3 dir3 = Quaternion.AngleAxis(i * (360f / nRaysAux), origin.forward) * dir2;
                Collider col = RayDetection(origin.position, dir3, range);
                if (col != null && !cols.Contains(col)) {
                    cols.Add(col);
                }
            }
        }
        return cols;
    }


    public void LightOff() {
        myLight.enabled = false;
        isTurnedOn = false;
    }

    public void TrueLightOn() {
        if(isTurnedOn)myLight.enabled = true;
    }

    public void LightOn() {
        isTurnedOn = true;
        Invoke("TrueLightOn", 0.2f);
    }

    private void Recharge() {
        if (BaterryCounter < BaterryDuration) {
            BaterryCounter += Time.deltaTime*(BaterryDuration/BaterryRecoveryDuration);
        }
    }

}
