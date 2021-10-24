using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperFunction : MonoBehaviour
{
    static public Color ChangeAlpha(Color RefColor, float TargetAlpha)
    {
        return new Color(RefColor.r, RefColor.g, RefColor.b, TargetAlpha);
    }
}
