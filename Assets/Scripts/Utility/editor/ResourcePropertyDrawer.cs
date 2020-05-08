using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(Resource))]
public class ResourcePropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var currentRect = new Rect(position.x, position.y, position.width * 0.666666666f, EditorGUIUtility.singleLineHeight);
        var maxRect = new Rect(position.x + position.width * 0.666666666f, position.y, position.width * 0.333333333f, EditorGUIUtility.singleLineHeight);

        SerializedProperty currentProp = property.FindPropertyRelative("current");
        SerializedProperty maxProp = property.FindPropertyRelative("max");

        GUIContent currentLabel = new GUIContent(label.text + " (value,max)");
        GUIContent maxLabel = new GUIContent();


        currentProp.floatValue = EditorGUI.FloatField(currentRect, currentLabel, currentProp.floatValue);
        maxProp.floatValue = EditorGUI.FloatField(maxRect, maxLabel, maxProp.floatValue);

        if(currentProp.floatValue > maxProp.floatValue)
        {
            currentProp.floatValue = maxProp.floatValue;
        }

        if (currentProp.floatValue < 0)
        {
            currentProp.floatValue = 0;
        }
    }
}
