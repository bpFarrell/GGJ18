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
    private void Update() {
        hiddenDistance = _distance;
        smoothDist = Mathf.Lerp(smoothDist, _distance, Time.deltaTime*10);
        mat.SetFloat("_Location", smoothDist*0.5f+10);
    }
    private void Reset() {
        _distance = 0;
    }
    private void OnEnable() {
        GoalLogic.OnLastFinish += Reset;
    }
    private void OnDisable() {
        GoalLogic.OnLastFinish -= Reset;
    }
}
