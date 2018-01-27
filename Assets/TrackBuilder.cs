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
    public float stepSize = 0.01f;
	// Use this for initialization
	void Start () {
        CreateTrackNodes();

        SpawnSlabs();
	}

    private void SpawnSlabs()
    {
        float deltaDistance = 0f;
        for (float counter = 0; counter <= spline.numberOfCurves; counter += stepSize)
        {
            Vector3 counterPoint = spline.EvaluatePosition(counter);
            Vector3 nextPoint = spline.EvaluatePosition(counter + stepSize);
            float distanceBetweenPoints = (nextPoint - counterPoint).magnitude;
            deltaDistance += distanceBetweenPoints;
            if(deltaDistance >= 1f)
            {
                Vector3 splineForward = spline.EvaluateSurfaceTangent(counter);
                Vector3 splineLeft = Vector3.Cross(splineForward, Vector3.up);
                Vector3 splineUp = Vector3.Cross(splineLeft, splineForward);

                Vector3 rotation = Quaternion.LookRotation(splineForward, splineUp).eulerAngles;

                deltaDistance -= 1f;
                SlabController.SpawnSlab(counterPoint, rotation, counter);
            }
        }
    }

    private void CreateTrackNodes()
    {
        // hand built for the moment
    }
}