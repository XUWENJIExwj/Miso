using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SplitOceanPart : OceanPart
{
    static private int point = 0;

    public override void Init()
    {
        base.Init();

        point = 0;
    }

    public override void AddCleanupPoint(EventButton Event)
    {
        if (point == 0)
        {
            point = HelperFunction.RandomPointRange(GlobalInfo.instance.pollutionInfos[(int)level].pointRange);
        }
        else
        {
            ShowCleanupView(point, Event);
            Player.instance.AddPoint(point);
            point = 0;
        }
    }
}
