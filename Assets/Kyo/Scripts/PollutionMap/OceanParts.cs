using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public enum OceanAreas
{
    Area_01,
    Area_02,
    Area_03,
    Area_04,
    Area_05,
    Area_06,
    Area_07,
    Area_08,
    Area_09,
    None,
}

public class OceanParts : MonoBehaviour
{
    [SerializeField] protected OceanPart[] parts = null;

    public void Init(Oceans Ocean)
    {
        parts = GetComponentsInChildren<OceanPart>();

        for (int i = 0; i < parts.Length; ++i)
        {
            parts[i].Init(Ocean, (OceanAreas)i);
        }
    }

    public void ResetPollutionLevel()
    {
        foreach (OceanPart part in parts)
        {
            part.ResetPollutionLevel();
        }
    }

    public virtual void SetPollutionLevel(OceanAreas Area, EventButton Event)
    {
        parts[(int)Area].SetPollutionLevel(Event);
    }

    public void AddResult()
    {
        foreach (OceanPart part in parts)
        {
            part.AddResult();
        }
    }

    public void Move(Vector2 Offset)
    {
        foreach (OceanPart part in parts)
        {
            part.Move(Offset);
        }
    }

    public void MovePath(Vector2 Offset, float Time)
    {
        foreach (OceanPart part in parts)
        {
            part.MovePath(Offset, Time);
        }
    }
}
