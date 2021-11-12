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

        // Stateごとの処理
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
        baseButtonUI.moveability.text = "移動: ";
        //baseButtonUI.ama.sprite = Base用のAMASprite辞書を用意する
        baseButtonUI.amaType.text = DictionaryManager.instance.GetAMAType(eventSO.amaType);
        baseButtonUI.got.text = "未獲得";
        if (Player.instance.CheckUnlockedAMAs(((BaseEventSO)eventSO).ama))
        {
            baseButtonUI.got.text = "獲得済み";
        }

        if (isSelected)
        {
            baseButtonUI.moveability.text += "選択中";
        }
        else
        {
            if (!IsCurrentBase())
            {
                DoScaleUp();
            }

            if (ImmovableDistance(RouteManager.instance.GetPreviousRoutePoint()) || !RouteManager.instance.RouteCouldBePlanned())
            {
                baseButtonUI.moveability.text += "不";
            }
            baseButtonUI.moveability.text += "可";
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

    // Stateごとの処理
    // RouteSelect
    public override void OnRouteSelect()
    {
        if (RouteManager.instance.RouteCouldBePlanned())
        {
            if (!isSelected)
            {
                if (FuelGauge.instance.NoMoreFuel()) return;
                if (ImmovableDistance(RouteManager.instance.GetPreviousRoutePoint())) return;

                SetSelected(true);
                RouteManager.instance.AddRoutePoint(this);
                baseButtonUI.moveability.text = "移動: 選択中";
            }
            else
            {
                SetSelected(false);
                RouteManager.instance.RemoveRoutePoints(this);
                baseButtonUI.moveability.text = "移動: 可";
            }
            RouteManager.instance.SetRoutePlanned(isSelected);
        }
    }

    // BaseSelect
    public void OnBaseSelect()
    {
        // Playerの初期位置
        Player.instance.SetFirstBase(this);
        Player.instance.AddAMA(((BaseEventSO)eventSO).ama);

        // PlayerのBaseをStartPointに登録
        RouteManager.instance.SetStartPoint();

        //FuelGauge.instance.ResetMaxValue();

        // 仮
        MainGameLogic logic = LogicManager.instance.GetSceneLogic<MainGameLogic>();
        logic.SetNextSate(MainGameState.RouteSelectPre);
    }

    // MouseがButtonの上に入る時
    public override void OnPointerEnter(PointerEventData E)
    {
        MainGameLogic logic = LogicManager.instance.GetSceneLogic<MainGameLogic>();

        // BaseSelectの時、BaseのSizeを拡大する
        if (logic.isBaseSelect())
        {
            DoScaleUp();
        }
        // RouteSelectの時、EventButtonのSizeを拡大する
        else if (logic.isRouteSelect())
        {
            ShowEventPreview();
        }
    }

    // MouseがButtonの上から離れる時
    public override void OnPointerExit(PointerEventData E)
    {
        MainGameLogic logic = LogicManager.instance.GetSceneLogic<MainGameLogic>();

        // BaseSelectの時、BaseのSizeを縮小する
        if (logic.isBaseSelect())
        {
            DoScaleDown();
        }
        // RouteSelectの時、選択されなかった場合、EventButtonのSizeを縮小する
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
