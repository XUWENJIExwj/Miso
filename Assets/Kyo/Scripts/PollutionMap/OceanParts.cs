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

    public void Init()
    {
        parts = GetComponentsInChildren<OceanPart>();

        foreach (OceanPart part in parts)
        {
            part.Init();
        }
    }

    public virtual void SetPollutionLevel(OceanAreas Area, EventButton Event)
    {
        parts[(int)Area].SetPollutionLevel(Event);
    }

    public void Move(Vector2 Offset)
    {
        for (int i = 0; i < parts.Length; ++i)
        {
            parts[i].Move(Offset);
        }
    }

    public void MovePath(Vector2 Offset, float Time)
    {
        for (int i = 0; i < parts.Length; ++i)
        {
            parts[i].MovePath(Offset, Time);
        }
    }
}
