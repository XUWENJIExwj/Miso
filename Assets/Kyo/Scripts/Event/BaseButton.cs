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
        baseButtonUI.eventDesc.text = eventSO.eventTitle;
        baseButtonUI.moveability.text = "�ړ�: ";
        //baseButtonUI.ama.sprite = Base�p��AMASprite������p�ӂ���
        baseButtonUI.amaType.text = DictionaryManager.instance.GetAMAType(eventSO.amaType);
        baseButtonUI.got.text = "���l��";
        if (Player.instance.CheckUnlockedAMAs(((BaseEventSO)eventSO).ama))
        {
            baseButtonUI.got.text = "�l���ς�";
        }

        if (isSelected)
        {
            baseButtonUI.moveability.text += "�I��";
        }
        else
        {
            if (!IsCurrentBase())
            {
                DoScaleUp();
            }

            if (CheckDistance(RouteManager.instance.GetPreviousRoutePoint()) || !RouteManager.instance.RouteCouldBePlanned())
            {
                baseButtonUI.moveability.text += "�s";
            }
            baseButtonUI.moveability.text += "��";
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

    // State���Ƃ̏���
    // RouteSelect
    public override void OnRouteSelect()
    {
        if (RouteManager.instance.RouteCouldBePlanned())
        {
            if (!isSelected)
            {
                if (CheckDistance(RouteManager.instance.GetPreviousRoutePoint())) return;

                SetSelected(true);
                RouteManager.instance.AddRoutePoint(this);
                baseButtonUI.moveability.text = "�ړ�: �I��";
            }
            else
            {
                SetSelected(false);
                RouteManager.instance.RemoveRoutePoints(this);
                baseButtonUI.moveability.text = "�ړ�: ��";
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

    public new BaseButtonUIElement GetEventButtonUIElement()
    {
        return baseButtonUI;
    }
}
