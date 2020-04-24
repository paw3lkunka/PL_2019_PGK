using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecruitNewCultist : MonoBehaviour
{
    #region Variables

    public GameObject dialogBox;
    private GameObject instanced;

    #endregion

    #region MonoBehaviour

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("NPC") && instanced == null)
        {
            //instanced = Instantiate(dialogBox, Vector3.zero, Quaternion.identity, ApplicationManager.Gui.UICanvas.transform);

            instanced.GetComponent<DialogBox>().caller = other.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("NPC"))
        {
            instanced = null;
        }
    }

    #endregion

    #region Component



    #endregion
}
