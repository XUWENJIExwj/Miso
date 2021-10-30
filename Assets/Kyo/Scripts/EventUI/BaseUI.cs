using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventScriptableObject;

public class BaseUI : EventUI
{
    [SerializeField] private BaseSO eventSO = null;

    public override void InitEventInfo(EventButton Event)
    {
        gameObject.SetActive(true);
        eventSO = Event.GetEventSO<BaseSO>();
        eventSO.EventStart();
    }

    public override void EventPlay()
    {
        eventSO.EventPlay();
    }
}
