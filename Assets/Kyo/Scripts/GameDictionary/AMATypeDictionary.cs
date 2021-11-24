using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public struct AMATypeDictionaryRef
{
    public AMATypes type;
    public string typeString;
}

public class AMATypeDictionary : MonoBehaviour
{
    [SerializeField] private AMATypeDictionaryRef[] amaTypes;
    [SerializeField] private Dictionary<AMATypes, string> amaTypeDictionary = null;

    public void MakeDictionary()
    {
        amaTypeDictionary = new Dictionary<AMATypes, string>();
        foreach(AMATypeDictionaryRef amaType in amaTypes)
        {
            amaTypeDictionary.Add(amaType.type, amaType.typeString);
        }
    }

    public string GetAMAType(AMATypes AMAType)
    {
        if (!amaTypeDictionary.ContainsKey(AMAType))
        {
            Debug.Log("éwíËÇÃAMATypeÇÕé´èëÇ…ë∂ç›ÇµÇ»Ç¢");
            return null;
        }
        return amaTypeDictionary[AMAType];
    }
}
