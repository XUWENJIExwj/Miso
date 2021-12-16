using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SplitOceanPart : OceanPart
{
    static private bool onSynchro = false;
    static private int synchroPoint = 0;
    static private PollutionLevel synchroLevel = PollutionLevel.None;

    public override void Init(Oceans Ocean, OceanAreas Area)
    {
        base.Init(Ocean, Area);

        synchroPoint = 0;
    }

    public override void Load(Oceans Ocean, OceanAreas Area, PollutionLevel Level)
    {
        base.Load(Ocean, Area, Level);

        synchroPoint = 0;
    }

    public override void ResetPollutionLevel()
    {
        if (onSynchro)
        {
            level = synchroLevel;
            onSynchro = false;
        }
        else
        {
            level = (PollutionLevel)Random.Range((int)PollutionLevel.Level_00, (int)PollutionLevel.None);
            synchroLevel = level;
            onSynchro = true;
        }
        image.DOColor(GlobalInfo.instance.pollutionInfos[(int)synchroLevel].color, fadeTime);
    }

    public override void AddResult()
    {
        if (onSynchro)
        {
            onSynchro = false;
        }
        else
        {
            observer.AddResult();
            onSynchro = true;
        }
        observer.ResetObserver();
    }

    public override int AddCleanupPoint(EventButton Event)
    {
        if (synchroPoint == 0)
        {
            synchroPoint = HelperFunction.RandomPointRange(GlobalInfo.instance.pollutionInfos[(int)level].pointRange);
            return synchroPoint;
        }
        else
        {
            ShowCleanupView(synchroPoint, Event);
            Player.instance.AddPoint(synchroPoint);
            int retPoint = synchroPoint;
            synchroPoint = 0;
            return retPoint;
        }
    }
}
