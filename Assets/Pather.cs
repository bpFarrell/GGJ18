using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pather : MonoBehaviour {
    public Material mat;
    public float smoothDist;
    public float hiddenDistance;
    private static float _distance;
    public static float distance{
        set {
            _distance = Mathf.Max(_distance, value);
        }
    }
    int currentIndex;
    private static int _highestIndex;
    public static int highestIndex {
        set {
            _highestIndex = Mathf.Max(_highestIndex, value+60);
        }
    }
    private void Update() {
        hiddenDistance = _distance;
        smoothDist = Mathf.Lerp(smoothDist, _distance, Time.deltaTime*10);
        mat.SetFloat("_Location", smoothDist*0.5f+20);
        while(currentIndex < _highestIndex) {
            SlabController.slabs[currentIndex].EnableRender();
            currentIndex++;
        }
    }
    private void Reset() {
        _distance = 0;
        _highestIndex = 0;
        smoothDist = 0;
        mat.SetFloat("_Location", 0);
    }
    private void OnEnable() {
        Reset();
        GoalLogic.OnLastFinish += Reset;
    }
    private void OnDisable() {
        GoalLogic.OnLastFinish -= Reset;
    }
}
