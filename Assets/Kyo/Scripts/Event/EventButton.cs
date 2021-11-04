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
    [SerializeField] private bool isSelected = false;
    [SerializeField] private EventSO eventSO = null;

    public void InitEventInfo(EventSO Event)
    {
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

    // クリック時の処理
    public virtual void OnClick()
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

    public bool IsBase()
    {
        return eventSO.type == EventSOType.Base;
    }

    // Stateごとの処理
    // BaseSelect
    public void OnBaseSelect()
    {
        // Base基地の選択
        RouteManager.instance.SetBasePoint(this);

        // 縮小
        //DoScaleDown();

        image.color = Color.red;

        // 仮
        MainGameLogic logic = LogicManager.instance.GetSceneLogic<MainGameLogic>();
        logic.SetNextSate(MainGameState.RouteSelectPre);
    }

    // RouteSelect
    public void OnRouteSelect()
    {
        if (IsBase())
        {
            RouteManager.instance.SetRouteLoop();
        }
        else
        {
            if (!isSelected)
            {
                isSelected = true;
                RouteManager.instance.AddRoutePoint(this);
            }
            else
            {
                isSelected = false;
                RouteManager.instance.RemoveRoutePoint(this);
            }
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

        // BaseSelectの時、BaseのSizeを拡大する
        if (logic.isBaseSelect())
        {
            if (IsBase())
            {
                DoScaleUp();
            }
        }
        // RouteSelectの時、EventButtonのSizeを拡大する
        else if (logic.isRouteSelect())
        {
            if (!IsBase())
            {
                DoScaleUp();
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
            if (IsBase())
            {
                DoScaleDown();
            }
        }
        // RouteSelectの時、選択されなかった場合、EventButtonのSizeを縮小する
        else if (logic.isRouteSelect())
        {
            if (!IsBase() && !isSelected)
            {
                DoScaleDown();
            }
        }
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
}
