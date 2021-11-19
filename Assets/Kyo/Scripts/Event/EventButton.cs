using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using EventScriptableObject;
using TMPro;
using DG.Tweening;

public class EventButton : Button
{
    [SerializeField] protected Vector2Int gridPos = Vector2Int.zero;
    [SerializeField] protected bool isSelected = false;
    [SerializeField] protected EventSO eventSO = null;
    [SerializeField] protected Oceans ocean = Oceans.None;
    [SerializeField] protected OceanAreas oceanArea = OceanAreas.None;
    static protected Vector2 size = new Vector2(30.0f, 30.0f);
    static Vector2 maxSize = Vector2.zero;

    public void Init(Oceans Ocean, OceanAreas OceanArea)
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        maxSize.x = Mathf.Max(maxSize.x, rectTransform.sizeDelta.x);
        maxSize.y = Mathf.Max(maxSize.y, rectTransform.sizeDelta.y);

        SetOceanInfo(Ocean, OceanArea);
        CreateEvent();
    }

    static public Vector2 GetMaxSize()
    {
        return maxSize;
    }

    public void SetOceanInfo(Oceans Ocean, OceanAreas OceanArea)
    {
        ocean = Ocean;
        oceanArea = OceanArea;
    }

    public virtual void CreateEvent()
    {
        eventSO = GlobalInfo.instance.CreateEventSO();
        image.sprite = eventSO.icon;
        gameObject.SetActive(false);
    }

    public void Move(Vector2 Offset)
    {
        transform.localPosition += new Vector3(Offset.x, Offset.y, 0.0f);

        // 画面外になったら、ループさせる
        FixPostion();
    }

    // 画面外になったら、ループさせる
    public void FixPostion()
    {
        Vector2 fixPos = transform.localPosition;

        // X軸
        if (transform.localPosition.x < -GlobalInfo.instance.halfMapSize.x)
        {
            fixPos.x = transform.localPosition.x + GlobalInfo.instance.mapSize.x;
        }
        else if (transform.localPosition.x > GlobalInfo.instance.halfMapSize.x)
        {
            fixPos.x = transform.localPosition.x - GlobalInfo.instance.mapSize.x;
        }

        transform.localPosition = fixPos;
    }

    // 拡大
    public void DoScaleUp(float MaxScale = 1.5f, float Time = 0.5f)
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.DOSizeDelta(size * MaxScale, Time);
    }

    // 縮小
    public void DoScaleDown(float MinScale = 1.0f, float Time = 0.5f)
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.DOSizeDelta(size * MinScale, Time);
    }

    public T GetEventSO<T>()
    {
        if (eventSO.GetType() == typeof(T))
        {
            return (T)(object)eventSO;
        }

        Debug.Log("取得しようとする型と違う!");
        return (T)(object)null;
    }

    public EventSOType GetEventType()
    {
        return eventSO.GetEventType();
    }

    // クリック時の処理
    public virtual void OnClick()
    {
        MainGameLogic logic = LogicManager.instance.GetSceneLogic<MainGameLogic>();

        if (logic.isRouteSelect())
        {
            OnRouteSelect();
        }
    }

    public bool IsBase()
    {
        return eventSO.type == EventSOType.Base;
    }

    public bool IsCurrentBase()
    {
        return Player.instance.GetCurrentBase() == this;
    }

    public void SetEventButtonColor(Color ButtonColor)
    {
        image.color = ButtonColor;
    }

    public void SetSelected(bool Selected)
    {
        isSelected = Selected;
    }

    public bool GetSelected()
    {
        return isSelected;
    }

    public Vector2Int GetGridPos()
    {
        return gridPos;
    }

    public bool ImmovableDistance(EventButton Event)
    {
        Vector2Int distance = gridPos - Event.GetGridPos();
        return Mathf.Abs(distance.x) > 1 || Mathf.Abs(distance.y) > 1;
    }

    public virtual void ShowEventPreview()
    {
        EventPreview eventPreview = EventButtonManager.instance.GetEventPreview();
        eventPreview.gameObject.SetActive(true);
        eventPreview.ResetPreview();
        eventPreview.SetPosition(transform.localPosition);
        eventPreview.SetMoveability("移動: ");

        if (eventSO.IsRandomEvent())
        {
            eventPreview.FixPosition();
            eventPreview.SetEventDesc("何か起こるかも");
            eventPreview.SetAMATypeActive(false);
            eventPreview.SetPointRangeActive(false);
        }
        else
        {
            eventPreview.SetEventDesc(eventSO.eventTitle);
            eventPreview.SetAMAType("有利タイプ: " + DictionaryManager.instance.GetAMAType(eventSO.amaType));
            eventPreview.SetAMATypeActive(true);
            eventSO.MakePointRange();
            eventPreview.SetPointRange("Point: " + eventSO.pointRange.min.ToString() + "～" + eventSO.pointRange.max.ToString());
            eventPreview.SetPointRangeActive(true);
        }

        if (isSelected)
        {
            eventPreview.AddMoveability("選択中");
        }
        else
        {
            DoScaleUp();

            if (FuelGauge.instance.NoMoreFuel())
            {
                eventPreview.AddMoveability("燃料不足");
            }
            else
            {
                if (ImmovableDistance(RouteManager.instance.GetPreviousRoutePoint()) || RouteManager.instance.RoutePlanned())
                {
                    eventPreview.AddMoveability("不");
                }
                eventPreview.AddMoveability("可");
            }
        }
    }

    public virtual void EndEventPreview()
    {
        if (!isSelected)
        {
            DoScaleDown();
        }
        EventPreview eventPreview = EventButtonManager.instance.GetEventPreview();
        eventPreview.gameObject.SetActive(false);
    }

    // Stateごとの処理
    // RouteSelect
    public virtual void OnRouteSelect()
    {
        EventPreview eventPreview = EventButtonManager.instance.GetEventPreview();
        if (!isSelected)
        {
            if (FuelGauge.instance.NoMoreFuel()) return;
            if (ImmovableDistance(RouteManager.instance.GetPreviousRoutePoint()) || RouteManager.instance.RoutePlanned()) return;

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
    }

    // Buttonを操作する時の処理
    // Buttonを押す時
    public override void OnPointerDown(PointerEventData E)
    {
        // Mapの移動を不可にする
        MapScroll.instance.SetOnDrag(false);
    }

    // Buttonを離す時
    public override void OnPointerUp(PointerEventData E)
    {
        // Mapの移動を可にする
        MapScroll.instance.SetOnDrag(true);
    }

    // MouseがButtonの上に入る時
    public override void OnPointerEnter(PointerEventData E)
    {
        MainGameLogic logic = LogicManager.instance.GetSceneLogic<MainGameLogic>();
        if (logic.isRouteSelect())
        {
            ShowEventPreview();
        }
    }

    // MouseがButtonの上から離れる時
    public override void OnPointerExit(PointerEventData E)
    {
        MainGameLogic logic = LogicManager.instance.GetSceneLogic<MainGameLogic>();
        if (logic.isRouteSelect())
        {
            EndEventPreview();
        }
    }
}