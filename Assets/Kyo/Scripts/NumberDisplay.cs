<<<<<<< .merge_file_a08508
using System.Collections;
using System.Collections.Generic;
=======
>>>>>>> .merge_file_a12220
using UnityEngine;
using UnityEngine.UI;

public class NumberDisplay : MonoBehaviour
{
    [SerializeField] protected Sprite[] numSprites = null;
    [SerializeField] protected Image[] digits = null;
    [SerializeField] protected int prevValue = 0;

    public virtual void SetValue(int Value)
    {
        for (int i = 0; i < digits.Length; ++i)
        {
            digits[i].sprite = numSprites[Value % 10];
            digits[i].SetNativeSize();
            Value /= 10;
        }
    }
}
