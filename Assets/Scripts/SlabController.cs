using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SplineLogic;
public class SlabController : MonoBehaviour {
    public static int width = 10;
    public float t;
    public float internalT;
    private Mesh[] quads;
    private GameObject[] gos;
    private static Material _mat;
    public static Material mat {
        get { return _mat ?? (_mat = Resources.Load("Unlit_Spawn")as Material); }
    }
    private static GameObject _customQuad;
    public static GameObject customQuad {
        get { return _customQuad ?? (_customQuad = Resources.Load("Quad") as GameObject); }
    } 
    private static bool hasFinished;
    public static List<SlabController> slabs = new List<SlabController>();
    public void Start() {
        gameObject.isStatic = true;
        for(int x = 0; x < gos.Length; x++) {
            gos[x].isStatic = true;
        }
    }
    public static void SpawnSlab(Vector3 pos, Quaternion rot,float t) {
        if (hasFinished) {
            hasFinished = false;
            slabs.Clear();
        }
        GameObject parent = new GameObject("Slab: " + t);
        parent.transform.position = pos;
        SlabController slab = parent.AddComponent<SlabController>();
        if (slabs.Count == 0) {
            slab.internalT = 0;
        } else {
            SlabController prevSlab = slabs[slabs.Count - 1];
            slab.internalT = Vector3.Distance(prevSlab.transform.position, pos) + prevSlab.internalT;
        }
        slabs.Add(slab);
        slab.t = t;
        parent.transform.rotation = rot;
        parent.layer = 4;
        slab.quads = new Mesh[width];
        slab.gos = new GameObject[width];
        Vector2[] lanePos = new Vector2[4];
        for (int uv = 0; uv < 4; uv++) {
            lanePos[uv] = new Vector2(slab.internalT, slab.internalT);
        }
        for (int x = 0; x < width; x++) {
            GameObject go = Instantiate(customQuad);//GameObject.CreatePrimitive(PrimitiveType.Quad);
            go.transform.SetParent(parent.transform);
            go.transform.localPosition = new Vector3(x-((float)(width-1)*0.5f), 0, 0);
            go.transform.localEulerAngles = new Vector3(0, 0, 0);
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
            slab.quads[x] = mesh;
            go.GetComponent<MeshFilter>().mesh = mesh;
            slab.gos[x] = go;
        }
        BoxCollider bc = parent.AddComponent<BoxCollider>();

        bc.size = new Vector3(width,0.01f,1);
    }
    public static void FinalizeSlabs() {
        hasFinished = true;
        for(int s = 0; s < slabs.Count-1; s++) {

            float tempT = (slabs[s].t + slabs[s + 1].t) * 0.5f;
            Quaternion midRot = Spline.instance.EvaluateRotation(tempT);
            Vector3 midPos = Spline.instance.EvaluatePosition(tempT);
            Vector3 midLeft = midPos + midRot * -Vector3.right * ((float)width) * 0.5f;
            Vector3 midRight = midPos + midRot * Vector3.right * ((float)width) * 0.5f;
            float valueStep = 1f / ((float)width);
            for(int q = 0; q < slabs[s].quads.Length; q++) {

                Vector3 currentPoint = Vector3.Lerp(slabs[s].fl, slabs[s].fr, valueStep * q);
                Vector3 deltaPos = Vector3.Lerp(midLeft, midRight, valueStep * q)-currentPoint;
                List<Vector3> tempVerts=new List<Vector3>();
                slabs[s].quads[q].GetVertices(tempVerts);
                tempVerts[3] += slabs[s].gos[q].transform.InverseTransformVector(deltaPos);
                currentPoint = Vector3.Lerp(slabs[s].fl, slabs[s].fr, valueStep * (q+1));
                deltaPos = Vector3.Lerp(midLeft, midRight, valueStep * (q+1)) - currentPoint;
                tempVerts[0] += slabs[s].gos[q].transform.InverseTransformVector(deltaPos);
                slabs[s].quads[q].SetVertices(tempVerts);
                slabs[s].quads[q].UploadMeshData(false);

                

                currentPoint = Vector3.Lerp(slabs[s+1].bl, slabs[s+1].br, valueStep * q);
                deltaPos = Vector3.Lerp(midLeft, midRight, valueStep * q) - currentPoint;
                tempVerts = new List<Vector3>();
                slabs[s+1].quads[q].GetVertices(tempVerts);
                tempVerts[2] += slabs[s+1].gos[q].transform.InverseTransformVector(deltaPos);
                currentPoint = Vector3.Lerp(slabs[s+1].bl, slabs[s+1].br, valueStep * (q + 1));
                deltaPos = Vector3.Lerp(midLeft, midRight, valueStep * (q + 1)) - currentPoint;
                tempVerts[1] += slabs[s+1].gos[q].transform.InverseTransformVector(deltaPos);
                slabs[s+1].quads[q].SetVertices(tempVerts);
                slabs[s+1].quads[q].UploadMeshData(false);
            }
        }
    }
    #region Edge Checks
    public Vector3 fl {
        get {
            return transform.position +
                transform.forward * 0.5f +
                transform.right * -((float)width) * 0.5f;
        }
    }
    public Vector3 fr {
        get {
            return transform.position +
                transform.forward * 0.5f +
                transform.right * ((float)width) * 0.5f;
        }
    }
    public Vector3 bl {
        get {
            return transform.position +
                transform.forward * -0.5f +
                transform.right * -((float)width) * 0.5f;
        }
    }
    public Vector3 br {
        get {
            return transform.position +
                transform.forward * -0.5f +
                transform.right * ((float)width) * 0.5f;
        }
    }
    #endregion
}
