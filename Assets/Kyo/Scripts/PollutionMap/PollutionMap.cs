using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using EventScriptableObject;

public enum Oceans
{
    ArcticOcean,
    PacificOcean,
    AtlanticOcean,
    IndianOcean,
    None,
}

public enum PollutionLevel
{
    Level_00,
    Level_01,
    Level_02,
    Level_03,
    Level_04,
    Level_05,
    None,
}

[Serializable]
public struct PollutionLevelInfo
{
    public Color color;
    public PointRange pointRange;
}

[Serializable]
public struct PollutionMapData
{
    public OceanPartsData[] datas;

    public void Init(int Count)
    {
        datas = new OceanPartsData[Count];
    }
}

public class PollutionMap : Monosingleton<PollutionMap>
{
    [SerializeField] private OceanParts[] parts = null;
    [SerializeField] private PollutionMapData pollutionMapData;

    public void Init()
    {
        pollutionMapData.Init(parts.Length);

        for (int i = 0; i < parts.Length; ++i)
        {
            parts[i].Init((Oceans)i);
        }

        Player.instance.SetPollutionMapData(pollutionMapData);
    }

    public void Load(PollutionMapData Data)
    {
        if (Data.datas != null)
        {
            for (int i = 0; i < parts.Length; ++i)
            {
                parts[i].Load((Oceans)i, Data.datas[i]);
            }

            pollutionMapData = Data;
        }
        else
        {
            Init();
        }
    }

    public void ResetPollutionLevel()
    {
        for (int i = 0; i < parts.Length; ++i)
        {
            parts[i].ResetPollutionLevel();
        }
    }

    public void SetPollutionLevel(Oceans Ocean, OceanAreas Area, EventButton Event)
    {
        parts[(int)Ocean].SetPollutionLevel(Area, Event);
    }

    public void AddResult()
    {
        foreach (OceanParts part in parts)
        {
            part.AddResult();
        }
    }

    public void Move(Vector2 Offset)
    {
        for (int i = 0; i < parts.Length; ++i)
        {
            parts[i].Move( Offset);
        }
    }

    public void MovePath(Vector2 Offset, float Time)
    {
        for (int i = 0; i < parts.Length; ++i)
        {
            parts[i].MovePath(Offset, Time);
        }
    }

    public PollutionMapData GetPollutionMapData()
    {
        for (int i = 0; i < parts.Length; ++i)
        {
            pollutionMapData.datas[i] = parts[i].GetOceanPartsData();
        }
        return pollutionMapData;
    }
}
