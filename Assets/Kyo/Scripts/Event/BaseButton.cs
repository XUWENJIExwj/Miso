using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventScriptableObject;
using UnityEngine.EventSystems;

public class BaseButton : EventButton
{
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

    // State���Ƃ̏���
    // RouteSelect
    public override void OnRouteSelect()
    {
        if (RouteManager.instance.RouteCouldBePlanned())
        {
            base.OnRouteSelect();
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
        else if (logic.isRouteSelect() && !isSelected)
        {
            if (!IsCurrentBase())
            {
                DoScaleUp();
            }
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
        else if (logic.isRouteSelect() && !isSelected)
        {
            if (!IsCurrentBase())
            {
                DoScaleDown();
            }
        }
    }

    public AMAs GetAMA()
    {
        return ((BaseEventSO)eventSO).ama;
    }
}
