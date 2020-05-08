using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions3d
{
    public static (GameObject, float) NearestFrom3d(this List<GameObject> objs, Vector3 from)
    {
        GameObject target = null;
        float minimum = float.PositiveInfinity;

        foreach (GameObject enemy in objs)
        {
            if (!enemy)
            {
                continue;
            }

            float distance = Vector3.Distance(from, enemy.transform.position);
            if (distance < minimum)
            {
                target = enemy;
                minimum = distance;
            }
        }

        return (target, minimum);
    }
}