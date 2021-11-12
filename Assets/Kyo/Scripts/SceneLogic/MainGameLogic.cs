using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MainGameState
{
    BaseSelect,
    RouteSelectPre,
    RouteSelect,
    RouteMove,
    EventPlayPre,
    EventPlay,
}

public class MainGameLogic : BaseSceneLogic
{
    [SerializeField] private MainGameState state = MainGameState.BaseSelect;

    void Start()
    {
        MapScroll.instance.Init();
        GridScroll.instance.Init();
        EventButtonManager.instance.Init();
        Player.instance.Init();
        EventUIManager.instance.Init();
        FuelGauge.instance.Init();
    }

    public override void UpdateScene()
    {
        switch (state)
        {
            case MainGameState.BaseSelect:
                BaseSelect();
                break;
            case MainGameState.RouteSelectPre:
                RouteSelectPre();
                break;
            case MainGameState.RouteSelect:
                RouteSelect();
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

    void RouteSelectPre()
    {
        EventButtonManager.instance.ActiveEventButton();
        FuelGauge.instance.ActiveFuelGauge();
        SetNextSate(MainGameState.RouteSelect);
    }

    void RouteSelect()
    {
        MapController();
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
