using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhaleTime : MonoBehaviour {
    Vector3 startPos;
    public float speed = 10;
    float delay = 30;
    float startTime;
	// Use this for initialization
	void Start () {
        startPos = transform.position;
        startTime = Time.time + delay;
	}
	
	// Update is called once per frame
	void Update () {
        if(startTime<Time.time)
        transform.position += transform.forward * Time.deltaTime * speed;
        if(transform.position.z < -720) {

            Reset();
        }
	}
    private void Reset() {
        transform.position = startPos;
        startTime = Time.time + delay;
    }
}
