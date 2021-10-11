using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RouteManager : Monosingleton<RouteManager>
{
    [SerializeField] private List<EventButton> routePoints = null;
    [SerializeField] private LineRenderer routeLine = null;
    [SerializeField] private float lineWidth = 0.035f;
    [SerializeField] private bool routePlanned = false;
    [SerializeField] private Player player = null;

    public override void InitAwake()
    {
        routePoints = new List<EventButton>();
        routeLine.positionCount = routePoints.Count;
        routeLine.startWidth = lineWidth;
        routeLine.endWidth = lineWidth;
    }

    // Player��Base��StartPoint�ɓo�^
    public void SetStartPoint(EventButton Point)
    {
        if (routePoints.Count == 0)
        {
            routePoints.Add(Point);
        }
        else
        {
            routePoints[0] = Point;
        }

        // Player�̏����ʒu
        player.gameObject.SetActive(true);
        player.transform.localPosition = Point.transform.localPosition;
    }

    public void AddRoutePoint(EventButton Point)
    {
        routePoints.Add(Point);
        DrawRoute();
    }

    public void RemoveRoutePoint(EventButton Point)
    {
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

    public void SetLineWidthWithMapScale(float Scale)
    {
        routeLine.startWidth = lineWidth * Scale;
        routeLine.endWidth = routeLine.startWidth;
    }

    public void Go()
    {
        if(routeLine.loop)
        {
            Vector3[] points = new Vector3[routePoints.Count + 1];
            for (int i = 0; i < routePoints.Count; ++i)
            {
                points[i] = routePoints[i].transform.localPosition;
            }
            points[routePoints.Count] = points[0];
            player.MovePath(points);
        }
    }

    public void SetPlayerPostion(Vector3 Offset)
    {
        player.SetPosition(Offset);
    }
}
