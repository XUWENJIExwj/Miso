using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventScriptableObject;

public class BaseUI : EventUI
{
    [SerializeField] private BaseSO eventSO = null;

    public override void InitEventInfo(EventButton Event)
    {
        eventSO = Event.GetEventSO<BaseSO>();

        gameObject.SetActive(true);
    }

    public override void EventPlay()
    {
        eventSO.EventPlay();
    }
}
