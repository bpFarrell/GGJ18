/*
////////////////////////////////////////////////////////////////////
 Jason Dean Crossley

 Notes:
\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplineHandle : MonoBehaviour {
    public float size;
    public Color color;

    private void OnDrawGizmos()
    {
        DrawHandle(size);
    }
    public void DrawHandle(float size)
    {
        Color prevColor = Gizmos.color;
        Gizmos.color = color;
        Gizmos.DrawSphere(transform.position, size);

        Gizmos.color = prevColor;
    }
}