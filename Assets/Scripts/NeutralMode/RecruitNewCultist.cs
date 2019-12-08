using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecruitNewCultist : MonoBehaviour
{
    public GameObject dialogBox;

    private GameObject instanced;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("NPC") && instanced == null)
        {
            instanced = Instantiate(dialogBox, Vector3.zero, Quaternion.identity, GameManager.Gui.UICanvas.transform);

            instanced.GetComponent<DialogBox>().caller = other.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if ( other.gameObject.CompareTag("NPC") )
        {
            instanced = null;
        }
    }
}
