using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SGUtils
{
    public static void SafeDestroy(Object obj)
    {
#      if UNITY_EDITOR
            if (Application.isEditor)
            {
                Object.DestroyImmediate(obj);
            }
            else
#      endif
            {
                Object.Destroy(obj);
            }
    }
}
