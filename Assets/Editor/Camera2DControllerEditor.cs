using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Camera2DController))]
public class Camera2DControllerEditor : Editor {

    private Camera2DController script;

    public override void OnInspectorGUI() {
        script = target as Camera2DController;
        DrawDefaultInspector();
    }

    public void OnSceneGUI() {
        if (script == null) return;

        Vector3 position = script.transform.position;
        Vector3 min = script.minConstraints;
        Vector3 max = script.maxConstraints;

        Vector3[] verts = new Vector3[] {
            new Vector3(min.x, min.y, position.z),
            new Vector3(max.x, min.y, position.z),
            new Vector3(max.x, max.y, position.z),
            new Vector3(min.x, max.y, position.z)
        };

        Handles.DrawSolidRectangleWithOutline(verts, Color.clear, Color.white);
    }
}