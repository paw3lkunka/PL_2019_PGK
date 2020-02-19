using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Util
{
    public static bool IsRealNull(this UnityEngine.Object aObj)
    {
        return (object)aObj == null;
    }
}
