using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CultLeader : MonoBehaviour
{

    #region MonoBehaviour
    private void Awake()
    {
        LocationManager.Instance.cultLeader = this;
    }

    #endregion
}
