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

    // Stateごとの処理
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
        // Playerの初期位置
        Player.instance.SetFirstBase(this);
        Player.instance.AddAMA(((BaseEventSO)eventSO).ama);

        // PlayerのBaseをStartPointに登録
        RouteManager.instance.SetStartPoint();

        // 仮
        MainGameLogic logic = LogicManager.instance.GetSceneLogic<MainGameLogic>();
        logic.SetNextSate(MainGameState.RouteSelectPre);
    }

    // MouseがButtonの上に入る時
    public override void OnPointerEnter(PointerEventData E)
    {
        MainGameLogic logic = LogicManager.instance.GetSceneLogic<MainGameLogic>();

        // BaseSelectの時、BaseのSizeを拡大する
        if (logic.isBaseSelect())
        {
            DoScaleUp();
        }
        // RouteSelectの時、EventButtonのSizeを拡大する
        else if (logic.isRouteSelect() && !isSelected)
        {
            if (!IsCurrentBase())
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
            DoScaleDown();
        }
        // RouteSelectの時、選択されなかった場合、EventButtonのSizeを縮小する
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
