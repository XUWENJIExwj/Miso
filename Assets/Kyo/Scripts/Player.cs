using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using EventScriptableObject;

[Serializable]
public struct PlayerData
{
    public int courseCount;
    public int courseResetCount;
    public AMASO[] amas;
    public AMAs ama;
    public AMANewFlags newFlags;
    public int baseIndex;
    public int totalPoint;
    public int currentPoint;
    public int encounter;
    public int encounterRatio;
    public AchievementProgress achievements;
    public bool tutorial;
    public Vector3 position;
    public Vector2 mapUVOffset;
    public Vector2 gridUVOffset;
    public PollutionMapData pollutionMapData;
    public AllEventButtonsData eventData;

    public void Init()
    {
        courseCount = 0;
        courseResetCount = 2;
        amas = new AMASO[(int)AMAs.Max];
        ama = AMAs.Max;
        newFlags.Init();
        baseIndex = -1;
        totalPoint = 0;
        currentPoint = 0;
        encounter = 10;
        encounterRatio = 10;
        achievements.Init();
        tutorial = true;
        mapUVOffset = new Vector2(0.0f, 0.00156f);
        gridUVOffset = new Vector2(0.5f, 0.5f);
    }
}

[Serializable]
public class Player : Monosingleton<Player>
{
    [SerializeField] private PlayerData playerData;
    [SerializeField] private Image amaIcon = null;
    [SerializeField] private EventButton currentEvent = null;
    [SerializeField] private bool mainEventPlayed = false;
    [SerializeField] private Vector3 iconOffset = Vector3.zero;
    [SerializeField] private AMAs newAMA = AMAs.Max;
    [SerializeField] private BaseButton currentBase = null;

    private Tween tweener = null;

   // PlayerData
   public void Init()
    {
        // ��
        playerData = Score.instance.GetSaveData();

        GlobalInfo.instance.playerData = playerData;

        gameObject.SetActive(false);
    }

    public void SetMainGameState()
    {
        // ��
        MainGameLogic logic = LogicManager.instance.GetSceneLogic<MainGameLogic>();

        if (playerData.tutorial)
        {
            logic.SetNextSate(MainGameState.Tutorial);
        }
        else if (playerData.baseIndex >= 0)
        {
            // Player�̏����ʒu
            AMAs work = playerData.ama;
            SetFirstBase(EventButtonManager.instance.GetBaseEventButton(playerData.baseIndex));
            SetCurrentAMA(work);

            // Player��Base��StartPoint�ɓo�^
            RouteManager.instance.SetStartPoint();
            logic.SetNextSate(MainGameState.RouteSelectPre);
        }
        else
        {
            logic.SetNextSate(MainGameState.BaseSelect);
        }
    }

    public void Load(PlayerData Data)
    {
        playerData = Data;
        transform.localPosition = Data.position;

        gameObject.SetActive(false);
    }

    public void Save()
    {
        if (currentBase)
        {
            playerData.baseIndex = currentBase.GetEventSO<BaseEventSO>().id;
        }
        else
        {
            playerData.baseIndex = -1;
        }
        playerData.position = transform.localPosition;
        playerData.mapUVOffset = MapScroll.instance.GetUVOffset();
        playerData.gridUVOffset = GridScroll.instance.GetUVOffset();
        playerData.pollutionMapData = PollutionMap.instance.GetPollutionMapData();
        playerData.eventData = EventButtonManager.instance.GetAllEventButtonsData();

        Score.instance.Save(playerData);
    }

    public PlayerData GetPlayerData()
    {
        return playerData;
    }

    public void SetTutorialFlag()
    {
        playerData.tutorial = false;
    }

    public bool GetAMANewFlag(AMAs AMA)
    {
        return playerData.newFlags.newAMA[(int)AMA];
    }

    public void SetAMANewFlag(AMAs AMA)
    {
        playerData.newFlags.newAMA[(int)AMA] = false;
    }

    public void SetMainEventCompleted(int ID)
    {
        playerData.achievements.main[(int)GetCurrentAMA()].completed[ID] = true;
    }

