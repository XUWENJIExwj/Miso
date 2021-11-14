using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using EventScriptableObject;
using TMPro;
using DG.Tweening;
using System;

[Serializable]
public struct EventButtonUIElement
{
    public Image frame;
    public TMP_Text eventDesc;
    public TMP_Text moveability;
    public TMP_Text amaType;
    public TMP_Text pointRange;
}

public class EventButton : Button
{
    [SerializeField] protected Vector2Int gridPos = Vector2Int.zero;
    [SerializeField] protected bool isSelected = false;
    [SerializeField] protected EventSO eventSO = null;
    [SerializeField] private EventButtonUIElement eventButtonUI;
    static protected Vector2 size = new Vector2(30.0f, 30.0f);

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

        // ��ʊO�ɂȂ�����A���[�v������
        FixPostion();
    }

    // ��ʊO�ɂȂ�����A���[�v������
    public void FixPostion()
    {
        Vector2 fixPos = transform.localPosition;

        // X��
        if (transform.localPosition.x < -GlobalInfo.instance.halfMapSize.x)
        {
            fixPos.x = transform.localPosition.x + GlobalInfo.instance.mapSize.x;
        }
        else if (transform.localPosition.x > GlobalInfo.instance.halfMapSize.x)
        {
            fixPos.x = transform.localPosition.x - GlobalInfo.instance.mapSize.x;
        }

        // Y��
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

    // �g��
    public void DoScaleUp(float MaxScale = 1.5f, float Time = 0.5f)
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.DOSizeDelta(size * MaxScale, Time);
    }

    // �k��
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

        Debug.Log("�擾���悤�Ƃ���^�ƈႤ!");
        return (T)(object)null;
    }

    public EventSOType GetEventType()
    {
        return eventSO.GetEventType();
    }

    // �N���b�N���̏���
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
        eventButtonUI.moveability.text = "�ړ�: ";

        if (eventSO.IsRandomEvent())
        {
            eventButtonUI.eventDesc.text = "�����N���邩��";
            eventButtonUI.amaType.gameObject.SetActive(false);
            eventButtonUI.pointRange.gameObject.SetActive(false);
        }
        else
        {
            eventButtonUI.eventDesc.text = eventSO.eventTitle;
            eventButtonUI.amaType.text = "�L���^�C�v: " + DictionaryManager.instance.GetAMAType(eventSO.amaType);
            eventButtonUI.amaType.gameObject.SetActive(true);
            eventSO.MakePointRange();
            eventButtonUI.pointRange.text = "Point: " + eventSO.pointRange.min.ToString() + "�`" + eventSO.pointRange.max.ToString();
            eventButtonUI.pointRange.gameObject.SetActive(true);
        }

        if (isSelected)
        {
            eventButtonUI.moveability.text += "�I��";
        }
        else
        {
            DoScaleUp();

            if (FuelGauge.instance.NoMoreFuel())
            {
                eventButtonUI.moveability.text += "�R���s��";
            }
            else
            {
                if (ImmovableDistance(RouteManager.instance.GetPreviousRoutePoint()) || RouteManager.instance.RoutePlanned())
                {
                    eventButtonUI.moveability.text += "�s";
                }
                eventButtonUI.moveability.text += "��";
            }
        }
        eventButtonUI.frame.gameObject.SetActive(true);
    }

    public virtual void EndEventPreview()
    {
        if (!isSelected)
        {
            DoScaleDown();
        }
        eventButtonUI.frame.gameObject.SetActive(false);
    }

    // State���Ƃ̏���
    // RouteSelect
    public virtual void OnRouteSelect()
    {
        if (!isSelected)
        {
            if (FuelGauge.instance.NoMoreFuel()) return;
            if (ImmovableDistance(RouteManager.instance.GetPreviousRoutePoint()) || RouteManager.instance.RoutePlanned()) return;

            SetSelected(true);
            RouteManager.instance.AddRoutePoint(this);
            eventButtonUI.moveability.text = "�ړ�: �I��";
        }
        else
        {
            SetSelected(false);
            RouteManager.instance.RemoveRoutePoints(this);
            eventButtonUI.moveability.text = "�ړ�: ��";
        }
    }

    // Button�𑀍삷�鎞�̏���
    // Button��������
    public override void OnPointerDown(PointerEventData E)
    {
        // Map�̈ړ���s�ɂ���
        MapScroll.instance.SetOnDrag(false);
    }

    // Button�𗣂���
    public override void OnPointerUp(PointerEventData E)
    {
        // Map�̈ړ����ɂ���
        MapScroll.instance.SetOnDrag(true);
    }

    // Mouse��Button�̏�ɓ��鎞
    public override void OnPointerEnter(PointerEventData E)
    {
        MainGameLogic logic = LogicManager.instance.GetSceneLogic<MainGameLogic>();
        if (logic.isRouteSelect())
        {
            ShowEventPreview();
        }
    }

    // Mouse��Button�̏ォ�痣��鎞
    public override void OnPointerExit(PointerEventData E)
    {
        MainGameLogic logic = LogicManager.instance.GetSceneLogic<MainGameLogic>();
        if (logic.isRouteSelect())
        {
            EndEventPreview();
        }
    }

    public EventButtonUIElement GetEventButtonUIElement()
    {
        return eventButtonUI;
    }
}