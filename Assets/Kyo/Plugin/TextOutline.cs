using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextOutline : MonoBehaviour
{
    [SerializeField] private Text text = null;
    [SerializeField] private CircleOutline outline = null;

    public void SetText(string T)
    {
        text.text = T;
    }

    public void AddChar(char T)
    {
        text.text += T;
    }

    public void SetAlpha(float Alpha)
    {
        SetTextAlpha(Alpha);
        SetOutlineAlpha(Alpha);
    }

    public void SetTextColor(Color C)
    {
        text.color = C;
    }

    public void SetTextAlpha(float Alpha)
    {
        text.color = HelperFunction.ChangeAlpha(text.color, Alpha);
    }

    public void SetOutlineColor(Color C)
    {
        outline.SetEffectColor(C);
    }

    public void SetOutlineAlpha(float Alpha)
    {
        outline.SetEffectAlpha(Alpha);
    }
}
