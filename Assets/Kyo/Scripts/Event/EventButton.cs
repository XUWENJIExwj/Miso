using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using EventScriptableObject;

public class EventButton : Button
{
    [SerializeField] private bool isSelected = false;
    [SerializeField] private EventSO eventSO = null;

    public void InitEventInfo(EventSO Event)
    {
        if(Event)
        {
            eventSO = Event;
            image.sprite = Event.icon;

            // 仮
            if(IsBase())
            {
                RouteManager.instance.SetStartPoint(this);
            }
        }
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

    public virtual void OnClick()
    {
        if(IsBase())
        {
            RouteManager.instance.SetRouteLoop();
        }
        else
        {
            if (!isSelected)
            {
                RouteManager.instance.AddRoutePoint(this);
                isSelected = true;
            }
            else
            {
                RouteManager.instance.RemoveRoutePoint(this);
                isSelected = false;
            }
        }
    }

    public bool IsBase()
    {
        if(!eventSO)
        {
            return false;
        }
        else
        {
            return eventSO.type == EventButtonType.Base;
        }
    }

    public override void OnPointerDown(PointerEventData E)
    {
        MapScroll.instance.SetOnDrag(false);
    }

    public override void OnPointerUp(PointerEventData E)
    {
        MapScroll.instance.SetOnDrag(true);
    }
}
