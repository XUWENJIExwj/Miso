using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using EventScriptableObject;
using DG.Tweening;

public abstract class Result : MonoBehaviour
{
    [SerializeField] protected int point = 0;
    static protected Tweener tweener = null;
    protected float fadeTime = 0.8f;

    public virtual void Init(EventSO Event)
    {

    }

    public virtual void Init(CleanupObserver Observer)
    {

    }

    public abstract void Appear();

    public abstract void Show();

    public void KillTweener()
    {
        if (tweener.IsActive())
        {
            tweener.Kill();
        }
    }

    public void AppearNext()
    {
        BaseEventUI eventUI = EventUIManager.instance.GetCurrentEventUI<BaseEventUI>();
        eventUI.AppearResult();
    }

    public void OnDisable()
    {
        Destroy(gameObject);
    }

    static public bool TweenerActive()
    {
        return tweener.IsActive();
    }

    public int GetPoint()
    {
        return point;
    }
}
