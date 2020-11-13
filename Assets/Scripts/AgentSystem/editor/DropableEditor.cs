using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEditor;
using UnityEditor.UIElements;

[CustomEditor(typeof(Dropable))]
public class DropableEditor : Editor
{
    private float amountRangeFrom;
    private float amountRangeTo;

    private float positionRangeFrom;
    private float positionRangeTo;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        Dropable script = target as Dropable;
        //set up variables
        amountRangeFrom = script.dropAmountRange.x;
        amountRangeTo = script.dropAmountRange.y;
        positionRangeFrom = script.dropPositionRange.x;
        positionRangeTo = script.dropPositionRange.y;

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("How Many Prefabs will drop? (Amount Range)", EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("From: " + script.dropAmountRange.x.ToString());
        EditorGUILayout.LabelField("To: " + script.dropAmountRange.y.ToString());
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.MinMaxSlider(ref amountRangeFrom, ref amountRangeTo, 0, 15);

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("How Far Prefabs will drop? (Position Range)", EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("From: " + script.dropPositionRange.x.ToString());
        EditorGUILayout.LabelField("To: " + script.dropPositionRange.y.ToString());
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.MinMaxSlider(ref positionRangeFrom, ref positionRangeTo, -10, 10);

        if (GUI.changed)
        {
            // round to int
            amountRangeFrom = Mathf.RoundToInt(amountRangeFrom);
            amountRangeTo = Mathf.RoundToInt(amountRangeTo);
            script.dropAmountRange.x = Mathf.RoundToInt(amountRangeFrom);
            script.dropAmountRange.y = Mathf.RoundToInt(amountRangeTo);


            // round to 2 decimal places
            positionRangeFrom = (float)System.Math.Round((double)positionRangeFrom, 1);
            positionRangeTo = (float)System.Math.Round((double)positionRangeTo, 1);
            script.dropPositionRange.x = positionRangeFrom;
            script.dropPositionRange.y = positionRangeTo;

            EditorUtility.SetDirty(script);
            EditorSceneManager.MarkSceneDirty(script.gameObject.scene);
        }
    }
}
