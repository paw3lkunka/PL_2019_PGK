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
    private bool showOccurrences;

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
                generator.Generate();

            if (GUILayout.Button("Clear"))
                generator.Clear();
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
                int index = 0;
                foreach (GameObject prefab in generator.Locations)
                {
                    if (index == PrefabDatabase.Load.stdLocations.Count)
                    {
                        EditorGUILayout.Space();
                    }

                    Location location = prefab.GetComponent<Location>();
                    int value;
                    try
                    {
                        value = generator.spawnChances[index];
                    }
                    catch
                    {
                        value = 0;
                    }
                    int newValue = EditorGUILayout.IntField(prefab.name, value);
                    generator.spawnChances[index] = newValue > 0 ? newValue : 0;
                    index++;
                }
                EditorGUILayout.Space();
                generator.emptyChance = EditorGUILayout.IntField("empty cell", generator.emptyChance);
                EditorUtility.SetDirty(target);
            }
            EditorGUI.indentLevel--;
        }
    }
}
