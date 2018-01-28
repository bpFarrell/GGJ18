/*
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
        public static Spline instance;
        public List<SplineNode> nodes = new List<SplineNode>();
        private Transform _hiddenContainer;
        private Transform hiddenContainer
        {
            get
            {
                if (_hiddenContainer != null) return _hiddenContainer;
                else
                {
                    _hiddenContainer = transform.Find("Container");
                    if (_hiddenContainer == null)
                    {
                        _hiddenContainer = new GameObject("Container").transform;
                        _hiddenContainer.transform.parent = transform;
                    }
                }
                return _hiddenContainer;
            }
        }
        public int segments = 6;
        public int NodesCount
        {
            get { return nodes.Count; }
        }
        public int totalSegments
        {
            get { return NodesCount * segments; }
        }
        public int numberOfCurves
        {
            get { return (NodesCount - 1);  }
        }

        public bool debugNodeLine;
        public bool debugPathLine;

        public float handleSize = 0.3f;

        private void Awake()
        {
            instance = this;
            RebuildNodes();
        }

        public void RebuildNodes()
        {
            nodes = hiddenContainer.GetComponentsInChildren<SplineNode>().ToList();
        }

        public SplineNode AddCurve()
        {
            if (NodesCount == 0) AddNode();

            Transform trans = nodes.Last().transform;
            return AddCurve(nodes.Last().transform.position + (Vector3.forward * handleSize * 3), trans.rotation, trans.localScale.z);
        }

        public SplineNode AddCurve(Vector3 pos, Quaternion rot, float zScale)
        {
            return AddNode(pos, rot, zScale);
        }

        private SplineNode AddNode()
        {
            if (NodesCount == 0) return AddNode(transform.position, transform.rotation, transform.localScale.z);
            else return null;
        }
        private SplineNode AddNode(Vector3 pos, Quaternion rot, float zScale)
        {
            GameObject go = new GameObject("Node");
            
            go.transform.parent = hiddenContainer.transform;
            SplineNode script = go.AddComponent<SplineNode>();
            script.size = handleSize;
            script.Init();
            go.transform.position = pos;
            go.transform.rotation = rot;
            go.transform.localScale = new Vector3(1, 1, zScale);
            nodes.Add(script);
            return script;
        }

        public void RemoveCurve()
        {
            RemoveNode();
        }

        private void RemoveNode()
        {
            if (NodesCount <= 0) return;
            GameObject go = nodes.Last().gameObject;
            nodes.Remove(nodes.Last());
            DestroyImmediate(go);
        }

        public void ClearNodes()
        {
            int count = NodesCount;
            for (int i = 0; i < count; i++)
            {
                RemoveNode();
            }
        }
        private void OnDrawGizmos()
        {
            if (nodes == null) RebuildNodes();
            if (NodesCount <= 1) return;
            if (debugNodeLine)
            {
                for (int i = 0; i < NodesCount - 1; i++)
                {
                    nodes[i].size = handleSize;
                    Gizmos.DrawLine(nodes[i].transform.position, nodes[i + 1].transform.position);
                }
            }
            if (debugPathLine)
            {
                Vector3 lastPoint = nodes.First().transform.position;
                Gizmos.color = Color.blue;
                for (int x = 1; x <= totalSegments; x++)
                {
                    Vector3 point = EvaluatePosition((float)((float)x / (float)totalSegments) * (float)numberOfCurves);
                    Gizmos.DrawLine(lastPoint, point);
                    lastPoint = point;
                }
            }
        }
        public Vector3 EvaluatePosition(float val)
        {
            val = Mathf.Clamp(val, 0, numberOfCurves);
            float t = val % 1;
            int curve = (int)(val - t);
            if (curve == numberOfCurves) return nodes.Last().transform.position;

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
        public Quaternion GetBezierRotation(Quaternion q0, Quaternion q1, Quaternion q2, Quaternion q3, float t)
        {
            float oneMinus = 1f - t;
            Quaternion q4 = Quaternion.Lerp(q0, q1, t);
            Quaternion q5 = Quaternion.Lerp(q1, q2, t);
            Quaternion q6 = Quaternion.Lerp(q2, q3, t);
            Quaternion q7 = Quaternion.Lerp(q4, q5, t);
            Quaternion q8 = Quaternion.Lerp(q5, q6, t);
            return Quaternion.Lerp(q7, q8, t);
        }
        public Vector3 EvaluateSurfaceTangent(float val)
        {
            val = Mathf.Clamp(val, 0, numberOfCurves);
            float t = val % 1;
            int curve = (int)(val - t);
            if (curve == numberOfCurves) return nodes.Last().transform.position;

            int startIndex = curve;

            return getBezierForward(nodes[startIndex + 0].transform.position,
                                  nodes[startIndex + 0].exit,
                                  nodes[startIndex + 1].enter,
                                  nodes[startIndex + 1].transform.position,
                                  t);
        }
        public Vector3 getBezierForward(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
        {
            float oneMinus = 1f - t;
            return 3f * oneMinus * oneMinus * (p1 - p0) +
                6f * oneMinus * t * (p2 - p1) +
                3f * t * t * (p3 - p2);
        }
        public Quaternion EvaluateRotation(float val)
        {
            val = Mathf.Clamp(val, 0, numberOfCurves);
            float t = val % 1;
            int curve = (int)(val - t);
            if (curve == numberOfCurves) return nodes.Last().transform.rotation;

            int startIndex = curve;

            Quaternion lerpRotation = GetBezierRotation(nodes[startIndex + 0].transform.rotation,
                                  nodes[startIndex + 0]._exitHandle.rotation,
                                  nodes[startIndex + 1]._enterHandle.rotation,
                                  nodes[startIndex + 1].transform.rotation,
                                  t);

            Vector3 splineForward = EvaluateSurfaceTangent(val);
            Vector3 splineLeft = Vector3.Cross(splineForward, lerpRotation * Vector3.up);
            Vector3 splineUp = Vector3.Cross(splineLeft, splineForward);

            return Quaternion.LookRotation(splineForward, splineUp);
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