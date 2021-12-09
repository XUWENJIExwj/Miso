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

    public override void Init(Oceans Ocean, OceanAreas OceanArea)
    {
        base.Init(Ocean, OceanArea);
        size = new Vector2(63.375f, 71.625f);
    }

    public override void SetPollutionLevel()
    {
        
    }

    public override void CreateBaseEvent()
    {
        eventSO = GlobalInfo.instance.CreateEventSO(baseIndex);

        RectTransform rectTransform = GetComponent<RectTransform>();
        maxSize.x = Mathf.Max(maxSize.x, rectTransform.sizeDelta.x);
        maxSize.y = Mathf.Max(maxSize.y, rectTransform.sizeDelta.y);
    }

    public override void CreateEvent()
    {
        
    }

    public override void ResetEvent()
    {
        
    }

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
        BaseEventPreview eventPreview = EventButtonManager.instance.GetBaseEventPreview();
        eventPreview.gameObject.SetActive(true);
        eventPreview.ResetPreview();
        eventPreview.SetPosition(transform.localPosition);
        eventPreview.SetEventDesc(eventSO.eventTitle);
        eventPreview.SetMoveability("移動: ");
        //eventPreview.SetAMASprite(Sprite AMA) Base用のAMASprite辞書を用意する
        eventPreview.SetAMAType(DictionaryManager.instance.GetAMAType(eventSO.amaType));
        eventPreview.SetGot("未獲得");
        if (Player.instance.CheckUnlockedAMAs(((BaseEventSO)eventSO).ama))
        {
            eventPreview.SetGot("獲得済み");
        }

        if (isSelected)
        {
            eventPreview.AddMoveability("選択中");
        }
        else
        {
            if (!IsCurrentBase())
            {
                DoScaleUp();
            }

            if (FuelGauge.instance.NoMoreFuel())
            {
                eventPreview.AddMoveability("燃料不足");
            }
            else
            {
                if (ImmovableDistance(RouteManager.instance.GetPreviousRoutePoint()) || !RouteManager.instance.RouteCouldBePlanned())
                {
                    eventPreview.AddMoveability("不");
                }
                eventPreview.AddMoveability("可");
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

    // Stateごとの処理
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
                eventPreview.SetMoveability("移動: 選択中");
            }
            else
            {
                SetSelected(false);
                RouteManager.instance.RemoveRoutePoints(this);
                eventPreview.SetMoveability("移動: 可");
            }
            RouteManager.instance.SetRoutePlanned(isSelected);
        }
    }

    // BaseSelect
    public void OnBaseSelect()
    {
        BaseConfirmView.instance.ActiveConfirmView(this);
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
            if (Player.instance.GetCurrentBase() != this)
            {
                ShowEventPreview();
            }
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
            if (Player.instance.GetCurrentBase() != this)
            {
                EndEventPreview();
            }
        }
    }

    public AMAs GetAMA()
    {
        return ((BaseEventSO)eventSO).ama;
    }
}
