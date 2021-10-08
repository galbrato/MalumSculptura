using System;
using UnityEngine;
using UnityEngine.UI;

public class HudFlashLight : MonoBehaviour {

    [SerializeField] private Image lightEffect;
    [SerializeField] private Text percentage;

    private Color colorCached;
    private const int MAX_BATTERY = 60;
    void Awake() {
        colorCached = lightEffect.color;
    }

    public void UpdateLight(float batteryCounter) {
        int percentage = (int) batteryCounter;
        UpdatePercentage(percentage);
        UpdateLightEffect(percentage);
    }

    private void UpdatePercentage(int batteryCounter) {
        int percentComplete = (int)Math.Round((double)(100 * batteryCounter) / MAX_BATTERY);
        percentage.text = percentComplete + "%";
    }
    private void UpdateLightEffect(int batteryCounter) {
        lightEffect.color = new Color(colorCached.r,colorCached.g,colorCached.b,batteryCounter/100f);
    }
}
