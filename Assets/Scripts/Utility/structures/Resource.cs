using System.Collections;
using System;
using UnityEngine;

/// <summary>
/// Stores float value clamped to [0, max]
/// </summary>
[Serializable]
public class Resource
{
    /// <summary>
    /// Creates Resource object.
    /// </summary>
    /// <param name="current">Current value.</param>
    /// <param name="max">Maximum allowed value.</param>
    public Resource(float current, float max, bool overflowable)
    {
        this.max = max;
        Overflowable = overflowable;
        Set(current);
        InitialValue = this.current;
    }

    /// <summary>
    /// Implicit cast to float
    /// </summary>
    /// <param name="val">current value</param>
    public static implicit operator float(Resource val) => val.current;

    /// <summary>
    /// Sets value, automatically clamps it to [0,max]
    /// </summary>
    /// <param name="value">value to set</param>
    /// <returns>real value (after clamp)</returns>
    public virtual float Set(float value) => current = Mathf.Clamp(value, 0, Overflowable ? float.PositiveInfinity : max);

    /// <summary>
    /// Max value, always is bigger or equal maximum.
    /// </summary>
    public float Max { get => max; set => max = Mathf.Max(value, 0); }

    /// <summary>
    /// Returns value divided by maximum.
    /// </summary>
    public float Normalized { get => current / max; }

    public float InitialValue { get; private set; }

    [SerializeField]
    private float current;

    [SerializeField]
    private float max;

    [SerializeField]
    private bool overflowable;

    public bool Overflowable 
    { 
        get => overflowable;
        set { overflowable = value; Set(current); }
    }

    public bool IsOverflowed { get => current > max; }

    #region Operators
    public static Resource operator + (Resource res, float f) => new Resource(f + res, res.Max, res.Overflowable);
    public static Resource operator - (Resource res, float f) => new Resource(res.current - f, res.Max, res.Overflowable);
    public static Resource operator * (Resource res, float f) => new Resource(f * res, res.Max, res.Overflowable);
    public static Resource operator / (Resource res, float f) => new Resource(res.current / f, res.Max, res.Overflowable);
    public static Resource operator % (Resource res, float f) => new Resource(res.current % f, res.Max, res.Overflowable);
    public static Resource operator ++ (Resource res) => new Resource(res.current + 1, res.Max, res.Overflowable);
    public static Resource operator -- (Resource res) => new Resource(res.current - 1, res.Max, res.Overflowable);
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

    public override bool Equals(object obj)
    {
        try
        {
            Resource that = (Resource)obj;
            return this.current == that.current && this.max == that.max;
        }
        catch
        {
            return false;
        }
    }

    public override int GetHashCode()
    {
        int result = 2137;
        result = 997 * result + current.GetHashCode();
        result = 997 * result + max.GetHashCode();
        result = 997 * result + (Overflowable ? 0 : 1);
        return result;
    }

    public override string ToString() => $"{current}[0,{max}]";
}
