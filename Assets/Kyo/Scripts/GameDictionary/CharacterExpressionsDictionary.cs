using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public struct ExpressionDictionaryRef
{
    public ExpressionTypes type;
    public Sprite sprite;
}

public class CharacterExpressionsDictionary : MonoBehaviour
{
    [SerializeField] private ExpressionDictionaryRef[] expressions;
    [SerializeField] private Dictionary<ExpressionTypes, Sprite> expressionDictionary = null;

    public void MakeDictionnary()
    {
        expressionDictionary = new Dictionary<ExpressionTypes, Sprite>();
        foreach (ExpressionDictionaryRef expression in expressions)
        {
            expressionDictionary.Add(expression.type, expression.sprite);
        }
    }

    public Sprite GetSprite(ExpressionTypes ExpressionType)
    {
        if (!expressionDictionary.ContainsKey(ExpressionType))
        {
            Debug.Log("éwíËÇÃï\èÓÇ™é´èëÇ…ë∂ç›ÇµÇ»Ç¢");
            return null;
        }

        return expressionDictionary[ExpressionType];
    }
}
