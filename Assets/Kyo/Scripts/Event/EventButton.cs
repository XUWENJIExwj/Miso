using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using EventScriptableObject;
using UnityEditor;
using DG.Tweening;

public class EventButton : Button
{
    [SerializeField] protected Vector2Int gridPos = Vector2Int.zero;
    [SerializeField] protected bool isSelected = false;
    [SerializeField] protected EventSO eventSO = null;

    public void Init(EventSO Event, int X, int Y)
    {
        gridPos.x = X;
        gridPos.y = Y;
        eventSO = Event;
        image.sprite = Event.icon;
        gameObject.SetActive(IsBase());
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

        // Y軸
        if (transform.localPosition.y < -GlobalInfo.instance.halfMapSize.y)
        {
            fixPos.y = transform.localPosition.y + GlobalInfo.instance.mapSize.y;
        }
        else if (transform.localPosition.y > GlobalInfo.instance.halfMapSize.y)
        {
            fixPos.y = transform.localPosition.y - GlobalInfo.instance.mapSize.y;
        }

        transform.localPosition = fixPos;
    }


    // 拡大
    public void DoScaleUp(float MaxScale = 1.5f, float Time = 0.5f)
    {
        transform.DOScale(MaxScale, Time);
    }

    // 縮小
    public void DoScaleDown(float MinScale = 1.0f, float Time = 0.5f)
    {
        transform.DOScale(MinScale, Time);
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
        OnRouteSelect();
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

    // Stateごとの処理
    // RouteSelect
    public virtual void OnRouteSelect()
    {
        if (!isSelected)
        {
            SetSelected(true);
            RouteManager.instance.AddRoutePoint(this);
        }
        else
        {
            SetSelected(false);
            RouteManager.instance.RemoveRoutePoint(this);
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
        if (logic.isRouteSelect() && !isSelected)
        {
            DoScaleUp();
        }
    }

    // MouseがButtonの上から離れる時
    public override void OnPointerExit(PointerEventData E)
    {
        MainGameLogic logic = LogicManager.instance.GetSceneLogic<MainGameLogic>();
        if (logic.isRouteSelect() && !isSelected)
        {
            DoScaleDown();
        }
    }
}