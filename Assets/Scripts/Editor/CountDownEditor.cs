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

[CustomEditor(typeof(CountDown))]
public class CountDownEditor : Editor {
    private bool debug = true;

    SerializedProperty sprites;

    void OnEnable()
    {
        sprites = serializedObject.FindProperty("sprites");
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(sprites, true);

        debug = EditorGUILayout.Foldout(debug, "Debug");
        if (debug)
        {
            // Function Testing
            if (GUILayout.Button("Reset and Start"))
            {
                CountDown.SetAndCount();
            }
            // Variable Display
            EditorGUI.BeginDisabledGroup(true);

            EditorGUI.EndDisabledGroup();
        }
        serializedObject.ApplyModifiedProperties();
    }
	
}