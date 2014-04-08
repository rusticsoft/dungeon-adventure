using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(InputTranslator))]
public class InputTranslatorEditor : Editor {

    public override void OnInspectorGUI() {
        EditorGUILayout.LabelField ("Some help", "Some other text");

        //http://docs.unity3d.com/Documentation/ScriptReference/Editor.html
        //http://docs.unity3d.com/Documentation/ScriptReference/EditorGUILayout.html
        //http://docs.unity3d.com/Documentation/ScriptReference/Editor.DrawDefaultInspector.html
        //moreOpts.FpsLook = EditorGUILayout.

        //target.speed = EditorGUILayout.Slider ("Speed", target.speed, 0, 100);
        // Show default inspector property editor
		EditorGUILayout.LabelField("Time since start: ",
            EditorApplication.timeSinceStartup.ToString());

        DrawDefaultInspector();
     }
}