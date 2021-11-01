using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventScriptableObject;
using UnityEngine.UI;

public class BaseEventUI : EventUI
{
    [SerializeField] private BaseEventSO eventSO = null;
    [SerializeField] private ResultConfirmButton button = null;
    [SerializeField] private ScrollRect resultView = null;
    [SerializeField] private Result[] prefabs = null;
    [SerializeField] private Queue<Result> results = null;
    [SerializeField] private Result currentResult = null;

    public void Awake()
    {
        results = new Queue<Result>();
    }

    public override void EventPlayPre(EventButton Event)
    {
        gameObject.SetActive(true);
        eventSO = Event.GetEventSO<BaseEventSO>();
        eventSO.EventStart();
    }

    public override void EventPlay()
    {
        eventSO.EventPlay();
    }

    public void AddResult(EventSO Event)
    {
        Result result = Instantiate(prefabs[(int)Event.type], resultView.content);
        result.Init(Event);
        results.Enqueue(result);
    }

    // Result�̏o���i�A�j���[�V�����t���j
    public void AppearResult()
    {
        if (results.Count > 0)
        {
            currentResult = results.Dequeue();
            currentResult.Appear();
        }
        else
        {
            currentResult = null;
        }
    }

    // Result�̏o���i�A�j���[�V�����Ȃ��j
    public void ShowResult()
    {
        if (currentResult)
        {
            currentResult.Show();
            currentResult = null;
        }

        foreach (Result result in results)
        {
            result.Show();
        }
        results.Clear();
    }

    public void OnClick()
    {
        if (ResultTweenerActive())
        {
            ShowResult();
            return;
        }
        eventSO.SetNextPhase(BaseEventPhase.Phase_End);
    }

    public bool ConfirmButtonPointerEnter()
    {
        return button.PointerEnter();
    }

    public bool ResultTweenerActive()
    {
        return Result.TweenerActive();
    }
}
