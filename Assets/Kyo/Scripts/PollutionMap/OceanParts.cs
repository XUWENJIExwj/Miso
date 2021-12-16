using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

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

public struct OceanPartsData
{
    public OceanPartData[] datas;

    public void Init(int Count)
    {
        datas = new OceanPartData[Count];

        for (int i = 0; i < datas.Length; ++i)
        {
            datas[i].Init();
        }
    }
}

public class OceanParts : MonoBehaviour
{
    [SerializeField] protected OceanPart[] parts = null;
    [SerializeField] protected OceanPartsData oceanPartsData;

    public void Init(Oceans Ocean)
    {
        parts = GetComponentsInChildren<OceanPart>();

        for (int i = 0; i < parts.Length; ++i)
        {
            parts[i].Init(Ocean, (OceanAreas)i);
        }

        oceanPartsData.Init(parts.Length);
    }

    public void Load(Oceans Ocean, OceanPartsData Data)
    {
        parts = GetComponentsInChildren<OceanPart>();

        for (int i = 0; i < parts.Length; ++i)
        {
            parts[i].Load(Ocean, (OceanAreas)i, Data.datas[i]);
        }

        oceanPartsData.Init(parts.Length);
    }

    public void ResetPollutionLevel()
    {
        for (int i = 0; i < parts.Length; ++i)
        {
            parts[i].ResetPollutionLevel();
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

    public OceanPart[] GetOceanPart()
    {
        return parts;
    }

    public OceanPartsData GetOceanPartsData()
    {
        for (int i = 0; i < parts.Length; ++i)
        {
            oceanPartsData.datas[i] = parts[i].GetOceanPartData();
        }
        return oceanPartsData;
    }
}
