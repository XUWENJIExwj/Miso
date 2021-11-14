using UnityEngine;
using UnityEngine.UI;

public class NumberDisplay : MonoBehaviour
{
    [SerializeField] protected Sprite[] numSprites = null;
    [SerializeField] protected Image prefab = null;
    [SerializeField] protected Image[] digits = null;
    [SerializeField] protected float sizeRatio = 0.8f;
    [SerializeField] protected int prevValue = 0;

    public void Init()
    {
        RectTransform[] rectTransforms = new RectTransform[digits.Length];
        float interval = 0.0f;
        for (int i = 0; i < digits.Length; ++i)
        {
            digits[i] = Instantiate(prefab, transform);
            digits[i].SetNativeSize();

            rectTransforms[i] = digits[i].GetComponent<RectTransform>();
            rectTransforms[i].sizeDelta *= sizeRatio;
            interval = Mathf.Max(interval, rectTransforms[i].sizeDelta.x);
        }

        interval *= sizeRatio;
        for (int i = 0; i < digits.Length; ++i)
        {
            rectTransforms[i].localPosition = new Vector3(
                rectTransforms[i].localPosition.x + ((digits.Length - 1) * 0.5f - i) * interval,
                rectTransforms[i].localPosition.y,
                rectTransforms[i].localPosition.z);
        }
    }

    public virtual void SetValue(int Value)
    {
        for (int i = 0; i < digits.Length; ++i)
        {
            digits[i].sprite = numSprites[Value % 10];
            digits[i].SetNativeSize();
            digits[i].GetComponent<RectTransform>().sizeDelta *= sizeRatio;
            Value /= 10;
        }
    }

    public void HideNumber()
    {
        foreach(Image digit in digits)
        {
            digit.color = HelperFunction.ChangeAlpha(digit.color, 0.0f);
        }
    }

    public void ShowNumber()
    {
        foreach (Image digit in digits)
        {
            digit.color = HelperFunction.ChangeAlpha(digit.color, 1.0f);
        }
    }
}
