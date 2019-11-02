using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Character : MonoBehaviour
{
    public int hp;
    [Range(0,20)]
    public int defence;
    

    // Update is called once per frame
    protected virtual void Update()
    {
        if(hp<=0)
        {
            Destroy(gameObject);
        }
    }

    public void takeDamage(int damage)
    {
        hp -= Mathf.Max( damage / (defence+1) , 0);

    }


}
