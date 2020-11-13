using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CultistEntityInfo
{
    public CultistEntityInfo(GameObject prefab)
    {
        Prefab = prefab;
        Save(prefab.GetComponent<Cultist>());
    }
    public void Save(Cultist cultist)
    {
        hp = cultist.GetComponent<Damageable>().Health;
    }

    public void Apply(Cultist cultist)
    {
        cultist.info = this;
        cultist.GetComponent<Damageable>().SetHealthForce(hp);
    }

    public void Instantiate() => Apply(Object.Instantiate(Prefab).GetComponent<Cultist>());
    public void Instantiate(Vector3 position, Quaternion rotation) => Apply(Object.Instantiate(Prefab, position, rotation).GetComponent<Cultist>());
    public void Instantiate(Vector3 position, Quaternion rotation, Transform parent) => Apply(Object.Instantiate(Prefab, position, rotation, parent).GetComponent<Cultist>());
    public void Instantiate(Transform parent) => Apply(Object.Instantiate(Prefab, parent).GetComponent<Cultist>());
    public void Instantiate(Transform parent, bool worldPositionStays) => Apply(Object.Instantiate(Prefab, parent, worldPositionStays).GetComponent<Cultist>());

    public GameObject Prefab { get; private set; }

    // -- [ Saved props ] ----------------------

    public float hp { get; private set; } 

}
