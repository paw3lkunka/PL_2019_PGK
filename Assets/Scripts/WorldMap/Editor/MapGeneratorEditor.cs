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

                int index = 0;
                foreach (GameObject prefab in generator.Locations)
                {
                    if (index == PrefabDatabase.Load.stdLocations.Count)
                    {
                        EditorGUILayout.Space();
                    }

                    var location = prefab.GetComponent<Location>();
                    int value;
                    try
                    {
                        value = generator.locationSpawnChances[index];
                    }
                    catch
                    {
                        value = 0;
                    }
                    int newValue = EditorGUILayout.IntField(prefab.name, value);
                    generator.locationSpawnChances[index] = newValue > 0 ? newValue : 0;
                    index++;
                }
                EditorGUILayout.Space();
                generator.emptyChance = EditorGUILayout.IntField("empty cell", generator.emptyChance);

                EditorGUILayout.Space();
                GUILayout.Label("Enviro:", EditorStyles.boldLabel);

                index = 0;
                foreach (GameObject prefab in generator.Enviro)
                {
                    var location = prefab.GetComponent<EnviroObject>();
                    int value;
                    try
                    {
                        value = generator.enviroSpawnChances[index];
                    }
                    catch
                    {
                        value = 0;
                    }
                    int newValue = EditorGUILayout.IntField(prefab.name, value);
                    generator.enviroSpawnChances[index] = newValue > 0 ? newValue : 0;
                    index++;
                }

                EditorUtility.SetDirty(target);
            }
            EditorGUI.indentLevel--;
        }
    }
}
