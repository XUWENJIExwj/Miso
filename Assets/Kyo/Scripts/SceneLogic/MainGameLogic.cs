using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum MainGameState
{
    BaseSelect,
    BaseConfirm,
    RouteSelectPre,
    RouteSelect,
    NewAMAPre,
    NewAMA,
    AMASwitch,
    RouteMove,
    EventPlayPre,
    EventPlay,
}

public class MainGameLogic : BaseSceneLogic
{
    [SerializeField] private MainGameState state = MainGameState.BaseSelect;

    void Start()
    {
        DOTween.SetTweensCapacity(1000, 50);
        MapScroll.instance.Init();
        GridScroll.instance.Init();
        PollutionMap.instance.Init();
        EventButtonManager.instance.Init();
        NewAMAView.instance.Init();
        Player.instance.Init();
        EventUIManager.instance.Init();
        BaseConfirmView.instance.Init();
        AMASwitchView.instance.Init();
        FuelGauge.instance.Init();
        Timer.instance.Init();
    }

    public override void UpdateScene()
    {
        switch (state)
        {
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
