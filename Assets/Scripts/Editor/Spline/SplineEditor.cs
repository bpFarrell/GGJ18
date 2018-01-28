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

namespace SplineLogic
{ 
    [CustomEditor(typeof(Spline))]
    public class SplineEditor : Editor {
        public bool debug = true;
        SerializedProperty nodes;
        SerializedProperty handleSize;
        //SerializedProperty hiddenContainer;
        SerializedProperty segments;
        SerializedProperty debugNodeLine;
        SerializedProperty debugPathLine;

        void OnEnable()
        {
            nodes = serializedObject.FindProperty("nodes");
            handleSize = serializedObject.FindProperty("handleSize");
            //hiddenContainer = serializedObject.FindProperty("hiddenContainer");
            segments = serializedObject.FindProperty("segments");
            debugNodeLine = serializedObject.FindProperty("debugNodeLine");
            debugPathLine = serializedObject.FindProperty("debugPathLine");
        }
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(nodes);
            EditorGUILayout.PropertyField(handleSize);
            //EditorGUILayout.PropertyField(hiddenContainer);
            EditorGUILayout.PropertyField(segments);
            EditorGUILayout.PropertyField(debugNodeLine);
            EditorGUILayout.PropertyField(debugPathLine);

            debug = EditorGUILayout.Foldout(debug, "Debug");
            if(debug)
            {
                // Function Testing
                if(GUILayout.Button("Add Node"))
                {
                    (target as Spline).AddCurve();
                }
                if (GUILayout.Button("Remove Node"))
                {
                    (target as Spline).RemoveCurve();
                }
                if (GUILayout.Button("Show Nodes"))
                {
                    (target as Spline).HideContainer(false);
                }
                if (GUILayout.Button("Hide Nodes"))
                {
                    (target as Spline).HideContainer(true);
                }
                if (GUILayout.Button("Rebuild Node List"))
                {
                    (target as Spline).RebuildNodes();
                }
                if (GUILayout.Button("Clear Nodes"))
                {
                    (target as Spline).ClearNodes();
                }
                // Variable Display
                EditorGUI.BeginDisabledGroup(true);

                EditorGUI.EndDisabledGroup();
            }
            serializedObject.ApplyModifiedProperties();
        }
	
    }
}