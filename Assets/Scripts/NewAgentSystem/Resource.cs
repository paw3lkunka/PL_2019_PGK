using System.Collections;
using System;
using UnityEngine;

/// <summary>
/// Stores float value clamped to [0, max]
/// </summary>
[Serializable]
public struct Resource
{
    /// <summary>
    /// Creates Resource object.
    /// </summary>
    /// <param name="current">Current value.</param>
    /// <param name="max">Maximum allowed value.</param>
    public Resource(float current, float max)
    {
        this.max = max;
        this.current = Mathf.Clamp(current, 0, max);
    }

    /// <summary>
    /// Implicit cast to float
    /// </summary>
    /// <param name="val">current value</param>
    public static implicit operator float(Resource val) => val.current;

    /// <summary>
    /// Sets value, autamaticlly clamps it to [0,max]
    /// </summary>
    /// <param name="value">value to set</param>
    /// <returns>real value (after clamp)</returns>
    public float Set(float value) => current = Mathf.Clamp(value, 0, max);

    /// <summary>
    /// Max value, allways is bigger or equal 0.W
    /// </summary>
    public float Max { get => max; set => max = Mathf.Max(value, 0); }

    [SerializeField]
    private float current;

    [SerializeField]
    private float max;

    #region Operators
    public static Resource operator + (Resource res, float f) => new Resource(f + res, res.Max);
    public static Resource operator - (Resource res, float f) => new Resource(f - res, res.Max);
    public static Resource operator * (Resource res, float f) => new Resource(f * res, res.Max);
    public static Resource operator / (Resource res, float f) => new Resource(f / res, res.Max);
    public static Resource operator % (Resource res, float f) => new Resource(f % res, res.Max);
    public static Resource operator ++ (Resource res) => new Resource(res.current + 1, res.Max);
    public static Resource operator -- (Resource res) => new Resource(res.current - 1, res.Max);
    public static bool operator == (Resource res, float f) => f == res;
    public static bool operator != (Resource res, float f) => f != res;
    public static bool operator == (Resource res1, Resource res2) => res1.current == res2.current;
    public static bool operator != (Resource res1, Resource res2) => res1.current != res2.current;
    public static bool operator > (Resource res, float f) => f < res;
    public static bool operator < (Resource res, float f) => f > res;
    public static bool operator >= (Resource res, float f) => f <= res;
    public static bool operator <= (Resource res, float f) => f >= res;
    public static bool operator > (Resource res1, Resource res2) => res1.current > res2.current;
    public static bool operator < (Resource res1, Resource res2) => res1.current < res2.current;
    public static bool operator >= (Resource res1, Resource res2) => res1.current >= res2.current;
    public static bool operator <= (Resource res1, Resource res2) => res1.current <= res2.current;
    #endregion

}
