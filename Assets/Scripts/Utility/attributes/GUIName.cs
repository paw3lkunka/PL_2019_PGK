using UnityEngine;

public class GUIName : PropertyAttribute
{
    public string Text { get; private set; }

    public GUIName(string name)
    {
        Text = name;
    }


}
