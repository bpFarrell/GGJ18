using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepTracking : MonoBehaviour {

    public Material mat;
    public Transform target;
    public float trackSpeed = 10;
    float locationT;
    public SlabController latestSlab;


    // Use this for initialization
    void Start () {
        mat = Resources.Load("Unlit_Spawn") as Material;
	}
	
	// Update is called once per frame
	void Update () {
        Move();
        GetNodeInfo();
        LerpT();
	}

    void Move() {
        transform.position = Vector3.Lerp(transform.position, target.position, Time.deltaTime * trackSpeed);
        transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, Time.deltaTime * trackSpeed);
    }

    void GetNodeInfo() {
        Ray ray = new Ray(transform.position+transform.up, -transform.up);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo)) {
            if (hitInfo.transform.GetComponent<SlabController>() != null) {
                latestSlab = hitInfo.transform.GetComponent<SlabController>();
                locationT = latestSlab.internalT;
            }
        }
    }

    void LerpT() {
        float currentT = mat.GetFloat("_Location");
        if (locationT > currentT)
        {
            currentT = Mathf.Lerp(currentT, locationT, Time.deltaTime * 10);
            mat.SetFloat("_Location", currentT + 8);
        }
    }
}
