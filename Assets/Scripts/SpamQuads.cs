using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpamQuads : MonoBehaviour {
    public Material mat;
    int width = 8;
	// Use this for initialization
	void Start () {
        GameObject parent = new GameObject("Parent");
        for (int y = 0; y < 40; y++) {
            Vector2[] pos = new Vector2[4];
            for(int uv = 0; uv < 4; uv++) {
                pos[uv] = new Vector2(y, y);
            }
            for (int x = 0; x < width; x++) {
                GameObject go = GameObject.CreatePrimitive(PrimitiveType.Quad);
                go.transform.position = new Vector3(x, 0, y);
                go.transform.eulerAngles = new Vector3(90, 0, 0);
                go.GetComponent<MeshRenderer>().material = mat;
                Mesh mesh = go.GetComponent<MeshFilter>().mesh;
                float xPos = ((float)x) / (width-1);
                for (int uv = 0; uv < 4; uv++) {
                    pos[uv].x = xPos;
                }
                mesh.uv2 = pos;
                mesh.UploadMeshData(true);
                go.GetComponent<MeshFilter>().mesh = mesh;
                go.transform.SetParent(parent.transform);
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
	}
}
