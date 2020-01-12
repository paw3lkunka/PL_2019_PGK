using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapGenerator))]
public class MapGeneratorEditor : Editor
{
    private bool showSpawnChances;

    public override void OnInspectorGUI()
    {
        MapGenerator generator = (MapGenerator)target;

        GUILayout.BeginHorizontal(GUILayout.ExpandWidth(false));
        {
            if (GUILayout.Button("Validate"))
                generator.ValidatePrefabs();

            if (GUILayout.Button("Generate"))
                generator.Generate();

            if (GUILayout.Button("Clear"))
                generator.Clear();
        }
        GUILayout.EndHorizontal();
        
        base.OnInspectorGUI();

        if( GUILayout.Button( (showSpawnChances ? "Hide" : "Show") + " spawn chances") )
        {
            showSpawnChances = !showSpawnChances;
        }

        if(showSpawnChances)
        {
            EditorGUI.indentLevel++;
            {
                foreach (GameObject prefab in generator.locationPrefabs)
                {
                    Location location = prefab.GetComponent<Location>();
                    location.spawnChance = EditorGUILayout.IntField(prefab.name, location.spawnChance);
                }
                generator.emptyChance = EditorGUILayout.IntField("empty", generator.emptyChance);
            }
            EditorGUI.indentLevel--;
        }
        
    }
}
