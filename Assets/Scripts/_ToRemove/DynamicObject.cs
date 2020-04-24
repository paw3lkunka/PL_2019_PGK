using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicObject : MonoBehaviour
{
    [field: SerializeField]
    public int ID { get; private set; }

#if UNITY_EDITOR
    public void SetId(int value) => ID = value;
#endif

    private void OnEnable()
    {
        if (ApplicationManager.Instance.CurrentLocationsDestroyedDynamicObjects?.Contains(ID) ?? false)
        {
            Destroy(gameObject);
        }
    }

    public void RememberAsDestroyed()
    {
        HashSet<int> value = null;
        try
        {
            value = ApplicationManager.Instance.destroyedDynamicObjects[ApplicationManager.Instance.currentLocation];
        }
        catch (KeyNotFoundException)
        {
            ApplicationManager.Instance.destroyedDynamicObjects.Add(ApplicationManager.Instance.currentLocation, new HashSet<int>());
            value = ApplicationManager.Instance.destroyedDynamicObjects[ApplicationManager.Instance.currentLocation];
        }
        catch (ArgumentNullException)
        {
            //will be destroyed after this function
            value = new HashSet<int>();
        }
        finally
        {
            value.Add(ID);
        }
    }
    public void DestroyAndRemember()
    {
        RememberAsDestroyed();
        Destroy(gameObject);
    }

}
