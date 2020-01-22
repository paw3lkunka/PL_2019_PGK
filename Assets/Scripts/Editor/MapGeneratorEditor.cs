using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

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
            /*
            if (GUILayout.Button("Validate"))
                generator.ValidatePrefabs();
                now invoked automaticly on validate
            */

            if (GUILayout.Button("Generate"))
                generator.Generate();

            if (GUILayout.Button("Clear"))
                generator.Clear();
        }
        GUILayout.EndHorizontal();
    
        if (generator.isValid)
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
                foreach (GameObject prefab in generator.locationPrefabs)
                {
                    Location location = prefab.GetComponent<Location>();
                    int inputValue = EditorGUILayout.IntField(prefab.name, location.spawnChance);
                    location.spawnChance = inputValue > 0 ? inputValue : 0;
                }
                generator.emptyChance = EditorGUILayout.IntField("empty", generator.emptyChance);
            }
            EditorGUI.indentLevel--;
        }

    }
}
