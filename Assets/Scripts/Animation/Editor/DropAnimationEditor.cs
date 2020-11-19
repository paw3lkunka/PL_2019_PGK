using UnityEditor.SceneManagement;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;

[CustomEditor(typeof(DropAnimation))]
public class DropAnimationEditor : Editor
{
    public float durationRangeFrom;
    public float durationRangeTo;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        DropAnimation script = target as DropAnimation;
        //set up variables
        durationRangeFrom = script.durationRange.x;
        durationRangeTo = script.durationRange.y;

        EditorGUILayout.LabelField("Drop roll animation duration (s)", EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("From: " + script.durationRange.x.ToString());
        EditorGUILayout.LabelField("To: " + script.durationRange.y.ToString());
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.MinMaxSlider(ref durationRangeFrom, ref durationRangeTo, 0, 5);

        if (GUI.changed)
        {
            // round to 2 decimal places
            durationRangeFrom = (float)System.Math.Round((double)durationRangeFrom, 2);
            durationRangeTo = (float)System.Math.Round((double)durationRangeTo, 2);
            script.durationRange.x = durationRangeFrom;
            script.durationRange.y = durationRangeTo;

            EditorUtility.SetDirty(script);
            EditorSceneManager.MarkSceneDirty(script.gameObject.scene);
        }
    }
}
