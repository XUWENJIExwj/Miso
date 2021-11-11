using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventScriptableObject;
using System;

public enum ExpressionTypes
{
    Normal,    // （普段）
    Curious,   // （疑）
    Surprised, // （驚）
    Confused,  // （困）
    Laughing,  // （笑）
    Angry,     // （怒）
    Crafty,    // （悪）
    None,
}

public enum CharacterTypes
{
    Player,
    NPC,
    Aside,
    AMA_Higashi,    // 東シナ
    AMA_Gibraltar,  // ジブラルタル
    AMA_Arctic,     // 北極
    AMA_Pirinesia,  // ピリネシア Polynesia
    AMA_Panama,     // パナマ
    AMA_Mozambique, // モザンビーク
    None,
}

[Serializable]
public struct StringToExpressionType
{
    public string expressionString;
    public ExpressionTypes expressionType;
}

[Serializable]
public struct ExpressionDictionaryRef
{
    public ExpressionTypes type;
    public Sprite sprite;
}

[Serializable]
public struct CharacterDictionaryRef
{
    public CharacterTypes type;
    public ExpressionDictionaryRef[] expressions;
}

public class IllustrationDictionary : MonoBehaviour
{
    [SerializeField] private StringToExpressionType[] stringToExpressionTypes;
    [SerializeField] private CharacterDictionaryRef[] characters;
    [SerializeField] private Dictionary<string, ExpressionTypes> stringToExpressionDictionary;
    [SerializeField] private Dictionary<ExpressionTypes, Sprite> expressionDictionary;
    [SerializeField] private Dictionary<CharacterTypes, Dictionary<ExpressionTypes, Sprite>> characterDictionary;

    public void MakeDictionary()
    {
        stringToExpressionDictionary = new Dictionary<string, ExpressionTypes>();
        foreach (StringToExpressionType stringToExpressionType in stringToExpressionTypes)
        {
            stringToExpressionDictionary.Add(stringToExpressionType.expressionString, stringToExpressionType.expressionType);
        }

        characterDictionary = new Dictionary<CharacterTypes, Dictionary<ExpressionTypes, Sprite>>();
        foreach (CharacterDictionaryRef character in characters)
        {
            expressionDictionary = new Dictionary<ExpressionTypes, Sprite>();
            foreach (ExpressionDictionaryRef expression in character.expressions)
            {
                expressionDictionary.Add(expression.type, expression.sprite);
            }
            characterDictionary.Add(character.type, expressionDictionary);
        }
    }

    private CharacterTypes CheckCharacterType(MainEventCharacterTypes CharacterType)
    {
        if (CharacterType == MainEventCharacterTypes.AMA)
        {
            return (int)Player.instance.GetCurrentAMAType() + CharacterTypes.AMA_Higashi;
        }
        else if (CharacterType == MainEventCharacterTypes.None)
        {
            return CharacterTypes.None;
        }
        return (CharacterTypes)CharacterType;
    }

    private Sprite CheckExpressionType(CharacterTypes CharacterType, ExpressionTypes ExpressionType)
    {
        if (!characterDictionary[CharacterType].ContainsKey(ExpressionType))
        {
            Debug.Log("指定の表情が辞書に存在しない");
            return null;
        }

        return characterDictionary[CharacterType][ExpressionType];
    }

    public Sprite GetTargetIllustration(MainEventCharacterTypes CharacterType, ExpressionTypes ExpressionType)
    {
        CharacterTypes type = CheckCharacterType(CharacterType);
        return CheckExpressionType(type, ExpressionType);
    }

    public Sprite GetTargetIllustration(MainEventCharacterTypes CharacterType, string ExpressionType)
    {
        ExpressionTypes expressionType = StringToExpressionType(ExpressionType);
        return GetTargetIllustration(CharacterType, expressionType);
    }

    public ExpressionTypes StringToExpressionType(string ExpressionType)
    {
        if (!stringToExpressionDictionary.ContainsKey(ExpressionType))
        {
            Debug.Log("指定の表情文字列が辞書に存在しない");
            return ExpressionTypes.None;
        }
        return stringToExpressionDictionary[ExpressionType];
    }
}
