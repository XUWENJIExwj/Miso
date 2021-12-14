using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using EventScriptableObject;
using DG.Tweening;

[Serializable]
public class MainResultObject : ResultObject
{
    public Image endingFrame = null;
    public TextOutline endingText = null;
}

public class MainResult : Result
{
    [SerializeField] private MainResultObject result = null;

    public override void Init(EventSO Event)
    {
        MainEventSO eventSO = (MainEventSO)Event;
        result.eventName.SetText(eventSO.eventTitle + DictionaryManager.instance.GetJudgement(eventSO.GetJudgement()));
        result.endingText.SetText(eventSO.GetEnding());
        result.resultFrame.color = HelperFunction.ChangeAlpha(result.resultFrame.color, 0.0f);
        result.eventName.SetAlpha(0.0f);
        result.endingFrame.color = HelperFunction.ChangeAlpha(result.endingFrame.color, 0.0f);
        result.endingText.SetAlpha(0.0f);

        point = eventSO.point;
    }

    public override void Appear()
    {
        tweener = result.resultFrame.DOFade(1.0f, fadeTime);
        tweener.OnUpdate(() => { result.eventName.SetAlpha(result.resultFrame.color.a); });
        tweener.OnComplete(() =>
        {
            result.eventName.SetAlpha(result.resultFrame.color.a);

            tweener = result.endingFrame.DOFade(1.0f, fadeTime);
            tweener.OnUpdate(() => { result.endingText.SetAlpha(result.endingFrame.color.a); });
            tweener.OnComplete(() =>
            {
                result.endingText.SetAlpha(result.endingFrame.color.a);
                AppearNext();
            });
        });
    }

    public override void Show()
    {
        KillTweener();

        result.resultFrame.color = HelperFunction.ChangeAlpha(result.resultFrame.color, 1.0f);
        result.eventName.SetAlpha(1.0f);
        result.endingFrame.color = HelperFunction.ChangeAlpha(result.endingFrame.color, 1.0f);
        result.endingText.SetAlpha(1.0f);
    }
}
