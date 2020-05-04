using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(GUIReadOnly))]
public class GUIReadOnlyDrawer : PropertyDrawer
{    
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.PropertyField(position, property, label);
    }
}
