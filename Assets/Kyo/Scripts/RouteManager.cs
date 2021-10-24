using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouteManager : Monosingleton<RouteManager>
{
    [SerializeField] private List<EventButton> routePoints = null;
    [SerializeField] private LineRenderer routeLine = null;
    [SerializeField] private float lineWidth = 0.035f;
    [SerializeField] private bool routePlanned = false;
    [SerializeField] private Player player = null;
    [SerializeField] private EventButton basePoint = null;

    public override void InitAwake()
    {
        routePoints = new List<EventButton>();
        routeLine.startWidth = lineWidth;
        routeLine.endWidth = lineWidth;
    }

    // PlayerのBaseをStartPointに登録
    public void SetStartPoint(EventButton Point)
    {  
        basePoint = Point;

        if (routePoints.Count == 0)
        {
            routePoints.Add(basePoint);
        }
        else
        {
            routePoints[0] = basePoint;
        }

        // LineRendererにStartPointを登録
        routeLine.positionCount = 1;
        routeLine.SetPosition(0, routePoints[0].transform.localPosition);

        // Playerの初期位置
        player.gameObject.SetActive(true);
        player.transform.localPosition = basePoint.transform.localPosition;
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

        CheckRoutePlanned();
    }

    // ループする経路になるには、少なくても点が三つ必要
    private void CheckRoutePlanned()
    {
        if (routePoints.Count >= 3)
        {
            routePlanned = true;
        }
        else
        {
            routePlanned = false;
            routeLine.loop = false;
        }
    }

    // Baseをタップするとき、常に道のループ状態をチェックする
    public void SetRouteLoop()
    {
        if (routePlanned)
        {
            routeLine.loop = !routeLine.loop;
        }
        else
        {
            routeLine.loop = false;
        }
    }

    // Routeの太さ
    public void SetLineWidthWithMapScale(float Scale)
    {
        routeLine.startWidth = lineWidth * Scale;
        routeLine.endWidth = routeLine.startWidth;
    }

    // Route上の移動開始
    public void StartMovePath()
    {
        if (routeLine.loop)
        {
            routePoints.Add(routePoints[0]);
            routePoints.RemoveAt(0);

            MovePath();
        }
    }

    // Route上の移動
    public void MovePath()
    {
        player.MovePath();
    }

    public void SetPlayerPostion(Vector3 Offset)
    {
        player.SetPosition(Offset);
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
        routeLine.loop = false;
        routeLine.positionCount = 1;
        routePoints.Add(basePoint);
        routeLine.SetPosition(0, routePoints[0].transform.localPosition);

        // 仮
        MainGameLogic logic = LogicManager.instance.GetSceneLogic<MainGameLogic>();
        logic.SetNextSate(MainGameState.RouteSelect);
    }
}
