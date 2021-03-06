using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ResultConfirmButton : Button
{
    [SerializeField] private bool pointerEnter = false;

    // MouseがButtonの上に入る時
    public override void OnPointerEnter(PointerEventData E)
    {
        pointerEnter = true;
    }

    // MouseがButtonの上から離れる時
    public override void OnPointerExit(PointerEventData E)
    {
        pointerEnter = false;
    }

    public bool PointerEnter()
    {
        return pointerEnter;
    }

    protected override void OnDisable()
    {
        pointerEnter = false;
    }
}
