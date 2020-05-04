using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(GUIName))]
public class GUINameDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        GUIName guiName = attribute as GUIName;
        string name = ObjectNames.NicifyVariableName(guiName.Text);

        EditorGUI.PropertyField(position, property, new GUIContent(name));
    }
}
