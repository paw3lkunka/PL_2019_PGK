using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(GUIName))]
public class GUINameDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        GUIName guiName = attribute as GUIName;
        label.text = ObjectNames.NicifyVariableName(guiName.Text);

        //HACK
        if (property.type == "Resource")
        {
            //new ResourcePropertyDrawer().OnGUI(position, property, label);
        }
        else
        {
            EditorGUI.PropertyField(position, property, label);
        }


    }
}
