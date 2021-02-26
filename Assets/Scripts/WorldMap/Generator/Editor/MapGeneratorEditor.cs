using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using System.Linq;

[CustomEditor(typeof(MapGenerator))]
public class MapGeneratorEditor : Editor
{
    string validationLog = null;
    GUIStyle validationLogStyle = new GUIStyle();

    private bool showSpawnChances;

    public override VisualElement CreateInspectorGUI()
    {
        validationLogStyle.alignment = TextAnchor.MiddleCenter;
        validationLogStyle.fontStyle = FontStyle.Bold;
        return base.CreateInspectorGUI();
    }

    public override void OnInspectorGUI()
    {
        MapGenerator generator = (MapGenerator)target;

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Preview"))
        {
            generator.Preview();
        }

        if (GUILayout.Button("Clean"))
        {
            generator.ClearPreview();
        }

        EditorGUILayout.EndHorizontal();

        base.OnInspectorGUI();
    }
}
