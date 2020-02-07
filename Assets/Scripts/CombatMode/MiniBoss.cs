using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniBoss : Enemy
{
    #region Variables



    #endregion

    #region MonoBehaviour



    #endregion

    #region Component

    public override void Die()
    {
        float gainedFaith = 0.05f;
        GameManager.Instance.Faith += gainedFaith;

        if (fatihTextEemitter)
        {
            fatihTextEemitter.Emit("+" + (int)Mathf.Round(gainedFaith * 100), Color.green, 3);
        }
        base.Die();
    }

    #endregion
}
