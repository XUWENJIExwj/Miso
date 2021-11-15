using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventScriptableObject;
using DG.Tweening;

public class RandomResult : Result
{
    [SerializeField] private ResultObject result = null;

    public override void Init(EventSO Event)
    {
        RandomEventSO eventSO = (RandomEventSO)Event;
        result.eventName.text = eventSO.eventTitle;
        result.resultFrame.color = HelperFunction.ChangeAlpha(result.resultFrame.color, 0.0f);
        result.eventName.color = HelperFunction.ChangeAlpha(result.eventName.color, 0.0f);

        point = eventSO.point;
    }

    public override void Appear()
    {
        tweener = result.resultFrame.DOFade(1.0f, fadeTime);
        tweener.OnUpdate(() => { result.eventName.color = HelperFunction.ChangeAlpha(result.eventName.color, result.resultFrame.color.a); });
        tweener.OnComplete(() =>
        {
            result.eventName.color = HelperFunction.ChangeAlpha(result.eventName.color, result.resultFrame.color.a);
            AppearNext();
        });
    }

    public override void Show()
    {
        KillTweener();

        result.resultFrame.color = HelperFunction.ChangeAlpha(result.resultFrame.color, 1.0f);
        result.eventName.color = HelperFunction.ChangeAlpha(result.eventName.color, 1.0f);
    }
}
