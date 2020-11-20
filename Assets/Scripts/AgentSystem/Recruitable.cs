using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Recruitable : MonoBehaviour
{
#pragma warning disable
    [field: SerializeField, GUIName("FaithCost")]
    public float FaithCost { get; private set; } = 5;
    [field: SerializeField, GUIName("MinimumFaithToRecruit")]
    public float MinimumFaithToRecruit { get; private set; } = 15;
#pragma warning restore

    private bool inRange = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Leader"))
        {
            UIOverlayManager.Instance.ControlsSheet.AddSheetElement(ButtonActionType.Walk, "Recruit new member");
            inRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Leader"))
        {
            UIOverlayManager.Instance.ControlsSheet.RemoveSheetElement(ButtonActionType.Walk);
            inRange = false;
        }
    }

    private void OnMouseDown()
    {
        if (inRange && !RecruitUIController.isRecruitUIOn)
        {
            GameObject recruitGUI = UIOverlayManager.Instance.PushToCanvas(ApplicationManager.Instance.PrefabDatabase.recruitGUI);
            recruitGUI.GetComponent<RecruitUIController>().recruit = this;
            BehaviourUserInput.controlEnabled = false;
            // TODO: Make it more universal
            GetComponent<BehaviourRandom>().enabled = false;
        }
    }

    public bool Recruit()
    {
        if (FaithCost < GameplayManager.Instance.Faith && MinimumFaithToRecruit < GameplayManager.Instance.Faith)
        {
            UIOverlayManager.Instance.ControlsSheet.RemoveSheetElement(ButtonActionType.Walk);
            GameplayManager.Instance.Faith -= FaithCost;

            var newCultist = new CultistEntityInfo(ApplicationManager.Instance.PrefabDatabase.cultists[0]);
            newCultist.Instantiate(transform.position, transform.rotation);
            GameplayManager.Instance.cultistInfos.Add(newCultist);

            Destroy(gameObject);
            return true;
        }
        else
        {
            return false;
        }
    }
}
