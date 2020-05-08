using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextEmitter : MonoBehaviour
{
    enum Mode
    {
        Manual,
        Damage,
        FaithGained,
        FaithLost
    }

#pragma warning disable
    [SerializeField] private Mode mode;
#pragma warning restore

    public void Emit(GameObject prefab, string text, Color color)
    {
        GameObject obj = Instantiate(prefab, transform.position, Quaternion.identity);
        obj.GetComponent<FloatingText>().Set(text);
        obj.GetComponentInChildren<TextMeshProUGUI>().color = color;
    }

    void DamageTextEmission(float damage) => Emit(ApplicationManager.Instance.PrefabDatabase.floatingTextResourceLost, "-" + damage.ToString("0.0"), Color.red);

    #region MonoBehaviour

    private void OnEnable()
    {
        switch (mode)
        {
            case Mode.Manual:
                break;
            case Mode.Damage:
                GetComponentInParent<Damageable>().DamageTaken += DamageTextEmission;
                break;
            case Mode.FaithGained:
                Debug.LogWarning("TextsEmitter.Mode.faithGained NOT IMPLEMENTED!");
                break;
            case Mode.FaithLost:
                Debug.LogWarning("TextsEmitter.Mode.faithLost NOT IMPLEMENTED!");
                break;
        }
    }

    private void OnDisable()
    {
        switch (mode)
        {
            case Mode.Manual:
                break;
            case Mode.Damage:
                var damageable = GetComponentInParent<Damageable>();
                if (damageable)
                {
                    damageable.DamageTaken -= DamageTextEmission;
                }
                break;
        }

    }


    #endregion
}
