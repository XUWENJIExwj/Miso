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

public class PollutionMap : Monosingleton<PollutionMap>
{
    [SerializeField] private OceanParts[] parts = null;

    public override void InitAwake()
    {
        
    }

    public void Init()
    {
        for (int i = 0; i < parts.Length; ++i)
        {
            parts[i].Init((Oceans)i);
        }
    }

    public void Load(PollutionLevel Level)
    {
        for (int i = 0; i < parts.Length; ++i)
        {
            parts[i].Load((Oceans)i, Level);
        }
    }

    public void ResetPollutionLevel()
    {
        foreach (OceanParts part in parts)
        {
            part.ResetPollutionLevel();
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
        foreach (OceanParts part in parts)
        {
            part.Move(Offset);
        }
    }

    public void MovePath(Vector2 Offset, float Time)
    {
        foreach (OceanParts part in parts)
        {
            part.MovePath(Offset, Time);
        }
    }
}
