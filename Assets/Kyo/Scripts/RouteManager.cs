using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouteManager : Monosingleton<RouteManager>
{
    [SerializeField] private List<EventButton> routePoints = null;
    [SerializeField] private int next = 1;
    [SerializeField] private LineRenderer prefab = null;
    [SerializeField] private List<LineRenderer> routes = null;
    [SerializeField] private float lineWidth = 0.035f;
    [SerializeField] private bool routePlanned = false;

    public override void InitAwake()
    {
        routePoints = new List<EventButton>();
        routes = new List<LineRenderer>();
        next = 1;
        AddRoute();
    }

    public void AddRoute()
    {
        for (int i = 0; i < GlobalInfo.instance.GetMaxAMAEnegry(); ++i)
        {
            LineRenderer route = Instantiate(prefab, transform);
            route.startWidth = lineWidth;
            route.endWidth = lineWidth;
            route.positionCount = 2;
            routes.Add(route);
        }
    }

    // PlayerÇÃBaseÇStartPointÇ…ìoò^
    public void SetStartPoint()
    {
        routePoints.Add(Player.instance.GetCurrentBase());
        DrawRoute();
    }

    public void AddRoutePoint(EventButton Point)
    {
        routePoints.Add(Point);
        DrawRoute();
        FuelGauge.instance.ReduceValueOnRouteSelect();
    }

    public void RemoveRoutePoints(EventButton Point)
    {
        int index = routePoints.FindIndex(1, routePoints.Count - 1, (EventButton routePoint) => routePoint == Point);
        for (int i = index + 1; i < routePoints.Count; ++i)
        {
            if (!routePlanned || i < routePoints.Count - 1)
            {
                routePoints[i].DoScaleDown();
            }

            routePoints[i].SetSelected(false);
        }

        int count = routePoints.Count - index;
        routePoints.RemoveRange(index, count);
        FuelGauge.instance.AddValueOnRouteSelect(count);
        DrawRoute();

        routePlanned = false;
    }

    public void DrawRoute()
    {
        int i;
        for (i = 0; i < routePoints.Count - 1; ++i)
        {
            if (CheckRoutePointsInterval(routePoints[i].transform.localPosition.x, routePoints[i + 1].transform.localPosition.x))
            {
                routes[i].SetPosition(0, routePoints[i].transform.localPosition);
                routes[i].SetPosition(1, routePoints[i + 1].transform.localPosition);
                routes[i].gameObject.SetActive(true);
            }
        }

        for(; i < routes.Count; ++i)
        {
            routes[i].gameObject.SetActive(false);
        }
    }

    public bool CheckRoutePointsInterval(float A, float B)
    {
        return Mathf.Abs(A - B) <= GridScroll.instance.GetGridInterval().x + EventButton.GetMaxSize().x;
    }

    public bool RouteCouldBePlanned()
    {
        return routePoints.Count >= 3;
    }

    public bool RoutePlanned()
    {
        return routePlanned;
    }


    public void SetRoutePlanned(bool Planned)
    {
        routePlanned = Planned;
    }

    public EventButton GetPreviousRoutePoint()
    {
        return routePoints[routePoints.Count - 1];
    }

    // RouteÇÃëæÇ≥
    public void SetLineWidthWithMapScale(float Scale)
    {
        foreach (LineRenderer route in routes)
        {
            route.startWidth = lineWidth * Scale;
            route.endWidth = route.startWidth;
        }
    }

    // Routeè„ÇÃà⁄ìÆäJén
    public void StartMovePath()
    {
        MainGameLogic logic = LogicManager.instance.GetSceneLogic<MainGameLogic>();

        if (routePlanned && logic.isRouteSelect())
        {
            FuelGauge.instance.ResetValuesWithAnimation(Player.instance.GetCurrentAMAEnergy());
            Player.instance.SetNewBase(routePoints[routePoints.Count - 1]);
            Player.instance.ResetEncounter();
            if (!routePoints[0].IsCurrentBase())
            {
                routePoints[0].DoScaleDown();
            }
            Player.instance.SetCurrentEvent(routePoints[0]);

            MovePath();
        }
    }

    // Routeè„ÇÃà⁄ìÆ
    public void MovePath()
    {
        FuelGauge.instance.ShowFuelGauge();
        FuelGauge.instance.ReduceValueOnMove();
        Timer.instance.ShowTimer();
        Player.instance.MovePath();
    }

    public EventButton GetNextRoutePoint()
    {
        if (next < routePoints.Count)
        {
            EventButton eventButton = routePoints[next];
            ++next;
            return eventButton;
        }

        ResetRoute();
        return null;
    }

    public void ResetRoute()
    {
        next = 1;
        routePoints.Clear();
        routePlanned = false;
        SetStartPoint();

        FuelGauge.instance.ResetValuesWithAnimation(Player.instance.GetCurrentAMAEnergy());
        Timer.instance.ResetTimer(0);

        // âº
        MainGameLogic logic = LogicManager.instance.GetSceneLogic<MainGameLogic>();
        logic.SetNextSate(MainGameState.RouteSelect);
    }
}
