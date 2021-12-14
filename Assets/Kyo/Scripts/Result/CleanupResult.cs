using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CleanupResult : Result
{
    [SerializeField] private ResultObject result = null;

    public override void Init(CleanupObserver Observer)
    {
        result.eventName.SetText(Observer.CleanupResultText());
        result.resultFrame.color = HelperFunction.ChangeAlpha(result.resultFrame.color, 0.0f);
        result.eventName.SetAlpha(0.0f);

        point = Observer.GetPoint();
    }

    public override void Appear()
    {
        tweener = result.resultFrame.DOFade(1.0f, fadeTime);
        tweener.OnUpdate(() => { result.eventName.SetAlpha(result.resultFrame.color.a); });
        tweener.OnComplete(() =>
        {
            result.eventName.SetAlpha(result.resultFrame.color.a);
            AppearNext();
        });
    }

    public override void Show()
    {
        KillTweener();

        result.resultFrame.color = HelperFunction.ChangeAlpha(result.resultFrame.color, 1.0f);
        result.eventName.SetAlpha(1.0f);
    }
}
