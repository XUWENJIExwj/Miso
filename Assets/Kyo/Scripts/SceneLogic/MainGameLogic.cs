using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum MainGameState
{
    Tutorial,
    BaseSelect,
    BaseConfirm,
    RouteSelectPre,
    RouteSelect,
    NewAMAPre,
    NewAMA,
    AMASwitch,
    Achievement,
    RouteMove,
    EventPlayPre,
    EventPlay,
}

public class MainGameLogic : BaseSceneLogic
{
    [SerializeField] private MainGameState state = MainGameState.Tutorial;

    void Start()
    {
        DOTween.SetTweensCapacity(1000, 50);

        if (LogicManager.instance.OnTest() || Score.instance.IsNewUser())
        {
            MapScroll.instance.Init();
            GridScroll.instance.Init();
            PollutionMap.instance.Init();
            EventButtonManager.instance.Init();
            Player.instance.Init();
        }
        else
        {
            SetNextSate(MainGameState.RouteSelect);

            PlayerData playerData = Score.instance.GetSaveData();
            MapScroll.instance.Load(playerData.mapUVOffset);
            GridScroll.instance.Load(playerData.gridUVOffset);
            PollutionMap.instance.Load(playerData.pollutionMapData);
            EventButtonManager.instance.Load(playerData.eventData);
            Player.instance.Load(playerData);
        }

        TutorialView.instance.Init();
        Timer.instance.Init();
        NewAMAView.instance.Init();
        EventUIManager.instance.Init();
        BaseConfirmView.instance.Init();
        AMASwitchView.instance.Init();
        AchievementView.instance.Init();
        FuelGauge.instance.Init();

        Player.instance.SetMainGameState();
    }

    public override void UpdateScene()
    {
        switch (state)
        {
            case MainGameState.Tutorial:
                Tutorial();
                break;
            case MainGameState.BaseSelect:
                BaseSelect();
                break;
            case MainGameState.BaseConfirm:
                BaseConfirm();
                break;
            case MainGameState.RouteSelectPre:
                RouteSelectPre();
                break;
            case MainGameState.RouteSelect:
                RouteSelect();
                break;
            case MainGameState.NewAMAPre:
                NewAMAPre();
                break;
            case MainGameState.NewAMA:
                NewAMA();
                break;
            case MainGameState.AMASwitch:
                AMASwitch();
                break;
            case MainGameState.Achievement:
                Achievement();
                break;
            case MainGameState.RouteMove:
                RouteMove();
                break;
            case MainGameState.EventPlayPre:
                EventPlayPre();
                break;
            case MainGameState.EventPlay:
                EventPlay();
                break;
            default:
                break;
        }
    }

    void Tutorial()
    {
        
    }

    void BaseSelect()
    {
        MapController();
    }

    void BaseConfirm()
    {

    }

    void RouteSelectPre()
    {
        EventButtonManager.instance.ActiveEventButton();
        FuelGauge.instance.ActiveFuelGauge(Player.instance.GetCurrentAMAEnergy());
        Timer.instance.ActiveTimer();
        SetNextSate(MainGameState.RouteSelect);
    }

    void RouteSelect()
    {
        MapController();
    }

    void NewAMAPre()
    {
        
    }

    void NewAMA()
    {
        NewAMAView.instance.EndNewAMAView();
    }

    void AMASwitch()
    {

    }

    void Achievement()
    {

    }

    void RouteMove()
    {
        //MapController();
    }

    void EventPlayPre()
    {
        EventUIManager.instance.EventPlayPre(Player.instance.GetCurrentEvent());
        SetNextSate(MainGameState.EventPlay);
    }

    void EventPlay()
    {
        EventUIManager.instance.EventPlay();
    }

    void MapController()
    {
        MapScaler.instance.Zoom();
        MapScroll.instance.OnDrag();
    }

    public bool isBaseSelect()
    {
        return state == MainGameState.BaseSelect;
    }

    public bool isRouteSelect()
    {
        return state == MainGameState.RouteSelect;
    }

    public bool IsRouteMove()
    {
        return state == MainGameState.RouteMove;
    }

    public void SetNextSate(MainGameState State)
    {
        state = State;
    }
}
