using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(Resource))]
public class ResourcePropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var floatrect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        var boolrect = new Rect(position.x + position.width - EditorGUIUtility.singleLineHeight, position.y, EditorGUIUtility.singleLineHeight, EditorGUIUtility.singleLineHeight);
        var boollabelrect = new Rect(position.x + position.width * 0.83f, position.y, position.width * 0.17f, EditorGUIUtility.singleLineHeight);

        SerializedProperty currentProp = property.FindPropertyRelative("current");
        SerializedProperty maxProp = property.FindPropertyRelative("max");
        SerializedProperty overflowable = property.FindPropertyRelative("overflowable");

        var buff = new float[2];
        var sublabels = new GUIContent[2];

        buff[0] = currentProp.floatValue;
        buff[1] = maxProp.floatValue;

        sublabels[0] = new GUIContent("curr");
        sublabels[1] = new GUIContent("max");

        EditorGUI.MultiFloatField(floatrect, label, sublabels, buff);

        currentProp.floatValue = buff[0];
        maxProp.floatValue = buff[1];

        EditorGUI.LabelField(boollabelrect, "overfw");
        overflowable.boolValue = EditorGUI.Toggle(boolrect, overflowable.boolValue);

        if (currentProp.floatValue > maxProp.floatValue)
        {
            currentProp.floatValue = maxProp.floatValue;
        }

        if (currentProp.floatValue < 0)
        {
            currentProp.floatValue = 0;
        }
    }
}
