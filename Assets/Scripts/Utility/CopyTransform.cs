using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyTransform : MonoBehaviour
{
#pragma warning disable
    [SerializeField] private Transform objectToFollow;
    [SerializeField] private string objectName;
    [SerializeField] private bool position;
    [SerializeField] private bool rotation;
    [SerializeField] private bool scale;
#pragma warning restore

    private void Start() 
    {
        if (objectToFollow == null)
        {
            objectToFollow = GameObject.Find(objectName).transform;
        }
    }

    void Update()
    {
        if (position)
        {
            transform.position = objectToFollow.transform.position;
        }

        if (rotation)
        {
            transform.rotation = objectToFollow.transform.rotation;
        }

        if (scale)
        {
            transform.localScale = objectToFollow.transform.localScale;
        }
    }
}
