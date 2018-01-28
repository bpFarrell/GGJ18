using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour {
    public float distBack = 7;
    public float distUp = 4;
    public Transform racer;
    public float speed = 7;
    Vector3 anchor {
        get {
            return racer.position + (racer.forward * -distBack) + (racer.up * distUp);
        }
    }

	void Update () {
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation((racer.position - transform.position),Vector3.up), Time.deltaTime*speed);
        transform.position = Vector3.Lerp(transform.position, anchor, Time.deltaTime * speed);
	}
}
