/*
////////////////////////////////////////////////////////////////////
 Jason Dean Crossley

 Notes:
\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
*/

using SplineLogic;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackBuilder : MonoBehaviour {

    public Spline spline;
    public bool generationEnabled = false;
    public int length;
    public float stepSize = 0.001f;
	// Use this for initialization
	void Start () {
        if (generationEnabled)
            CreateTrackNodes();

        SpawnSlabs();
	}

    public void SpawnSlabs()
    {
        SlabController.SpawnSlab(spline.nodes[0].transform.position, spline.nodes[0].transform.rotation, 0);

        float deltaDistance = 0f;
        for (float counter = 0; counter <= spline.numberOfCurves; counter += stepSize)
        {
            Vector3 counterPoint = spline.EvaluatePosition(counter);
            Vector3 nextPoint = spline.EvaluatePosition(counter + stepSize);
            float distanceBetweenPoints = (nextPoint - counterPoint).magnitude;
            deltaDistance += distanceBetweenPoints;
            if(deltaDistance >= 1f)
            {
                Quaternion rotation = spline.EvaluateRotation(counter);

                deltaDistance -= 1f;
                SlabController.SpawnSlab(counterPoint, rotation, counter);
            }
        }
        SlabController.FinalizeSlabs();
    }

    public void CreateTrackNodes()
    {
        spline.ClearNodes();
        CreateTrackNode(transform.position, Quaternion.identity, 8.5f, length);
    }
    private void CreateTrackNode(Vector3 lastPos, Quaternion lastRot, float lastZScale, int depth )
    {
        spline.AddCurve(lastPos, lastRot, lastZScale);
        if (depth <= 0) return;

        Vector3 circle = UnityEngine.Random.insideUnitCircle;
        Vector3 offset = lastRot * ((Vector3.forward + (circle*2f)) * 30);

        Quaternion fromTo = Quaternion.FromToRotation(lastRot * Vector3.forward, offset);
        Quaternion rotation = Quaternion.RotateTowards( lastRot, fromTo, 20f);

        float zScale = lastZScale + UnityEngine.Random.Range(-1f, 1f);

        CreateTrackNode(lastPos + offset, rotation, zScale, --depth);
    }
}