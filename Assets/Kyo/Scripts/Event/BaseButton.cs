using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventScriptableObject;
using UnityEngine.EventSystems;
using System;
using TMPro;
using UnityEngine.UI;

[Serializable]
public struct BaseButtonUIElement
{
    public Image frame;
    public TMP_Text eventDesc;
    public TMP_Text moveability;
    public Image ama;
    public TMP_Text amaType;
    public TMP_Text got;
}

public class BaseButton : EventButton
{
    [SerializeField] private BaseButtonUIElement baseButtonUI;

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
        baseButtonUI.eventDesc.text = eventSO.eventTitle;
        baseButtonUI.moveability.text = "ˆÚ“®: ";
        //baseButtonUI.ama.sprite = Base—p‚ÌAMASprite«‘‚ğ—pˆÓ‚·‚é
        baseButtonUI.amaType.text = DictionaryManager.instance.GetAMAType(eventSO.amaType);
        baseButtonUI.got.text = "–¢Šl“¾";
        if (Player.instance.CheckUnlockedAMAs(((BaseEventSO)eventSO).ama))
        {
            baseButtonUI.got.text = "Šl“¾Ï‚İ";
        }

        if (isSelected)
        {
            baseButtonUI.moveability.text += "‘I‘ğ’†";
        }
        else
        {
            if (!IsCurrentBase())
            {
                DoScaleUp();
            }

            if (CheckDistance(RouteManager.instance.GetPreviousRoutePoint()) || !RouteManager.instance.RouteCouldBePlanned())
            {
                baseButtonUI.moveability.text += "•s";
            }
            baseButtonUI.moveability.text += "‰Â";
        }

        baseButtonUI.frame.gameObject.SetActive(true);
    }

    public override void EndEventPreview()
    {
        if (!isSelected && !IsCurrentBase())
        {
            DoScaleDown();
        }
        baseButtonUI.frame.gameObject.SetActive(false);
    }

    // State‚²‚Æ‚Ìˆ—
    // RouteSelect
    public override void OnRouteSelect()
    {
        if (RouteManager.instance.RouteCouldBePlanned())
        {
            if (!isSelected)
            {
                if (CheckDistance(RouteManager.instance.GetPreviousRoutePoint())) return;

                SetSelected(true);
                RouteManager.instance.AddRoutePoint(this);
                baseButtonUI.moveability.text = "ˆÚ“®: ‘I‘ğ’†";
            }
            else
            {
                SetSelected(false);
                RouteManager.instance.RemoveRoutePoints(this);
                baseButtonUI.moveability.text = "ˆÚ“®: ‰Â";
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

    public new BaseButtonUIElement GetEventButtonUIElement()
    {
        return baseButtonUI;
    }
}
