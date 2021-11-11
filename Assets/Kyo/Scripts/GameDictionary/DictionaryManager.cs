using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventScriptableObject;

public class DictionaryManager : Monosingleton<DictionaryManager>
{
    [SerializeField] private IllustrationDictionary illustrationDictionary = null;
    [SerializeField] private AMATypeDictionary amaTypeDictionary = null;

    public override void InitAwake()
    {
        illustrationDictionary.MakeDictionary();
        amaTypeDictionary.MakeDictionary();
    }

    // IllustrationDictionary
    public Sprite GetTargetIllustration(MainEventCharacterTypes CharacterType, ExpressionTypes ExpressionType)
    {
        return illustrationDictionary.GetTargetIllustration(CharacterType, ExpressionType);
    }

    public Sprite GetTargetIllustration(MainEventCharacterTypes CharacterType, string ExpressionType)
    {
        return illustrationDictionary.GetTargetIllustration(CharacterType, ExpressionType);
    }

    // AMATypeDictionary
    public string GetAMAType(AMATypes AMAType)
    {
        return amaTypeDictionary.GetAMAType(AMAType);
    }
}
