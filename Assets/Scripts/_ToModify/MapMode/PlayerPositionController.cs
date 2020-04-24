using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPositionController : MonoBehaviour
{
    #region Variables

    public Vector2 Delta { get; private set; }
    public bool Moved { get => Delta != Vector2.zero; }

    private Vector2 lastPos;

    #endregion

    #region MonoBehaviour

    void Start()
    {
        LoadPosition();
        ApplicationManager.Gui.Initialize();
    }
    private void Update()
    {
        Delta = (Vector2)transform.position - lastPos;
        lastPos = transform.position;
    }

    #endregion

    #region Component

    public void LoadPosition()
    {
        transform.position = ApplicationManager.Instance.savedPosition;
    }

    #endregion
}
