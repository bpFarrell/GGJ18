﻿/*
////////////////////////////////////////////////////////////////////
 Jason Dean Crossley

 Notes: Credits to a very nice tutorial that gives tons of unity specific examples and explanation of splines
 http://catlikecoding.com/unity/tutorials/curves-and-splines/
\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SplineLogic
{
    public class Spline : MonoBehaviour
    {
        public List<SplineNode> nodes = new List<SplineNode>();
        public GameObject hiddenContainer;
        public int segments = 6;
        public int length
        {
            get { return nodes.Count; }
        }
        public int count
        {
            get { return length * segments; }
        }
        public int curves
        {
            get { return (length - 1);  }
        }

        public bool debugNodeLine;
        public bool debugPathLine;

        public float handleSize = 0.3f;

        private void Start()
        {
            Init();
        }

        private void Init()
        {
            nodes = GetComponentsInChildren<SplineNode>().ToList();
        }

        public void AddCurve()
        {
            if (length == 0) AddNode();

            AddNode();
        }

        private void AddNode()
        {
            GameObject go = new GameObject("Node");
            if (length > 0) go.transform.position = nodes.Last().transform.position + (Vector3.left * handleSize*3);
            else go.transform.position = transform.position;
            go.transform.parent = hiddenContainer.transform;
            SplineNode script = go.AddComponent<SplineNode>();
            script.size = handleSize;
            script.Init();
            nodes.Add(script);
        }

        public void RemoveCurve()
        {
            RemoveNode();
        }

        private void RemoveNode()
        {
            if (length <= 0) return;
            GameObject go = nodes.Last().gameObject;
            nodes.Remove(nodes.Last());
            DestroyImmediate(go);
        }

        private void OnDrawGizmos()
        {
            if (nodes == null) Init();
            if (length <= 1) return;
            if (debugNodeLine)
            {
                for (int i = 0; i < length - 1; i++)
                {
                    nodes[i].size = handleSize;
                    Gizmos.DrawLine(nodes[i].transform.position, nodes[i + 1].transform.position);
                }
            }
            if (debugPathLine)
            {
                Vector3 lastPoint = nodes.First().transform.position;
                Gizmos.color = Color.blue;
                for (int x = 1; x <= count; x++)
                {
                    Vector3 point = EvaluatePosition((float)((float)x / (float)count) * (float)curves);
                    Gizmos.DrawLine(lastPoint, point);
                    lastPoint = point;
                }
            }
        }
        public Vector3 EvaluatePosition(float val)
        {
            val = Mathf.Clamp(val, 0, curves);
            float t = val % 1;
            int curve = (int)(val - t);
            if (curve == curves) return nodes.Last().transform.position;


            int startIndex = curve;

            return GetBezierPoint(nodes[startIndex + 0].transform.position,
                                  nodes[startIndex + 0].exit,
                                  nodes[startIndex + 1].enter,
                                  nodes[startIndex + 1].transform.position,
                                  t);
        }
        public Vector3 GetBezierPoint(Vector3 p0, Vector3 p1, Vector3 p2, float t)
        {
            // found a helpful tutorial online that solved the cubic bezier in a quadratic function. 
            // Allows for the computation of a derivative for velocity at a given point on the curve.
            float oneMinus = 1f - t;
            return oneMinus * oneMinus * p0 + 2f * oneMinus * t * p1 + t * t * p2;
        }
        public Vector3 GetBezierPoint(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
        {
            // found a helpful tutorial online that solved the quadratic bezier in a quadratic function. 
            // Allows for the computation of a derivative for velocity at a given point on the curve.
            float oneMinus = 1f - t;
            return oneMinus * oneMinus * oneMinus * p0 +
                3f * oneMinus * oneMinus * t * p1 +
                3f * oneMinus * t * t * p2 + 
                t * t * t * p3;
        }
        public Quaternion EvaluateRotation(float val)
        {

            Quaternion value = new Quaternion();

            return value;
        }
        public void HideContainer(bool b)
        {
            if (b)
                hiddenContainer.hideFlags = HideFlags.HideInHierarchy;
            else
                hiddenContainer.hideFlags = HideFlags.None;
        }
    }
}