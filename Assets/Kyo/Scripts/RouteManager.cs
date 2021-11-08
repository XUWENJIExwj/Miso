using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouteManager : Monosingleton<RouteManager>
{
    [SerializeField] private List<EventButton> routePoints = null;
    [SerializeField] private LineRenderer routeLine = null;
    [SerializeField] private float lineWidth = 0.035f;
    [SerializeField] private bool routePlanned = false;

    public override void InitAwake()
    {
        routePoints = new List<EventButton>();
        routeLine.startWidth = lineWidth;
        routeLine.endWidth = lineWidth;
    }

    // PlayerÇÃBaseÇStartPointÇ…ìoò^
    public void SetStartPoint()
    {
        AddRoutePoint(Player.instance.GetCurrentBase());
        DrawRoute();
    }

    public void AddRoutePoint(EventButton Point)
    {
        Point.DoScaleUp();
        routePoints.Add(Point);
        DrawRoute();
    }

    public void RemoveRoutePoint(EventButton Point)
    {
        Point.DoScaleDown();
        routePoints.Remove(Point);
        DrawRoute();
    }

    public void DrawRoute()
    {
        routeLine.positionCount = routePoints.Count;
        for (int i = 0; i < routePoints.Count; ++i)
        {
            routeLine.SetPosition(i, routePoints[i].transform.localPosition);
        }
    }

    public bool RouteCouldBePlanned()
    {
        return routePoints.Count >= 3;
    }


    public void SetRoutePlanned(bool Planned)
    {
        routePlanned = Planned;
    }

    // RouteÇÃëæÇ≥
    public void SetLineWidthWithMapScale(float Scale)
    {
        routeLine.startWidth = lineWidth * Scale;
        routeLine.endWidth = routeLine.startWidth;
    }

    // Routeè„ÇÃà⁄ìÆäJén
    public void StartMovePath()
    {
        MainGameLogic logic = LogicManager.instance.GetSceneLogic<MainGameLogic>();

        if (routePlanned && logic.isRouteSelect())
        {
            Player.instance.SetNewBase(routePoints[routePoints.Count - 1]);
            if (!routePoints[0].IsCurrentBase())
            {
                routePoints[0].DoScaleDown();
            }
            routePoints.RemoveAt(0);
            MovePath();
        }
    }

    // Routeè„ÇÃà⁄ìÆ
    public void MovePath()
    {
        Player.instance.MovePath();
    }

    public void SetPlayerPostion(Vector3 Offset)
    {
        Player.instance.SetPosition(Offset);
    }

    public EventButton GetNextRoutePoint()
    {
        if (routePoints.Count > 0)
        {
            EventButton eventButton = routePoints[0];

            routePoints.RemoveAt(0);

            return eventButton;
        }

        ResetRoute();
        return null;
    }

    public void ResetRoute()
    {
        routePlanned = false;
        SetStartPoint();

        // âº
        MainGameLogic logic = LogicManager.instance.GetSceneLogic<MainGameLogic>();
        logic.SetNextSate(MainGameState.RouteSelect);
    }
}
