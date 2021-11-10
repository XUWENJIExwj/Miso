using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventScriptableObject;
using UnityEngine.EventSystems;

public class BaseButton : EventButton
{
    public override void OnClick()
    {
        MainGameLogic logic = LogicManager.instance.GetSceneLogic<MainGameLogic>();

        // State‚²‚Æ‚Ìˆ—
        if (logic.isBaseSelect())
        {
            OnBaseSelect();
        }
        else if (logic.isRouteSelect())
        {
            OnRouteSelect();
        }
    }

    // State‚²‚Æ‚Ìˆ—
    // RouteSelect
    public override void OnRouteSelect()
    {
        if (RouteManager.instance.RouteCouldBePlanned())
        {
            base.OnRouteSelect();
            RouteManager.instance.SetRoutePlanned(isSelected);
        }
    }

    // BaseSelect
    public void OnBaseSelect()
    {
        // Player‚Ì‰ŠúˆÊ’u
        Player.instance.SetFirstBase(this);
        Player.instance.AddAMA(((BaseEventSO)eventSO).ama);

        // Player‚ÌBase‚ğStartPoint‚É“o˜^
        RouteManager.instance.SetStartPoint();

        // ‰¼
        MainGameLogic logic = LogicManager.instance.GetSceneLogic<MainGameLogic>();
        logic.SetNextSate(MainGameState.RouteSelectPre);
    }

    // Mouse‚ªButton‚Ìã‚É“ü‚é
    public override void OnPointerEnter(PointerEventData E)
    {
        MainGameLogic logic = LogicManager.instance.GetSceneLogic<MainGameLogic>();

        // BaseSelect‚ÌABase‚ÌSize‚ğŠg‘å‚·‚é
        if (logic.isBaseSelect())
        {
            DoScaleUp();
        }
        // RouteSelect‚ÌAEventButton‚ÌSize‚ğŠg‘å‚·‚é
        else if (logic.isRouteSelect() && !isSelected)
        {
            if (!IsCurrentBase())
            {
                DoScaleUp();
            }
        }
    }

    // Mouse‚ªButton‚Ìã‚©‚ç—£‚ê‚é
    public override void OnPointerExit(PointerEventData E)
    {
        MainGameLogic logic = LogicManager.instance.GetSceneLogic<MainGameLogic>();

        // BaseSelect‚ÌABase‚ÌSize‚ğk¬‚·‚é
        if (logic.isBaseSelect())
        {
            DoScaleDown();
        }
        // RouteSelect‚ÌA‘I‘ğ‚³‚ê‚È‚©‚Á‚½ê‡AEventButton‚ÌSize‚ğk¬‚·‚é
        else if (logic.isRouteSelect() && !isSelected)
        {
            if (!IsCurrentBase())
            {
                DoScaleDown();
            }
        }
    }

    public AMAs GetAMA()
    {
        return ((BaseEventSO)eventSO).ama;
    }
}
