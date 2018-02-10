using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSwapper : MonoBehaviour {
    public UnityEngine.PostProcessing.PostProcessingProfile post;
    public float swapChance;
    public float[] hueValues;
    public static ColorSwapper instance;
    private static bool hasFirst;
    private void OnEnable() {
        instance = this;
        Reset();
    }
    private void OnDisable() {
        Reset();
    }
    public void TrySwap() {
        if (!hasFirst) {
            hasFirst = true;
            return;
        }
        float rand = Random.value;
        
        if (rand < swapChance) {
            int pallet = Random.Range(0, hueValues.Length);
            SetHue(hueValues[pallet]);
        }
    }
    public void Reset() {
        SetHue(0);
    }
    private void SetHue(float hueShift) {
        var temp = post.colorGrading.settings;
        temp.basic.hueShift = hueShift;
        post.colorGrading.settings = temp;
    }
}
