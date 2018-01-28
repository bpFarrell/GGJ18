/*
////////////////////////////////////////////////////////////////////
 Jason Dean Crossley

 Notes:
\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TrackBuilder))]
public class TrackBuilderEditor : Editor {
    private bool debug = true;

    //SerializedProperty lookAtPoint;
    public SerializedProperty spline;
    public SerializedProperty generationEnabled;
    public SerializedProperty length;
    public SerializedProperty stepSize;

    void OnEnable()
    {
        //lookAtPoint = serializedObject.FindProperty("lookAtPoint");
        spline = serializedObject.FindProperty("spline");
        generationEnabled = serializedObject.FindProperty("generationEnabled");
        length = serializedObject.FindProperty("length");
        stepSize = serializedObject.FindProperty("stepSize");
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        //EditorGUILayout.PropertyField(lookAtPoint);
        EditorGUILayout.PropertyField(spline);
        EditorGUILayout.PropertyField(generationEnabled);
        EditorGUILayout.PropertyField(length);
        EditorGUILayout.PropertyField(stepSize);

        debug = EditorGUILayout.Foldout(debug, "Debug");
        if (debug)
        {
            // Function Testing
            if (GUILayout.Button("Build Track"))
            {
                (target as TrackBuilder).SpawnSlabs();
            }
            if (GUILayout.Button("Build Track"))
            {
                (target as TrackBuilder).CreateTrackNodes();
            }
            // Variable Display
            EditorGUI.BeginDisabledGroup(true);

            EditorGUI.EndDisabledGroup();
        }
        serializedObject.ApplyModifiedProperties();
    }
	
}