using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;

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

        GUILayout.BeginHorizontal(GUILayout.ExpandWidth(false));
        {
            if (GUILayout.Button("Generate"))
            {
                generator.Generate();
                EditorUtility.SetDirty(target);
            }

            if (GUILayout.Button("Clear"))
            {
                generator.Clear();
                EditorUtility.SetDirty(target);
            }
        }
        GUILayout.EndHorizontal();
    
        if (generator.IsValid)
        {
            validationLog = "Generator state is valid";
            validationLogStyle.normal.textColor = Color.green * Color.gray;
        }
        else
        {
            validationLog = "Generator state is invalid!";
            validationLogStyle.normal.textColor = Color.red;
        }

        GUILayout.Label(validationLog, validationLogStyle);

        base.OnInspectorGUI();

        if( GUILayout.Button( (showSpawnChances ? "Hide" : "Show") + " spawn chances") )
        {
            showSpawnChances = !showSpawnChances;
        }

        if(showSpawnChances)
        {
            EditorGUI.indentLevel++;
            {
                GUILayout.Label("Locations:", EditorStyles.boldLabel);
                    DrawSpawnChances(generator.Locations, generator.locationSpawnChances);
                    EditorGUILayout.Space();
                
                    generator.emptyChance = EditorGUILayout.IntField("empty cell", generator.emptyChance);

                GUILayout.Label("Shrines:", EditorStyles.boldLabel);
                    DrawSpawnChances(generator.Shrines, generator.shrinesSpawnChances);

                GUILayout.Label("Enviro:", EditorStyles.boldLabel);
                    DrawSpawnChances(generator.Enviro, generator.enviroSpawnChances);

                EditorUtility.SetDirty(target);
            }
            EditorGUI.indentLevel--;
        }
    }

    private void DrawSpawnChances<T>(IList<T> objects, IList<int> chances)
    {
        int index = 0;
        foreach (T obj in objects)
        {
            string name = "Unknown";

            if (obj is GameObject)
            {
                name = (obj as GameObject).name;
            }
            else if (obj is LocationsPool)
            {
                name = "Pool: " + (obj as LocationsPool).locations[0].name;
            }

            int value;
            try
            {
                value = chances[index];
            }
            catch
            {
                value = 0;
            }
            int newValue = EditorGUILayout.IntField(name, value);
            chances[index] = newValue > 0 ? newValue : 0;
            index++;
        }
    }
}
