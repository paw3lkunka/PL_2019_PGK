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

        GUILayout.BeginHorizontal(GUILayout.ExpandWidth(false));
        {
            if (GUILayout.Button("Generate"))
            {
                generator.Generate(generator.transform.position);
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

                    SpawnChancesFields(generator.emptyChance, "Empty cell");

                GUILayout.Label("Enviro:", EditorStyles.boldLabel);
                    DrawSpawnChances(generator.Enviro, generator.enviroSpawnChances);

                EditorUtility.SetDirty(target);
            }
            EditorGUI.indentLevel--;
        }
    }

    private void SpawnChancesFields(SpawnChance spawnChances, string name)
    {
        for (int i = 0; i < MapGenerator.ZONES; i++)
        {
            int value;
            try
            {
                value = spawnChances.forZone[i];
            }
            catch
            {
                value = 0;
            }
            int newValue = EditorGUILayout.IntField($"Z{i} - {name}", value);
            spawnChances.forZone[i] = newValue > 0 ? newValue : 0;
        }

        EditorGUILayout.Space();
    }
    
    private class DSCPair<T>
    {
        public T obj;
        public SpawnChance chance;
    }

    private void DrawSpawnChances<T>(IList<T> objects, IList<SpawnChance> chances)
    {
        var pairs = objects.Zip(chances, (o, c) => new DSCPair<T> { obj = o, chance = c });
        foreach (var pair in pairs)
        {
            string name = "Unknown";

            if (pair.obj is GameObject)
            {
                name = (pair.obj as GameObject).name;
            }
            else if (pair.obj is LocationsPool)
            {
                name = "Pool: " + (pair.obj as LocationsPool).locations[0].name;
            }

            SpawnChancesFields(pair.chance, name);

        }
    }
}
