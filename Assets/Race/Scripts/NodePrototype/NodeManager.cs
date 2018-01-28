using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeManager : MonoBehaviour {

    public int amount;
    public bool create;
    public class Node {
        public Transform nodeTrans;
        public enum State {
            nuetral,
            red,
            blue
        }
        State state;
        public Vector3 position;
        public Node(Vector3 pos) {
            position = pos;
            nodeTrans = GameObject.CreatePrimitive(PrimitiveType.Sphere).transform;
            NodeObj nodeObj = nodeTrans.gameObject.AddComponent<NodeObj>();
            nodeObj.node = this;
            nodeTrans.position = pos;
        }
        public void Remove() {
            Destroy(nodeTrans.gameObject);
        }
    }
    public List<Node> nodes = new List<Node>();
    Plane plane;

    public float energy;
    int costForPawn = 2;

    public List<GameObject> pawns = new List<GameObject>();
	// Use this for initialization
	void Start () {
        plane = new Plane(Vector3.up, Vector3.zero);
	}
	
	// Update is called once per frame
	void Update () {
        if (energy < 10) { energy += Time.deltaTime; }
        else energy = 10;

        if (create) {
            create = false;
            for (int i = 0; i < nodes.Count; i++) {
                nodes[i].Remove();
            }
            nodes.Clear();
            for (int i = 0; i < amount; i++) {
                float x = Random.Range(0, amount);
                float z = Random.Range(0, amount);
                Vector3 pos = new Vector3(x, 0, z);
                Node n = new Node(pos);
                nodes.Add(n);
            }
        }
        
        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo))
            {
                if (hitInfo.transform.GetComponent<NodeObj>() != null)
                {
                    for (int i = 0; i < pawns.Count; i++)
                    {
                        if (pawns[i].GetComponent<PawnBehave>().state == PawnBehave.State.idle)
                        {
                            pawns[i].GetComponent<PawnBehave>().SetDestination(hitInfo.transform.GetComponent<NodeObj>().node);// = hitInfo.transform.GetComponent<NodeObj>().node.position;
                        }
                    }
                    return;
                }
            }

            float dist;
            if (plane.Raycast(ray, out dist))
            {
                Debug.DrawRay(ray.origin, ray.direction*dist, Color.cyan, 3);
                Vector3 spawnPoint = ray.origin + (ray.direction * dist);
                if (energy > costForPawn)
                {
                    GameObject pawn = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    pawn.transform.position = spawnPoint;
                    pawn.AddComponent<PawnBehave>();
                    pawns.Add(pawn);
                    energy -= costForPawn;
                }
            }
        }
	}
}