    public void SetSubEventCompleted(int ID)
    {
        playerData.achievements.sub.completed[ID] = true;
    }

    public void SetRandomEventCompleted(int ID)
    {
        playerData.achievements.random.completed[ID] = true;
    }

    public void ActiveSwitchView()
    {
        AMASwitchView.instance.ActiveSwitchView(playerData);
    }

    public void CompleteCourse()
    {
        mainEventPlayed = false;
        RouteManager.instance.ResetMainEventStoredFlag();
        ++playerData.courseCount;
        GlobalInfo.instance.playerData = playerData;

        if ((playerData.courseCount % playerData.courseResetCount) == 0)
        {
            EventButtonManager.instance.ResetEvents();
            PollutionMap.instance.ResetPollutionLevel();
        }
    }

    public void ActiveNewAMAView()
    {
        if (newAMA < AMAs.Max)
        {
            NewAMAView.instance.ActiveNewAMAView(newAMA);
            newAMA = AMAs.Max;
        }
    }

    public void AddAMA(AMAs AMA, bool Setting = false)
    {
        if (playerData.amas[(int)AMA]) return;

        playerData.amas[(int)AMA] = GlobalInfo.instance.amaList[(int)AMA];

        if (Setting)
        {
            SetCurrentAMA(AMA);
        }
        else
        {
            newAMA = AMA;
        }

        // ��
        GlobalInfo.instance.playerData = playerData;
    }

    public void SetCurrentAMA(AMAs AMA)
    {
        playerData.ama = AMA;
        amaIcon.sprite = GlobalInfo.instance.amaList[(int)AMA].icon;
    }

    public AMAs GetCurrentAMA()
    {
        return playerData.ama;
    }

    public bool CheckUnlockedAMA(AMAs AMA)
    {
        return playerData.amas[(int)AMA] != null;
    }

    public AMASO GetCurrentAMASO()
    {
        return playerData.amas[(int)GetCurrentAMA()];
    }

    public AMATypes GetCurrentAMAType()
    {
        return playerData.amas[(int)GetCurrentAMA()].type;
    }

    public int GetCurrentAMAEnergy()
    {
        return playerData.amas[(int)GetCurrentAMA()].energy;
    }

    public int GetCurrentAMATimePerGrid()
    {
        return (int)playerData.amas[(int)GetCurrentAMA()].timePerGrid;
    }

    public void SetCurrentEvent(EventButton Event)
    {
        currentEvent = Event;
    }

    public void ResetCurrentEvent()
    {
        if (!currentEvent.IsBase())
        {
            currentEvent.SetPollutionLevel();

            if (!currentEvent.IsMainEvent() || !mainEventPlayed)
            {
                currentEvent.CreateRandomEvent();
            }
        }
    }

    public EventButton GetCurrentEvent()
    {
        return currentEvent;
    }

    public void SetFirstBase(BaseButton Base)
    {
        gameObject.SetActive(true);
        currentBase = Base;
        currentBase.SetEventButtonColor(Color.red);
        transform.localPosition = currentBase.transform.localPosition + iconOffset;

        SetCurrentAMA(Base.GetAMA());

        // ��
        GlobalInfo.instance.playerData = playerData;
    }

    public void SetNewBase(BaseButton Base)
    {
        currentBase.SetEventButtonColor(Color.white);
        currentBase = Base;
        currentBase.SetEventButtonColor(Color.red);

        AddAMA(Base.GetAMA());

        // ��
        GlobalInfo.instance.playerData = playerData;
    }

    public EventButton GetCurrentBase()
    {
        return currentBase;
    }

