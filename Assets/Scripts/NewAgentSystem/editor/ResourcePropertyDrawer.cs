using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(Resource))]
public class ResourcePropertyDrawer : PropertyDrawer
{

    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        return base.CreatePropertyGUI(property);
    }

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
    }


    // Multifloat version
/*    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        SerializedProperty currentProp = property.FindPropertyRelative("current");
        SerializedProperty maxProp = property.FindPropertyRelative("max");

        float[] values =
        {
            currentProp.floatValue,
            maxProp.floatValue
        };

        GUIContent[] labels =
        {
            new GUIContent("value"),
            new GUIContent("max")
        };

        EditorGUI.MultiFloatField(position, label, labels, values);

        if (values[0] > values[1])
        {
            values[0] = values[1];
        }

        currentProp.floatValue = values[0];
        maxProp.floatValue = values[1];
    }*/
}
