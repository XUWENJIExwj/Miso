using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanupObserver : MonoBehaviour
{
    [SerializeField] private Oceans ocean = Oceans.None;
    [SerializeField] private OceanAreas area = OceanAreas.None;
    [SerializeField] private PollutionLevel startLevel = PollutionLevel.Level_05;
    [SerializeField] private PollutionLevel endLevel = PollutionLevel.Level_05;
    [SerializeField] private bool hasCleaned = false;
    [SerializeField] private int point = 0;

    public void Init(Oceans Ocean, OceanAreas Area)
    {
        ocean = Ocean;
        area = Area;
        startLevel = PollutionLevel.Level_05;
        endLevel = PollutionLevel.Level_05;
        hasCleaned = false;
        point = 0;
    }

    public void AddResult()
    {
        if (hasCleaned)
        {
            EventUIManager.instance.AddResult(this);
        }
    }

    public void ListenCleanup(PollutionLevel Level, int Point)
    {
        if (!hasCleaned)
        {
            hasCleaned = true;
            startLevel = Level + 1;
            endLevel = Level;
            point += Point;
        }
        else
        {
            endLevel = Level;
            point += Point;
        }
    }

    public string CleanupResultText()
    {
        string result =
            DictionaryManager.instance.GetOcean(ocean) + " " +
            DictionaryManager.instance.GetOceanArea(area) + " ÇÃèÚâª: \n" +
            DictionaryManager.instance.GetPollutionLevel(startLevel) + " Å® " +
            DictionaryManager.instance.GetPollutionLevel(endLevel);
        return result;
    }

    public int GetPoint()
    {
        return point;
    }

    public void ResetObserver()
    {
        hasCleaned = false;
        point = 0;
    }
}