    public Vector3 CurrentBasePosition()
    {
        return currentBase.transform.localPosition;
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

    public int GetTotalPoint()
    {
        return playerData.totalPoint;
    }

    public int GetCurrentPoint()
    {
        return playerData.currentPoint;
    }

    public void ResetEncounter()
    {
        playerData.encounter = 10;

        // ��
        GlobalInfo.instance.playerData = playerData;
    }

    public bool Encounter()
    {
        if (UnityEngine.Random.Range(0, 100) <= playerData.encounter)
        {
            ResetEncounter();
            return true;
        }
        playerData.encounter += playerData.encounterRatio;

        // ��
        GlobalInfo.instance.playerData = playerData;
        return false;
    }

    public void SetMainEventPlayedFlag()
    {
        mainEventPlayed = true;
    }

    public bool MainEventPlayed()
    {
        return mainEventPlayed;
    }

    // PlayerMove
    public void FixPostionBeforeMove()
    {
        Vector2 offset = new Vector2(transform.localPosition.x, 0.0f);
        float time = Mathf.Abs(offset.x * 0.005f);
        time = Mathf.Min(time, 1.0f);
        tweener = transform.DOLocalMoveX(0.0f, time);
        tweener.SetEase(Ease.Linear);
        tweener.OnComplete(() => { RouteManager.instance.MovePath(); });
        tweener.OnUpdate(() => { RouteManager.instance.DrawRoute(); });

        PollutionMap.instance.MovePath(offset, time);
        EventButtonManager.instance.MovePath(offset, time);

        offset /= GlobalInfo.instance.mapSize;
        MapScroll.instance.MovePath(offset, time);
        GridScroll.instance.MovePath(offset, time);

        Timer.instance.ShowTimer();
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

        transform.localPosition = fixPos;
    }

    public void MovePath()
    {
        EventUIManager.instance.ResetEventInfo();
        EventButton nextPoint = RouteManager.instance.GetNextRoutePoint();

        if (nextPoint)
        {
            ResetCurrentEvent();

            MainGameLogic logic = LogicManager.instance.GetSceneLogic<MainGameLogic>();
            logic.SetNextSate(MainGameState.RouteMove);

            float time = GetCurrentAMATimePerGrid();
            Vector2 offset = new Vector2(nextPoint.transform.localPosition.x - transform.localPosition.x, 0.0f);

            PollutionMap.instance.MovePath(offset, time);
            EventButtonManager.instance.MovePath(offset, time);

            offset /= GlobalInfo.instance.mapSize;
            MapScroll.instance.MovePath(offset, time);
            GridScroll.instance.MovePath(offset, time);

            tweener = transform.DOLocalMoveY(nextPoint.transform.localPosition.y, GetCurrentAMATimePerGrid());
            tweener.SetEase(Ease.Linear);
            tweener.OnStart(() => { Timer.instance.StartTimer(GetCurrentAMATimePerGrid()); });
            tweener.OnUpdate(() => { RouteManager.instance.DrawRoute(); });
            tweener.OnComplete(() =>
            {
                nextPoint.SetSelected(false);
                if (!nextPoint.IsCurrentBase())
                {
                    nextPoint.DoScaleDown();
                }
                
                SetCurrentEvent(nextPoint);
                FuelGauge.instance.HideFuelGauge();
                Timer.instance.HideTimer();
                logic.SetNextSate(MainGameState.EventPlayPre);
            });
        }
        else
        {
            ActiveNewAMAView();
        }
    }

    //public void MovePath()
    //{
    //    MainGameLogic logic = LogicManager.instance.GetSceneLogic<MainGameLogic>();
    //    logic.SetNextSate(MainGameState.RouteMove);

    //    tweener = transform.DOLocalMove(nextPoint.transform.localPosition, GetCurrentAMATimePerGrid());
    //    tweener.SetEase(Ease.Linear);
    //    tweener.OnStart(() => { Timer.instance.StartTimer(GetCurrentAMATimePerGrid()); });
    //    tweener.OnComplete(() =>
    //    {
    //        NextPoint.SetSelected(false);
    //        if (!NextPoint.IsCurrentBase())
    //        {
    //            NextPoint.DoScaleDown();
    //        }

    //        SetCurrentEvent(NextPoint);
    //        FuelGauge.instance.HideFuelGauge();
    //        Timer.instance.HideTimer();
    //        logic.SetNextSate(MainGameState.EventPlayPre);
    //    });
    //}
}
