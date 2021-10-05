using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouteManager : Monosingleton<RouteManager>
{
    [SerializeField] private List<EventButton> routePoints = null;
    [SerializeField] private LineRenderer routeLine = null;

    public override void InitAwake()
    {
        routePoints = new List<EventButton>();
    }

    public void AddRoutePoint(EventButton Point)
    {
        routePoints.Add(Point);
        DrawRoute();
    }

    public void RemoveRoutePoint(EventButton Point)
    {
        routePoints.Remove(Point);
    }

    public void DrawRoute()
    {
        if (routePoints.Count > 1)
        {
            routeLine.positionCount = routePoints.Count;
            for (int i = 0; i < routePoints.Count; ++i)
            {
                routeLine.SetPosition(i, routePoints[i].transform.localPosition);
            }
        }
    }
}
