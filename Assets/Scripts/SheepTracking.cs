using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepTracking : MonoBehaviour {

    public Transform target;
    public float trackSpeed = 10;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = Vector3.Lerp(transform.position, target.position, Time.deltaTime * trackSpeed);
        transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, Time.deltaTime * trackSpeed);
	}
}
