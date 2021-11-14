using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : Monosingleton<Timer>
{
    [SerializeField] private TMP_Text timeText = null;
    [SerializeField] private NumberDisplay numberDisplay = null;

    public void Init()
    {
        numberDisplay.Init();
        gameObject.SetActive(false);
    }

    public void ActiveTimer(int Time)
    {
        gameObject.SetActive(true);
        numberDisplay.SetValue(Time);
    }

    public void ResetTimer(int Time)
    {
        numberDisplay.SetValue(Time);
    }

    public void StartTimer(int Time)
    {
        StartCoroutine(CountDown(Time));
    }

    private IEnumerator CountDown(int Time)
    {
        for (int time = Time - 1; time >= 0; --time)
        {
            numberDisplay.SetValue(time);
            yield return new WaitForSeconds(1.0f);
        }
    }

    public void ShowTimer()
    {
        timeText.color = HelperFunction.ChangeAlpha(timeText.color, 1.0f);
        numberDisplay.ShowNumber();
    }

    public void HideTimer()
    {
        timeText.color = HelperFunction.ChangeAlpha(timeText.color, 0.0f);
        numberDisplay.HideNumber();
    }
}
