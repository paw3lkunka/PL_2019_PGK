using System.Diagnostics;
using UnityEditor;
using UnityEngine;
using UnityToolbarExtender;

[InitializeOnLoad]
public class GitBashHere
{
    static GitBashHere()
    {
        ToolbarExtender.LeftToolbarGUI.Add(OnToolbarGUI);
    }

    static void OnToolbarGUI()
    {
        GUILayout.FlexibleSpace();

        if (GUILayout.Button(new GUIContent("Git bash here", (Texture)EditorGUIUtility.Load("Assets/GitBashHere/git_icon.png"), "Git bash here"), "AppCommand"))
        {
            Process.Start("C:\\Program Files\\Git\\git-bash.exe");
        }
    }
}