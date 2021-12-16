using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class FuelGauge : Monosingleton<FuelGauge>
{
    [SerializeField] private Image bg = null;
    [SerializeField] private Image gauge = null;
    [SerializeField] private Text valueText = null;
    [SerializeField] private int currentValue = 0;
    [SerializeField] private int targetValue = 0;
    [SerializeField] private float valueInterval = 0.02f;
    [SerializeField] private float fadeTime = 0.4f;

    private Tweener tweener = null;
    private Coroutine coroutine = null;

    public void Init()
    {
        gameObject.SetActive(false);
    }

    public void ActiveFuelGauge(int Value)
    {
        gameObject.SetActive(true);
        ResetValues(Value);
    }

    public void ResetValues(int Value)
    {
        targetValue = Value;
        currentValue = Value;
        UpdateFuelGauge();
        KillTweener();
    }

    public void ResetValuesWithAnimation(int Value)
    {
        targetValue = Value;
        UpdateValues(0);
    }

    public void AddValueOnRouteSelect(int Value = 1)
    {
        UpdateValues(Value);
    }

    public void ReduceValueOnRouteSelect(int Value = -1)
    {
        UpdateValues(Value);

        if (!tweener.IsActive()) GaugeBreathing(0.5f, 1.0f);
    }

    public void ReduceValueOnMove()
    {
        UpdateValues(-1);
        KillTweener();
    }

    private void UpdateValues(int Value)
    {
        targetValue += Value;

        if (coroutine == null)
        {
            //gameObject.SetActive(true);
            coroutine = StartCoroutine(AnimateFuelGauge());
        }
    }

    private IEnumerator AnimateFuelGauge()
    { 
        while (currentValue != targetValue)
        {
            if (currentValue < targetValue)
            {
                ++currentValue;
            }
            else
            {
                --currentValue;
            }
            UpdateFuelGauge();
            yield return new WaitForSeconds(valueInterval);
        }
        coroutine = null;

        if (IsMaxGauge()) KillTweener();
    }

    public void UpdateFuelGauge()
    {
        UpdateCurrentValue();
        UpdateFillAmount();
    }

    public void UpdateCurrentValue()
    {
        valueText.text = currentValue.ToString() + "/" + Player.instance.GetCurrentAMAEnergy().ToString();
    }

    public void UpdateFillAmount()
    {
        gauge.fillAmount = (float)currentValue / Player.instance.GetCurrentAMAEnergy();
    }

    public bool IsMaxGauge()
    {
        return currentValue == Player.instance.GetCurrentAMAEnergy();
    }

    public void KillTweener()
    {
        if (tweener.IsActive())
        {
            tweener.Kill();
            tweener = null;
            gauge.color = HelperFunction.ChangeAlpha(gauge.color, 1.0f);
        }
    }

    private Tweener GaugeAlphaFade(float Alpha)
    {
        return gauge.DOFade(Alpha, fadeTime);
    }

    public void GaugeBreathing(float AlphaA, float AlphaB)
    {
        tweener = GaugeAlphaFade(AlphaA);
        tweener.OnComplete(() =>
        {
            tweener = GaugeAlphaFade(AlphaB);
            tweener.OnComplete(() => GaugeBreathing(AlphaA, AlphaB));
        });
    }

    public bool NoMoreFuel()
    {
        if (currentValue == 0)
        {
            Debug.Log("”R—¿•s‘«");
            return true;
        }
        return false;
    }

    public void ShowFuelGauge()
    {
        bg.color = HelperFunction.ChangeAlpha(bg.color, 1.0f);
        gauge.color = HelperFunction.ChangeAlpha(gauge.color, 1.0f);
        valueText.color = HelperFunction.ChangeAlpha(valueText.color, 1.0f);
    }

    public void HideFuelGauge()
    {
        bg.color = HelperFunction.ChangeAlpha(bg.color, 0.0f);
        gauge.color = HelperFunction.ChangeAlpha(gauge.color, 0.0f);
        valueText.color = HelperFunction.ChangeAlpha(valueText.color, 0.0f);
    }
}
