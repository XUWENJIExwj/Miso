using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerButton : Button
{
    public void OnClick()
    {
        Player.instance.GetCurrentBase().OnClick();
    }

    // Button‚ğ‘€ì‚·‚é‚Ìˆ—
    // Button‚ğ‰Ÿ‚·
    public override void OnPointerDown(PointerEventData E)
    {
        // Map‚ÌˆÚ“®‚ğ•s‰Â‚É‚·‚é
        MapScroll.instance.SetOnDrag(false);
    }

    // Button‚ğ—£‚·
    public override void OnPointerUp(PointerEventData E)
    {
        // Map‚ÌˆÚ“®‚ğ‰Â‚É‚·‚é
        MapScroll.instance.SetOnDrag(true);
    }

    // Mouse‚ªButton‚Ìã‚É“ü‚é
    public override void OnPointerEnter(PointerEventData E)
    {
        MainGameLogic logic = LogicManager.instance.GetSceneLogic<MainGameLogic>();
        if (logic.isRouteSelect())
        {
            Player.instance.GetCurrentBase().ShowEventPreview();
        }
    }

    // Mouse‚ªButton‚Ìã‚©‚ç—£‚ê‚é
    public override void OnPointerExit(PointerEventData E)
    {
        MainGameLogic logic = LogicManager.instance.GetSceneLogic<MainGameLogic>();
        if (logic.isRouteSelect())
        {
            Player.instance.GetCurrentBase().EndEventPreview();
        }
    }
}
