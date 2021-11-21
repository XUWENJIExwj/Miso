using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public struct PointRange
{
    public int min;
    public int max;
}

public class HelperFunction : MonoBehaviour
{
    // Return RefColor and TargetAlpha
    static public Color ChangeAlpha(Color RefColor, float TargetAlpha)
    {
        return new Color(RefColor.r, RefColor.g, RefColor.b, TargetAlpha);
    }

    // Return [PointRange.min, PointRange.max]
    static public int RandomPointRange(PointRange Range)
    {
        return UnityEngine.Random.Range(Range.min, Range.max + 1);
    }

    static public Vector2 CeilVector2(Vector2 V)
    {
        Vector2 v;
        v.x = Mathf.Ceil(V.x);
        v.y = Mathf.Ceil(V.y);
        return v;
    }

    // Return the Square of the Length of Vector2
    static public float Length2(Vector2 V)
    {
        return V.x * V.x + V.y * V.y;
    }

    // Return the Square of the Length of Vector3
    static public float Length2(Vector3 V)
    {
        return V.x * V.x + V.y * V.y + V.z * V.z;
    }

    // Return the Square of the Distance of the Vector3 between A and B
    static public float Distance2(Vector3 VA,Vector3 VB)
    {
        Vector3 interval = VA - VB;
        return Length2(interval);
    }
}
