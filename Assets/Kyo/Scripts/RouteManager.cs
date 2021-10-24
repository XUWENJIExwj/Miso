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

    // Player��Base��StartPoint�ɓo�^
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

        // LineRenderer��StartPoint��o�^
        routeLine.positionCount = 1;
        routeLine.SetPosition(0, routePoints[0].transform.localPosition);

        // Player�̏����ʒu
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

    // ���[�v����o�H�ɂȂ�ɂ́A���Ȃ��Ă��_���O�K�v
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

    // Base���^�b�v����Ƃ��A��ɓ��̃��[�v��Ԃ��`�F�b�N����
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

    // Route�̑���
    public void SetLineWidthWithMapScale(float Scale)
    {
        routeLine.startWidth = lineWidth * Scale;
        routeLine.endWidth = routeLine.startWidth;
    }

    // Route��̈ړ��J�n
    public void StartMovePath()
    {
        if (routeLine.loop)
        {
            routePoints.Add(routePoints[0]);
            routePoints.RemoveAt(0);

            MovePath();
        }
    }

    // Route��̈ړ�
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

        // ��
        MainGameLogic logic = LogicManager.instance.GetSceneLogic<MainGameLogic>();
        logic.SetNextSate(MainGameState.RouteSelect);
    }
}
