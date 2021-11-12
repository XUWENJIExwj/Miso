using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using EventScriptableObject;

[Serializable]
public struct PlayerData
{
    public AMASO[] amas;
    public AMAs ama;
    public EventButton basePoint;
    public int totalPoint;
    public int currentPoint;
}

[Serializable]
public class Player : Monosingleton<Player>
{
    [SerializeField] private PlayerData playerData;
    [SerializeField] private EventButton currentEvent = null;
    private Tween tweener = null;

    // PlayerData
    public void Init()
    {
        // ��
        playerData.amas = new AMASO[(int)AMAs.Max];
        playerData.ama = AMAs.Max;
        playerData.basePoint = null;
        playerData.totalPoint = 0;
        playerData.currentPoint = 0;

        GlobalInfo.instance.playerData = playerData;

        gameObject.SetActive(false);
    }

    public void AddAMA(AMAs AMA)
    {
        if (playerData.amas[(int)AMA]) return;

        playerData.amas[(int)AMA] = GlobalInfo.instance.amaList[(int)AMA];

        // ��
        GlobalInfo.instance.playerData = playerData;
    }

    public void SetCurrentAMA(AMAs AMA)
    {
        playerData.ama = AMA;
    }

    public AMAs GetCurrentAMA()
    {
        return playerData.ama;
    }


    public bool CheckUnlockedAMAs(AMAs AMA)
    {
        return playerData.amas[(int)AMA] != null;
    }

    public AMASO GetCurrentAMASO()
    {
        return playerData.amas[(int)GetCurrentAMA()];
    }

    public void SetCurrentEvent(EventButton Event)
    {
        currentEvent = Event;
    }

    public EventButton GetCurrentEvent()
    {
        return currentEvent;
    }

    public void SetFirstBase(BaseButton Base)
    {
        gameObject.SetActive(true);
        playerData.basePoint = Base;
        playerData.basePoint.SetEventButtonColor(Color.red);
        transform.localPosition = playerData.basePoint.transform.localPosition;

        SetCurrentAMA(Base.GetAMA());

        // ��
        GlobalInfo.instance.playerData = playerData;
    }

    public void SetNewBase(EventButton Base)
    {
        playerData.basePoint.SetEventButtonColor(Color.white);
        playerData.basePoint = Base;
        playerData.basePoint.SetEventButtonColor(Color.red);

        // ��
        GlobalInfo.instance.playerData = playerData;
    }

    public Vector3 CurrentBasePosition()
    {
        return playerData.basePoint.transform.localPosition;
    }

    public void AddTotalPoint(int Point)
    {
        playerData.totalPoint += Point;
    }

    public void AddCurrentPoint(int Point)
    {
        playerData.currentPoint += Point;
    }

    public void ResetCurrentPoint()
    {
        playerData.currentPoint = 0;

        // ��
        GlobalInfo.instance.playerData = playerData;
    }

    public void AddPoint(int Point)
    {
        AddTotalPoint(Point);
        AddCurrentPoint(Point);

        // ��
        GlobalInfo.instance.playerData = playerData;
    }

    public int GetCurrentPoint()
    {
        return playerData.currentPoint;
    }

    // PlayerMove
    public void SetPosition(Vector2 Offset)
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

    public void MovePath()
    {
        EventButton nextPoint = RouteManager.instance.GetNextRoutePoint();

        if (nextPoint)
        {
            MainGameLogic logic = LogicManager.instance.GetSceneLogic<MainGameLogic>();
            logic.SetNextSate(MainGameState.RouteMove);

            tweener = transform.DOLocalMove(nextPoint.transform.localPosition, 1.0f); // GetCurrentAMASO().timePerGrid
            tweener.SetEase(Ease.Linear);
            tweener.OnComplete(() =>
            {
                nextPoint.SetSelected(false);
                if (!nextPoint.IsCurrentBase())
                {
                    nextPoint.DoScaleDown();
                }
                
                SetCurrentEvent(nextPoint);
                FuelGauge.instance.HideFuelGauge();
                logic.SetNextSate(MainGameState.EventPlayPre);
            });
        }

        EventUIManager.instance.ResetEventInfo();
    }

    public EventButton GetCurrentBase()
    {
        return playerData.basePoint;
    }

    public int GetTotalPoint()
    {
        return playerData.totalPoint;
    }
}
