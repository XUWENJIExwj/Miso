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
    [SerializeField] protected Vector2 size = Vector2.zero;
    static protected Vector2 maxSize = Vector2.zero;

    protected Tweener tweener = null;

    public virtual void Init(Oceans Ocean, OceanAreas OceanArea)
    {
        SetOceanInfo(Ocean, OceanArea);
        CreateBaseEvent();
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

    public virtual void SetPollutionLevel()
    {
        PollutionMap.instance.SetPollutionLevel(ocean, oceanArea, this);
    }

    public virtual void CreateBaseEvent()
    {
        gameObject.SetActive(false);
    }

    public virtual void CreateEvent()
    {
        eventSO = GlobalInfo.instance.CreateEventSO();
        image.sprite = eventSO.icons[0];

        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.sizeDelta = eventSO.Resize();
        size = rectTransform.sizeDelta;

        maxSize.x = Mathf.Max(maxSize.x, rectTransform.sizeDelta.x);
        maxSize.y = Mathf.Max(maxSize.y, rectTransform.sizeDelta.y);

        if (eventSO.type == EventSOType.MainEvent)
        {
            EventButtonManager.instance.AddMainEvent(this);
        }
    }

    public virtual void ResetEvent()
    {
        eventSO = GlobalInfo.instance.CreateEventSO();
        image.sprite = eventSO.icons[0];

        Resize();

        if (eventSO.type == EventSOType.MainEvent)
        {
            EventButtonManager.instance.AddMainEvent(this);
        }
    }

    public void CreateRandomEvent()
    {
        if (tweener.IsActive())
        {
            tweener.OnComplete(() => { CreateRandomEventSO(); });
        }
        else
        {
            CreateRandomEventSO();
        }
    }

    public void CreateRandomEventSO()
    {
        eventSO = GlobalInfo.instance.CreateRandomEventSO();
        image.sprite = eventSO.icons[0];
        Resize();
    }

    public void Resize()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.sizeDelta = eventSO.Resize();
        size = rectTransform.sizeDelta;
    }

    public void LinkMainEventToAMA(AMAs AMA)
    {
        eventSO = GlobalInfo.instance.mainEventLists[(int)AMA].mainEvents[eventSO.id];
    }

    public void Move(Vector2 Offset)
    {
        transform.localPosition += new Vector3(Offset.x, Offset.y, 0.0f);

        // ��ʊO�ɂȂ�����A���[�v������
        FixPostion();
    }

    public void MovePath(Vector2 Offset, float Time)
    {
        Tweener tweener = transform.DOLocalMoveX(transform.localPosition.x - Offset.x, Time);
        tweener.SetEase(Ease.Linear);
        tweener.OnUpdate(() => { FixPostion(); });
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

        transform.localPosition = fixPos;
    }

    // �g��
    public void DoScaleUp(float MaxScale = 1.5f, float Time = 0.5f)
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        tweener = rectTransform.DOSizeDelta(size * MaxScale, Time);
    }

    // �k��
    public void DoScaleDown(float MinScale = 1.0f, float Time = 0.5f)
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        tweener = rectTransform.DOSizeDelta(size * MinScale, Time);
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
        Vector2 interval = GridScroll.instance.GetGridInterval();
        interval = HelperFunction.CeilVector2(interval);
        float length2 = HelperFunction.Length2(interval);
        float distance2 = HelperFunction.Distance2(transform.localPosition, Event.transform.localPosition);

        return distance2 > length2;
    }

    public virtual void ShowEventPreview()
    {
        EventPreview eventPreview = EventButtonManager.instance.GetEventPreview();
        eventPreview.gameObject.SetActive(true);
        eventPreview.ResetPreview();
        eventPreview.SetPosition(transform.localPosition);
        eventPreview.SetMoveability("�ړ�: ");

        if (eventSO.IsRandomEvent())
        {
            eventPreview.FixPosition();
            eventPreview.SetEventDesc("�����N���邩��");
            eventPreview.SetAMATypeActive(false);
            eventPreview.SetPointRangeActive(false);
        }
        else
        {
            eventPreview.SetEventDesc(eventSO.eventTitle);
            eventPreview.SetAMAType("�L���^�C�v: " + DictionaryManager.instance.GetAMAType(eventSO.amaType));
            eventPreview.SetAMATypeActive(true);
            eventSO.MakePointRange();
            eventPreview.SetPointRange("Point: " + eventSO.pointRange.min.ToString() + "�`" + eventSO.pointRange.max.ToString());
            eventPreview.SetPointRangeActive(true);
        }

        if (isSelected)
        {
            eventPreview.AddMoveability("�I��");
        }
        else
        {
            DoScaleUp();

            if (FuelGauge.instance.NoMoreFuel())
            {
                eventPreview.AddMoveability("�R���s��");
            }
            else
            {
                if (ImmovableDistance(RouteManager.instance.GetPreviousRoutePoint()) || RouteManager.instance.RoutePlanned())
                {
                    eventPreview.AddMoveability("�s");
                }
                eventPreview.AddMoveability("��");

                if (RouteManager.instance.MainEventStored() && eventSO.IsMainEvent())
                {
                    eventPreview.AddMoveability("\n�i�C�x���g�����Ȃ��j");
                }
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

    // State���Ƃ̏���
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
            eventPreview.SetMoveability("�ړ�: �I��");

            if (eventSO.IsRandomEvent())
            {
                image.sprite = eventSO.icons[1];
            }
        }
        else
        {
            SetSelected(false);
            RouteManager.instance.RemoveRoutePoints(this);
            eventPreview.SetMoveability("�ړ�: ��");

            if (RouteManager.instance.MainEventStored() && eventSO.IsMainEvent())
            {
                eventPreview.AddMoveability("\n�i�C�x���g�����Ȃ��j");
            }
            else if (eventSO.IsRandomEvent())
            {
                image.sprite = eventSO.icons[0];
            }
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
}