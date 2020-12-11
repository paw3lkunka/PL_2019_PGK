using UnityEngine;
using TMPro;

public class VersionNumber : MonoBehaviour
{
    private void OnValidate()
    {
        GetComponent<TextMeshProUGUI>().text = Application.version; ;
    }

    private void Awake()
    {
        GetComponent<TextMeshProUGUI>().text = Application.version; ;
    }
}
