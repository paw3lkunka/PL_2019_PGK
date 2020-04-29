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

    [SerializeField]
    private Mode mode;

    public void Emit(GameObject prefab,string text)
    {
        GameObject obj = Instantiate(prefab, transform.position, Quaternion.identity);
        obj.GetComponent<FloatingText>().Set(text);
    }

    void DamageTextEmission(float damage) => Emit(ApplicationManager.Instance.prefabDatabase.floatingTextLife, "-" + damage);

    #region MonoBehaviour

    private void OnEnable()
    {
        switch(mode)
        {
            case Mode.damage:
                GetComponentInParent<Damageable>().DamageTaken += DamageTextEmission;
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
