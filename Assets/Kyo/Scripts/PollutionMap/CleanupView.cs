using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;

[Serializable]
public struct CleanupViewUIElement
{
    public Image frame;
    public TextOutline title;
    public TextOutline point;
}

public class CleanupView : MonoBehaviour
{
    [SerializeField] private CleanupViewUIElement ui;
    [SerializeField] private float fadeInTime = 0.5f;
    [SerializeField] private float fadeOutTime = 1.0f;
    [SerializeField] private Vector3 startOffset = Vector3.zero;
    [SerializeField] private Vector3 endOffset = Vector3.zero;

    public void ShowCleanupView(int Point, EventButton Event)
    {
        SoundManager.instance.SE_Clean();

        transform.localPosition = Event.transform.localPosition + startOffset;

        ui.frame.color = HelperFunction.ChangeAlpha(ui.frame.color, 0.0f);
        ui.title.SetAlpha(0.0f);
        ui.title.SetText("ƒGƒŠƒAò‰»");
        ui.point.SetAlpha(0.0f);
        ui.point.SetText("Point: " + Point.ToString("+#;-#;0"));

        Sequence sequence = DOTween.Sequence();
        sequence.Append(AnimateCleanupView(50.0f / 255.0f, fadeInTime));
        sequence.Append(AnimateCleanupView(0.0f, fadeOutTime));
        sequence.Insert(0.0f, transform.DOLocalMove(transform.localPosition + endOffset, fadeInTime + fadeOutTime).SetEase(Ease.Linear));
        sequence.OnComplete(() => { Destroy(gameObject); });
    }

    public Tweener AnimateCleanupView(float TargetAlpha, float Time)
    {
        Tweener tweener = ui.frame.DOFade(TargetAlpha, Time);
        tweener.SetEase(Ease.Linear);
        tweener.OnUpdate(() => { UpdateChildrenAlpha(); });
        tweener.OnComplete(() => { UpdateChildrenAlpha(); });
        return tweener;
    }

    public void UpdateChildrenAlpha()
    {
        ui.title.SetAlpha(ui.frame.color.a / (50.0f / 255.0f));
        ui.point.SetAlpha(ui.frame.color.a / (50.0f / 255.0f));
    }
}
