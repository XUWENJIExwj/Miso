using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public struct OceanRef
{
    public Oceans type;
    public string name;
}

[Serializable]
public struct OceanAreaRef
{
    public OceanAreas type;
    public string name;
}

[Serializable]
public struct PollutionLevelRef
{
    public PollutionLevel level;
    public string levelText;
}

public class OceanDictionary : MonoBehaviour
{
    [SerializeField] private OceanRef[] oceans;
    [SerializeField] private Dictionary<Oceans, string> oceanDictionary = null;
    [SerializeField] private OceanAreaRef[] areas;
    [SerializeField] private Dictionary<OceanAreas, string> oceanAreaDictionary = null;
    [SerializeField] private PollutionLevelRef[] levels;
    [SerializeField] private Dictionary<PollutionLevel, string> pollutionLevelDictionary = null;

    public void MakeDictionnary()
    {
        oceanDictionary = new Dictionary<Oceans, string>();
        foreach (OceanRef ocean in oceans)
        {
            oceanDictionary.Add(ocean.type, ocean.name);
        }

        oceanAreaDictionary = new Dictionary<OceanAreas, string>();
        foreach (OceanAreaRef area in areas)
        {
            oceanAreaDictionary.Add(area.type, area.name);
        }

        pollutionLevelDictionary = new Dictionary<PollutionLevel, string>();
        foreach (PollutionLevelRef level in levels)
        {
            pollutionLevelDictionary.Add(level.level, level.levelText);
        }
    }

    public string GetOcean(Oceans Ocean)
    {
        return oceanDictionary[Ocean];
    }

    public string GetOceanArea(OceanAreas Area)
    {
        return oceanAreaDictionary[Area];
    }

    public string GetPollutionLevel(PollutionLevel Level)
    {
        return pollutionLevelDictionary[Level];
    }
}
