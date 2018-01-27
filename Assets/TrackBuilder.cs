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
    public float stepSize = 0.001f;
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


                Quaternion rotation = spline.EvaluateRotation(counter);

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