using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextsEmitter : MonoBehaviour
{
    enum Mode
    {
        damage,
        faithGained,
        faithLost
    }

#pragma warning disable
    [SerializeField] private Mode mode;
#pragma warning restore

    public void Emit(GameObject prefab,string text)
    {
        GameObject obj = Instantiate(prefab, transform.position, Quaternion.identity);
        obj.GetComponent<FloatingText>().Set(text);
    }

    void DamageTextEmission(float damage) => Emit(ApplicationManager.prefabDatabase.floatingTextLife, "-" + damage);

    #region MonoBehaviour

    private void OnEnable()
    {
        switch (mode)
        {
            case Mode.damage:
                GetComponentInParent<Damageable>().DamageTaken += DamageTextEmission;
                break;
            case Mode.faithGained:
                Debug.LogWarning("TextsEmitter.Mode.faithGained NOT IMPLEMENTED!");
                break;
            case Mode.faithLost:
                Debug.LogWarning("TextsEmitter.Mode.faithLost NOT IMPLEMENTED!");
                break;
        }
    }

    private void OnDisable()
    {
        switch (mode)
        {
            case Mode.damage:
                GetComponentInParent<Damageable>().DamageTaken -= DamageTextEmission;
                break;
        }

    }


    #endregion
}
