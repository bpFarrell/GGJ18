using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpamQuads : MonoBehaviour {
    public Transform racer;
    public Material mat;
    int width = 8;
	// Use this for initialization
	void Start () {
        GameObject parent = new GameObject("Parent");
        for (int y = 0; y < 40; y++) {
            SlabController.SpawnSlab(new Vector3(0, 0, y), Quaternion.identity, y);
        }
    }
	
	// Update is called once per frame
	void Update () {
        //mat.SetFloat("_Location", racer.position.magnitude+5);
    }
}
