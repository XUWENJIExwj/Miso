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

    public override void CreateEvent()
    {
        eventSO = GlobalInfo.instance.CreateEventSO(baseIndex);
    }

    public override void OnClick()
    {
        MainGameLogic logic = LogicManager.instance.GetSceneLogic<MainGameLogic>();

        // State���Ƃ̏���
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
        eventPreview.SetMoveability("�ړ�: ");
        //eventPreview.SetAMASprite(Sprite AMA) Base�p��AMASprite������p�ӂ���
        eventPreview.SetAMAType(DictionaryManager.instance.GetAMAType(eventSO.amaType));
        eventPreview.SetGot("���l��");
        if (Player.instance.CheckUnlockedAMAs(((BaseEventSO)eventSO).ama))
        {
            eventPreview.SetGot("�l���ς�");
        }

        if (isSelected)
        {
            eventPreview.AddMoveability("�I��");
        }
        else
        {
            if (!IsCurrentBase())
            {
                DoScaleUp();
            }

            if (FuelGauge.instance.NoMoreFuel())
            {
                eventPreview.AddMoveability("�R���s��");
            }
            else
            {
                if (ImmovableDistance(RouteManager.instance.GetPreviousRoutePoint()) || !RouteManager.instance.RouteCouldBePlanned())
                {
                    eventPreview.AddMoveability("�s");
                }
                eventPreview.AddMoveability("��");
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

    // State���Ƃ̏���
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
                eventPreview.SetMoveability("�ړ�: �I��");
            }
            else
            {
                SetSelected(false);
                RouteManager.instance.RemoveRoutePoints(this);
                eventPreview.SetMoveability("�ړ�: ��");
            }
            RouteManager.instance.SetRoutePlanned(isSelected);
        }
    }

    // BaseSelect
    public void OnBaseSelect()
    {
        // Player�̏����ʒu
        Player.instance.SetFirstBase(this);
        Player.instance.AddAMA(((BaseEventSO)eventSO).ama);

        // Player��Base��StartPoint�ɓo�^
        RouteManager.instance.SetStartPoint();

        //FuelGauge.instance.ResetMaxValue();

        // ��
        MainGameLogic logic = LogicManager.instance.GetSceneLogic<MainGameLogic>();
        logic.SetNextSate(MainGameState.RouteSelectPre);
    }

    // Mouse��Button�̏�ɓ��鎞
    public override void OnPointerEnter(PointerEventData E)
    {
        MainGameLogic logic = LogicManager.instance.GetSceneLogic<MainGameLogic>();

        // BaseSelect�̎��ABase��Size���g�傷��
        if (logic.isBaseSelect())
        {
            DoScaleUp();
        }
        // RouteSelect�̎��AEventButton��Size���g�傷��
        else if (logic.isRouteSelect())
        {
            ShowEventPreview();
        }
    }

    // Mouse��Button�̏ォ�痣��鎞
    public override void OnPointerExit(PointerEventData E)
    {
        MainGameLogic logic = LogicManager.instance.GetSceneLogic<MainGameLogic>();

        // BaseSelect�̎��ABase��Size���k������
        if (logic.isBaseSelect())
        {
            DoScaleDown();
        }
        // RouteSelect�̎��A�I������Ȃ������ꍇ�AEventButton��Size���k������
        else if (logic.isRouteSelect())
        {
            EndEventPreview();
        }
    }

    public AMAs GetAMA()
    {
        return ((BaseEventSO)eventSO).ama;
    }
}
