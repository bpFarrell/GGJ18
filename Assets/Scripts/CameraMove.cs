using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour {

    public Transform racer;
    public float speed = 7;
    Vector3 anchor {
        get {
            return racer.position + (racer.forward * -5) + (racer.up * 7);
        }
    }

	void Update () {
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(racer.position - transform.position), Time.deltaTime*speed);
        transform.position = Vector3.Lerp(transform.position, anchor, Time.deltaTime * speed);
	}
}
