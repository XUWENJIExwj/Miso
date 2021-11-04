using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ResultConfirmButton : Button
{
    [SerializeField] private bool pointerEnter = false;

    // Mouse��Button�̏�ɓ��鎞
    public override void OnPointerEnter(PointerEventData E)
    {
        pointerEnter = true;
    }

    // Mouse��Button�̏ォ�痣��鎞
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
