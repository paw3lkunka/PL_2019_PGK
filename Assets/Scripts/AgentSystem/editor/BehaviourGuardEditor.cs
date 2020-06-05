using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(BehaviourGuard))]
public class BehaviourGuardEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var behaviourGuard = target as BehaviourGuard;

        if (GUILayout.Button("Set post here"))
        {
            behaviourGuard.SetPostHere();
            EditorUtility.SetDirty(target);
        }

        base.OnInspectorGUI();
    }
}
