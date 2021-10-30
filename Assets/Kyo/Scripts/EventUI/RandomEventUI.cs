using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventScriptableObject;

public class RandomEventUI : EventUI
{
    [SerializeField] private RandomEventSO eventSO = null;

    public override void InitEventInfo(EventButton Event)
    {
        eventSO = Event.GetEventSO<RandomEventSO>();

        gameObject.SetActive(true);
    }

    public override void EventPlay()
    {
        eventSO.EventPlay();
    }
}
