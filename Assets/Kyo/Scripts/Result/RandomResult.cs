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
    }

    public override void Appear()
    {

    }

    public override void Show()
    {
        KillTweener();
    }
}
