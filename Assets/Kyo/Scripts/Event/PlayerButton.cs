using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerButton : Button
{
    public void OnClick()
    {
        Player.instance.GetCurrentBase().OnClick();
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
            Player.instance.GetCurrentBase().ShowEventPreview();
        }
    }

    // Mouse��Button�̏ォ�痣��鎞
    public override void OnPointerExit(PointerEventData E)
    {
        MainGameLogic logic = LogicManager.instance.GetSceneLogic<MainGameLogic>();
        if (logic.isRouteSelect())
        {
            Player.instance.GetCurrentBase().EndEventPreview();
        }
    }
}
