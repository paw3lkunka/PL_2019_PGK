using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
#pragma warning disable
    [SerializeField]
    private float eyesightRange;
    [SerializeField]
    private float shootingRange;
    [SerializeField]
    private float chaseRange;
#pragma warning restore

    private GameObject chasedObject;

    private void Start()
    {
        
    }

    protected void Update()
    {
        var nearestDistance = GameManager.Instance.ourCrew.GetDistanceFromNearest(this.transform.position);
        if(!nearestDistance.Key)
        {
            return ;
        }

        if(!chasedObject)
        {
            if(nearestDistance.Value < this.eyesightRange)
            {
                chasedObject = nearestDistance.Key;
            }
        }
        else
        {
            chasedObject = nearestDistance.Key;
            
            if(IsShooting())
            {
                // TODO: How enemy shoots
            }

            if(IsChasing())
            {
                // TODO: How enemy chase
            }

            chasedObject = null;
        }
    }

    private bool IsShooting() => chasedObject ? Vector2.Distance(chasedObject.transform.position, this.transform.position) <= this.shootingRange : false;

    private bool IsChasing() => chasedObject ? Vector2.Distance(chasedObject.transform.position, this.transform.position) > this.chaseRange : false;
}
