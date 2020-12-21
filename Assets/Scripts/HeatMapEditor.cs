using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(HeatMap))]
public class HeatMapEditor : Editor
{
    public override void OnInspectorGUI()
    {
        HeatMap heatMapScript = (HeatMap)target;

        DrawDefaultInspector();

        if (EventHandler.instance && EventHandler.instance.sessions != null)
        {
            EditorGUILayout.LabelField("Current Session ID: " + EventHandler.instance.sessions[0]);
            string[] list = EventHandler.instance.sessions.ToArray();
            EditorGUILayout.LabelField("Session ID");
            heatMapScript.sessionChoice = EditorGUILayout.Popup(heatMapScript.sessionChoice, list);
        }

        GUILayout.Label("", GUI.skin.horizontalSlider);
        if (GUILayout.Button("Build Map"))
            heatMapScript.CreateMap();
        if (GUILayout.Button("Clear Map"))
            heatMapScript.ClearHeatMap();

    }
}
