using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventScriptableObject;
using System;

public enum ExpressionTypes
{
    Normal,    // �i���i�j
    Curious,   // �i�^�j
    Surprised, // �i���j
    Confused,  // �i���j
    Laughing,  // �i�΁j
    Angry,     // �i�{�j
    Crafty,    // �i���j
    None,
}

public enum CharacterTypes
{
    Player,
    NPC,
    Aside,
    AMA_Higashi,    // ���V�i
    AMA_Gibraltar,  // �W�u�����^��
    AMA_Arctic,     // �k��
    AMA_Polynesia,  // �|���l�V�A
    AMA_Panama,     // �p�i�}
    AMA_Mozambique, // ���U���r�[�N
    None,
}

[Serializable]
public struct StringToExpressionType
{
    public string expressionString;
    public ExpressionTypes expressionType;
}

[Serializable]
public struct CharacterRef
{
    public CharacterTypes type;
    public CharacterExpressionsDictionary expressions;
}

public class AllCharactersExpressionsDictionary : MonoBehaviour
{
    [SerializeField] private StringToExpressionType[] stringToExpressionTypes;
    [SerializeField] private Dictionary<string, ExpressionTypes> stringToExpressionDictionary = null;
    [SerializeField] private CharacterRef[] characters = null;
    [SerializeField] private Dictionary<CharacterTypes, CharacterExpressionsDictionary> characterDictionary = null;

    public void MakeDictionary()
    {
        stringToExpressionDictionary = new Dictionary<string, ExpressionTypes>();
        foreach (StringToExpressionType stringToExpressionType in stringToExpressionTypes)
        {
            stringToExpressionDictionary.Add(stringToExpressionType.expressionString, stringToExpressionType.expressionType);
        }

        characterDictionary = new Dictionary<CharacterTypes, CharacterExpressionsDictionary>();
        foreach (CharacterRef character in characters)
        {
            character.expressions.MakeDictionnary();
            characterDictionary.Add(character.type, character.expressions);
        }
    }

    public ExpressionTypes StringToExpressionType(string ExpressionType)
    {
        if (!stringToExpressionDictionary.ContainsKey(ExpressionType))
        {
            Debug.Log("�w��̕\����񂪎����ɑ��݂��Ȃ�");
            return ExpressionTypes.None;
        }
        return stringToExpressionDictionary[ExpressionType];
    }

    private CharacterTypes CheckCharacterType(MainEventCharacterTypes CharacterType)
    {
        if (CharacterType == MainEventCharacterTypes.AMA)
        {
            return (int)Player.instance.GetCurrentAMA() + CharacterTypes.AMA_Higashi;
        }
        else if (CharacterType == MainEventCharacterTypes.None)
        {
            return CharacterTypes.None;
        }
        return (CharacterTypes)CharacterType;
    }

    public Sprite GetSprite(CharacterTypes CharacterType, ExpressionTypes ExpressionType)
    {
        return characterDictionary[CharacterType].GetSprite(ExpressionType);
    }

    public Sprite GetTargetSprite(MainEventCharacterTypes CharacterType, ExpressionTypes ExpressionType)
    {
        CharacterTypes type = CheckCharacterType(CharacterType);
        return GetSprite(type, ExpressionType);
    }

    public Sprite GetTargetSprite(MainEventCharacterTypes CharacterType, string ExpressionType)
    {
        ExpressionTypes expressionType = StringToExpressionType(ExpressionType);
        return GetTargetSprite(CharacterType, expressionType);
    }
}
