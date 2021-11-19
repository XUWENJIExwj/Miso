using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Oceans
{
    ArcticOcean,
    PacificOcean,
    AtlanticOcean,
    IndianOcean,
    None,
}

public class PollutionMap : Monosingleton<PollutionMap>
{
    [SerializeField] private OceanParts[] parts = null;

    public override void InitAwake()
    {
        
    }

    public void Init()
    {
        foreach (OceanParts part in parts)
        {
            part.Init();
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
