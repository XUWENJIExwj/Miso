using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventScriptableObject;

public class DictionaryManager : Monosingleton<DictionaryManager>
{
    [SerializeField] private AllCharactersExpressionsDictionary illustrationDictionary = null;
    [SerializeField] private AMATypeDictionary amaTypeDictionary = null;
    [SerializeField] private OceanDictionary oceanDictionary = null;
    [SerializeField] private MainResultDictionary mainResultDictionary = null;

    public override void InitAwake()
    {
        illustrationDictionary.MakeDictionary();
        amaTypeDictionary.MakeDictionary();
        oceanDictionary.MakeDictionnary();
        mainResultDictionary.MakeDictionnary();
    }

    // IllustrationDictionary
    public Sprite GetTargetIllustration(CharacterTypes CharacterType, ExpressionTypes ExpressionType)
    {
        return illustrationDictionary.GetSprite(CharacterType, ExpressionType);
    }

    public Sprite GetTargetIllustration(MainEventCharacterTypes CharacterType, ExpressionTypes ExpressionType)
    {
        return illustrationDictionary.GetTargetSprite(CharacterType, ExpressionType);
    }

    public Sprite GetTargetIllustration(MainEventCharacterTypes CharacterType, string ExpressionType)
    {
        return illustrationDictionary.GetTargetSprite(CharacterType, ExpressionType);
    }

    // AMATypeDictionary
    public string GetAMAType(AMATypes AMAType)
    {
        return amaTypeDictionary.GetAMAType(AMAType);
    }

    // OceanDictionary
    public string GetOcean(Oceans Ocean)
    {
        return oceanDictionary.GetOcean(Ocean);
    }

    public string GetOceanArea(OceanAreas Area)
    {
        return oceanDictionary.GetOceanArea(Area);
    }

    public string GetPollutionLevel(PollutionLevel Level)
    {
        return oceanDictionary.GetPollutionLevel(Level);
    }

    // MainResultDictionary
    public string GetJudgement(JudgementType Judgement)
    {
        return mainResultDictionary.GetJudgement(Judgement);
    }
}
