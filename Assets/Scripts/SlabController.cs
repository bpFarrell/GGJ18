﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlabController : MonoBehaviour {
    public static int width = 8;
    private static Material _mat;
    public static Material mat {
        get { return _mat ?? (_mat = Resources.Load("Unlit_Spawn")as Material); }
    }
    private static bool hasFinished;
    public static List<SlabController> slabs = new List<SlabController>();
    public static void SpawnSlab(Vector3 pos, Vector3 rot,float t) {
        if (hasFinished) {
            hasFinished = false;
            slabs.Clear();
        }
        GameObject parent = new GameObject("Slab: " + t);
        parent.transform.position = pos;
        parent.transform.eulerAngles = rot;
        slabs.Add(parent.AddComponent<SlabController>());
        parent.layer = 4;
        Vector2[] lanePos = new Vector2[4];
        for (int uv = 0; uv < 4; uv++) {
            lanePos[uv] = new Vector2(t, t);
        }
        for (int x = 0; x < width; x++) {
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Quad);
            go.transform.SetParent(parent.transform);
            go.transform.localPosition = new Vector3(x-((float)(width-1)*0.5f), 0, 0);
            go.transform.localEulerAngles = new Vector3(90, 0, 0);
            Destroy(go.GetComponent<MeshCollider>());
            go.GetComponent<MeshRenderer>().material = mat;
            Mesh mesh = go.GetComponent<MeshFilter>().mesh;
            float xPos = ((float)x) / (width - 1);
            Color[] clrs = new Color[4];
            for (int uv = 0; uv < 4; uv++) {
                lanePos[uv].x = xPos;
                Color clr = new Color(
                    Random.Range(-1f, 1f),
                    Random.Range(-1f, 1f),
                    Random.Range(-1f, 1f));
                clrs[uv] = clr;
            }
            mesh.colors = clrs;
            mesh.uv2 = lanePos;
            mesh.UploadMeshData(false);
            go.GetComponent<MeshFilter>().mesh = mesh;
        }
        BoxCollider bc = parent.AddComponent<BoxCollider>();

        bc.size = new Vector3(width,0.01f,1);
    }
    public static void FinalizeSlabs() {
        hasFinished = true;
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
