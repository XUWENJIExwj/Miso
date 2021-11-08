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
        transform.DOScale(MaxScale, Time);
    }

    // �k��
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

    // State���Ƃ̏���
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
        if (logic.isRouteSelect() && !isSelected)
        {
            DoScaleUp();
        }
    }

    // Mouse��Button�̏ォ�痣��鎞
    public override void OnPointerExit(PointerEventData E)
    {
        MainGameLogic logic = LogicManager.instance.GetSceneLogic<MainGameLogic>();
        if (logic.isRouteSelect() && !isSelected)
        {
            DoScaleDown();
        }
    }
}