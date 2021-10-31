using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventScriptableObject;
using System;

[Serializable]
public struct ExpressionDictionaryRef
{
    public string type;
    public Sprite sprite;
}

[Serializable]
public struct CharacterDictionaryRef
{
    public CharacterTypes type;
    public ExpressionDictionaryRef[] expressions;
}

public class IllustrationDictionary : Monosingleton<IllustrationDictionary>
{
    [SerializeField] private CharacterDictionaryRef[] characters;
    [SerializeField] private Dictionary<string, Sprite> expressionDictionary;
    [SerializeField] private Dictionary<CharacterTypes, Dictionary<string, Sprite>> characterDictionary;

    public override void InitAwake()
    {
        characterDictionary = new Dictionary<CharacterTypes, Dictionary<string, Sprite>>();

        MakeDictionary();
    }

    private void MakeDictionary()
    {
        foreach(CharacterDictionaryRef character in characters)
        {
            expressionDictionary = new Dictionary<string, Sprite>();
            foreach (ExpressionDictionaryRef expression in character.expressions)
            {
                expressionDictionary.Add(expression.type, expression.sprite);
            }
            characterDictionary.Add(character.type, expressionDictionary);
        }
    }

    public Sprite GetTargetIllustration(CharacterTypes CharacterType, string ExpressionType)
    {
        if (!characterDictionary.ContainsKey(CharacterType))
        {
            Debug.Log("éwíËÇÃCharacterÇ™é´èëÇ…ë∂ç›ÇµÇ»Ç¢");
            return null;
        }

        if (!characterDictionary[CharacterType].ContainsKey(ExpressionType))
        {
            Debug.Log("éwíËÇÃï\èÓÇ™é´èëÇ…ë∂ç›ÇµÇ»Ç¢");
            return null;
        }

        return characterDictionary[CharacterType][ExpressionType];
    }
}
