using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Security.AccessControl;

//#if UNITY_EDITOR

[CustomEditor(typeof(InstantResource))]
public class InstantResourceEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        InstantResource script = (InstantResource)target;

        if(!script.dissappearOnCollect)
        {
            script.onEmptyMaterial = (Material)EditorGUILayout.ObjectField("On Empty Material", script.onEmptyMaterial, typeof(Material), true);
            script.onFullMaterial = (Material)EditorGUILayout.ObjectField("On Full Material", script.onFullMaterial, typeof(Material), true);
            script.indicator = (GameObject)EditorGUILayout.ObjectField("Indicator", script.indicator, typeof(GameObject), true);
        }
        else if( script.Type == ResourceType.Water)
        {
            script.appendMaxValue = EditorGUILayout.FloatField("Append Max Value", script.appendMaxValue);
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(script);
            EditorSceneManager.MarkSceneDirty(script.gameObject.scene);
        }

    }
}

//#endif
