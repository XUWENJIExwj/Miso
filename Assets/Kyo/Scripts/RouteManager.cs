using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EventScriptableObject;

public class RouteManager : Monosingleton<RouteManager>
{
    [SerializeField] private Button moveButton = null;
    [SerializeField] private Button switchButton = null;
    [SerializeField] private Button achievementButton = null;
    [SerializeField] private List<EventButton> routePoints = null;
    [SerializeField] private int next = 1;
    [SerializeField] private LineRenderer prefab = null;
    [SerializeField] private List<LineRenderer> routes = null;
    [SerializeField] private float lineWidth = 0.035f;
    [SerializeField] private bool routePlanned = false;
    [SerializeField] private bool mainEventStored = false;

    public override void InitAwake()
    {
        routePoints = new List<EventButton>();
        routes = new List<LineRenderer>();
        next = 1;
        AddRoute();
        ActiveMoveButton(false);
        ActiveButtons(false);
    }

    public void ActiveButtons(bool Active)
    {
        moveButton.gameObject.SetActive(Active);
        switchButton.gameObject.SetActive(Active);
        achievementButton.gameObject.SetActive(Active);
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

    // Player??Base??StartPoint?ɓo?^
    public void SetStartPoint()
    {
        routePoints.Add(Player.instance.GetCurrentBase());
        DrawRoute();
        ActiveButtons(true);
    }

    public void AddRoutePoint(EventButton Point)
    {
        routePoints.Add(Point);
        CheckStoredMainEvent(Point);
        DrawRoute();
        FuelGauge.instance.ReduceValueOnRouteSelect();
    }

    public void ResetMainEventStoredFlag()
    {
        mainEventStored = false;
    }

    public void CheckStoredMainEvent(EventButton Point)
    {
        if (Point.GetEventSO<MainEventSO>() && !mainEventStored)
        {
            mainEventStored = true;
        }
    }

    public bool MainEventStored()
    {
        return mainEventStored;
    }

    public void RemoveRoutePoints(EventButton Point)
    {
        ActiveMoveButton(false);

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

    public void RemoveRoutePointsWhenSwitchAMA()
    {
        for (int i = routePoints.Count - 1; i > 0; --i)
        {
            routePoints[i].DoScaleDown();
            routePoints[i].SetSelected(false);
            routePoints.RemoveAt(i);
        }
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
        moveButton.interactable = Planned;
    }

    public EventButton GetPreviousRoutePoint()
    {
        return routePoints[routePoints.Count - 1];
    }

    // Route?̑???
    public void SetLineWidthWithMapScale(float Scale)
    {
        foreach (LineRenderer route in routes)
        {
            route.startWidth = lineWidth * Scale;
            route.endWidth = route.startWidth;
        }
    }

    // Route???̈ړ??J?n
    public void ActiveMoveButton(bool Active)
    {
        moveButton.interactable = Active;
    }

    public void StartMovePath()
    {
        SoundManager.instance.SE_Go();
        ChangeBGM.instance.BGM_Move();

        MainGameLogic logic = LogicManager.instance.GetSceneLogic<MainGameLogic>();

        if (routePlanned && logic.isRouteSelect())
        {
            ActiveMoveButton(false);
            ActiveButtons(false);

            FuelGauge.instance.ResetValuesWithAnimation(Player.instance.GetCurrentAMAEnergy());
            Player.instance.SetNewBase((BaseButton)routePoints[routePoints.Count - 1]);
            Player.instance.ResetEncounter();
            if (!routePoints[0].IsCurrentBase())
            {
                routePoints[0].DoScaleDown();
            }
            Player.instance.SetCurrentEvent(routePoints[0]);
            Player.instance.FixPostionBeforeMove();
            logic.SetNextSate(MainGameState.RouteMove);
        }
    }

    // Route???̈ړ?
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
        ChangeBGM.instance.BGM_Route();

        next = 1;
        routePoints.Clear();
        routePlanned = false;
        SetStartPoint();

        FuelGauge.instance.ResetValuesWithAnimation(Player.instance.GetCurrentAMAEnergy());
        Timer.instance.ResetTimer(0);

        // ??
        MainGameLogic logic = LogicManager.instance.GetSceneLogic<MainGameLogic>();
        logic.SetNextSate(MainGameState.RouteSelect);
    }
}
