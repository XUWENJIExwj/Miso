using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using EventScriptableObject;

[Serializable]
public struct JudgementRef
{
    public JudgementType type;
    public string judgement;
}

public class MainResultDictionary : MonoBehaviour
{
    [SerializeField] private JudgementRef[] judgements;
    [SerializeField] private Dictionary<JudgementType, string> mainResultDictionary = null;

    public void MakeDictionnary()
    {
        mainResultDictionary = new Dictionary<JudgementType, string>();

        foreach (JudgementRef judgement in judgements)
        {
            mainResultDictionary.Add(judgement.type, judgement.judgement);
        }
    }

    public string GetJudgement(JudgementType Judgement)
    {
        return  " (" + mainResultDictionary[Judgement] + ")";
    }
}
