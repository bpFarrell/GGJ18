/*
////////////////////////////////////////////////////////////////////
 Jason Dean Crossley

 Notes:
\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
*/

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SplineNode : MonoBehaviour {
    public int id;
    public float size;
    private Transform _enterHandle;
    public Vector3 enter
    {
        get {
            if (_enterHandle == null) return Vector3.zero;
            else return _enterHandle.position;
        }
    }

    private Transform _exitHandle;

    public Vector3 exit
    {
        get {
            if (_exitHandle == null) return Vector3.zero;
            else return _exitHandle.position;
        }
    }

    public Vector3 node
    {
        get { return transform.position; }
    }
    public void Init()
    {
        if (_enterHandle == null)
        {
            _enterHandle = new GameObject("enter handle").transform;
            _enterHandle.parent = transform;
            _enterHandle.position = transform.position + (-1f * transform.forward);
            SplineHandle enterhand = _enterHandle.gameObject.AddComponent<SplineHandle>();
            enterhand.size = size / 3;
            enterhand.color = Color.yellow;
        }
        if (_exitHandle == null)
        {
            _exitHandle = new GameObject("exit handle").transform;
            _exitHandle.parent = transform;
            _exitHandle.position = transform.position + (1f * transform.forward);
            SplineHandle exithand = _exitHandle.gameObject.AddComponent<SplineHandle>();
            exithand.size = size / 3;
            exithand.color = Color.red;
        }
    }
    private void OnDrawGizmos()
    {
        DrawHandle(size);
    }
    public void DrawHandle(float size)
    {
        Color prevColor = Gizmos.color;
        Gizmos.DrawLine(transform.position, enter);
        Gizmos.DrawLine(transform.position, exit);
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, size);

        Gizmos.color = prevColor;
    }
}