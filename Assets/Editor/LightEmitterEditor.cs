using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LightEmitter))]
public class LightEmitterEditor : Editor {

    private LightEmitter script;
    private bool safeToDraw;

    private void OnEnable() {
        safeToDraw = false;
    }

    public override void OnInspectorGUI() {
        serializedObject.Update();
        script = target as LightEmitter;

        script.originOffset = EditorGUILayout.Vector2Field("Origin Offset", script.originOffset);
        script.LightRange = EditorGUILayout.IntSlider("Light Range", script.LightRange, 0, 180);
        script.LightDirection = EditorGUILayout.IntSlider("Light Direction", script.LightDirection, 0, 359);
        script.LightDistance = EditorGUILayout.FloatField("Light Distance", script.LightDistance);
        if (GUI.changed) {
            Undo.RegisterCompleteObjectUndo(script, "Light Emitter Change");
        }

        safeToDraw = true;
        serializedObject.ApplyModifiedProperties();
        SceneView.lastActiveSceneView.Repaint();
    }

    public void OnSceneGUI() {

        if (safeToDraw) {
            Vector3 thisPosition = script.transform.position + (Vector3)script.originOffset;
            EditorGUI.BeginChangeCheck();

            Vector2 angleUpper = new Vector2(Mathf.Cos(Mathf.Deg2Rad * (script.LightDirection + script.LightRange / 2)), Mathf.Sin(Mathf.Deg2Rad * (script.LightDirection + script.LightRange / 2)));
            Vector2 angleLower = new Vector2(Mathf.Cos(Mathf.Deg2Rad * (script.LightDirection - script.LightRange / 2)), Mathf.Sin(Mathf.Deg2Rad * (script.LightDirection - script.LightRange / 2)));

            Handles.color = Color.yellow;
            Handles.DrawDottedLine(thisPosition, (Vector3)angleUpper * script.LightDistance + thisPosition, 2f);
            Handles.DrawDottedLine(thisPosition, (Vector3)angleLower * script.LightDistance + thisPosition, 2f);
            Handles.DrawWireArc(thisPosition, new Vector3(0,0,1), angleLower, script.LightRange, script.LightDistance);
        }
        else {
            SceneView.RepaintAll();
        }
    }
}