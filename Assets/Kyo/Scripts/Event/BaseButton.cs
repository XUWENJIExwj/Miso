using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventScriptableObject;
using UnityEngine.EventSystems;
using System;
using TMPro;
using UnityEngine.UI;

public class BaseButton : EventButton
{
    [SerializeField] private BaseIndex baseIndex = BaseIndex.None;

    public override void CreateEvent()
    {
        eventSO = GlobalInfo.instance.CreateEventSO(baseIndex);
    }

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

    public override void ShowEventPreview()
    {
        BaseEventPreview eventPreview = EventButtonManager.instance.GetBaseEventPreview();
        eventPreview.gameObject.SetActive(true);
        eventPreview.ResetPreview();
        eventPreview.SetPosition(transform.localPosition);
        eventPreview.SetEventDesc(eventSO.eventTitle);
        eventPreview.SetMoveability("ˆÚ“®: ");
        //eventPreview.SetAMASprite(Sprite AMA) Base—p‚ÌAMASprite«‘‚ğ—pˆÓ‚·‚é
        eventPreview.SetAMAType(DictionaryManager.instance.GetAMAType(eventSO.amaType));
        eventPreview.SetGot("–¢Šl“¾");
        if (Player.instance.CheckUnlockedAMAs(((BaseEventSO)eventSO).ama))
        {
            eventPreview.SetGot("Šl“¾Ï‚İ");
        }

        if (isSelected)
        {
            eventPreview.AddMoveability("‘I‘ğ’†");
        }
        else
        {
            if (!IsCurrentBase())
            {
                DoScaleUp();
            }

            if (FuelGauge.instance.NoMoreFuel())
            {
                eventPreview.AddMoveability("”R—¿•s‘«");
            }
            else
            {
                if (ImmovableDistance(RouteManager.instance.GetPreviousRoutePoint()) || !RouteManager.instance.RouteCouldBePlanned())
                {
                    eventPreview.AddMoveability("•s");
                }
                eventPreview.AddMoveability("‰Â");
            }
        }
    }

    public override void EndEventPreview()
    {
        if (!isSelected && !IsCurrentBase())
        {
            DoScaleDown();
        }
        BaseEventPreview eventPreview = EventButtonManager.instance.GetBaseEventPreview();
        eventPreview.gameObject.SetActive(false);
    }

    // State‚²‚Æ‚Ìˆ—
    // RouteSelect
    public override void OnRouteSelect()
    {
        BaseEventPreview eventPreview = EventButtonManager.instance.GetBaseEventPreview();
        if (RouteManager.instance.RouteCouldBePlanned())
        {
            if (!isSelected)
            {
                if (FuelGauge.instance.NoMoreFuel()) return;
                if (ImmovableDistance(RouteManager.instance.GetPreviousRoutePoint())) return;

                SetSelected(true);
                RouteManager.instance.AddRoutePoint(this);
                eventPreview.SetMoveability("ˆÚ“®: ‘I‘ğ’†");
            }
            else
            {
                SetSelected(false);
                RouteManager.instance.RemoveRoutePoints(this);
                eventPreview.SetMoveability("ˆÚ“®: ‰Â");
            }
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

        //FuelGauge.instance.ResetMaxValue();

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
        else if (logic.isRouteSelect())
        {
            ShowEventPreview();
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
        else if (logic.isRouteSelect())
        {
            EndEventPreview();
        }
    }

    public AMAs GetAMA()
    {
        return ((BaseEventSO)eventSO).ama;
    }
}
