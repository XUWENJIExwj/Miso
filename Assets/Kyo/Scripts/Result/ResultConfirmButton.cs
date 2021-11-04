using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ResultConfirmButton : Button
{
    [SerializeField] private bool pointerEnter = false;

    // Mouse‚ªButton‚Ìã‚É“ü‚é
    public override void OnPointerEnter(PointerEventData E)
    {
        pointerEnter = true;
    }

    // Mouse‚ªButton‚Ìã‚©‚ç—£‚ê‚é
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
