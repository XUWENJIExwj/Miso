using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SplitOceanPart : OceanPart
{
    static private int point = 0;
    static private bool resultAdded = false;

    public override void Init(Oceans Ocean, OceanAreas Area)
    {
        base.Init(Ocean, Area);

        point = 0;
    }

    public override void AddResult()
    {
        if (resultAdded)
        {
            resultAdded = false;
        }
        else
        {
            observer.AddResult();
            resultAdded = true;
        }
        observer.ResetObserver();
    }

    public override int AddCleanupPoint(EventButton Event)
    {
        if (point == 0)
        {
            point = HelperFunction.RandomPointRange(GlobalInfo.instance.pollutionInfos[(int)level].pointRange);
            return point;
        }
        else
        {
            ShowCleanupView(point, Event);
            Player.instance.AddPoint(point);
            int retPoint = point;
            point = 0;
            return retPoint;
        }
    }
}
