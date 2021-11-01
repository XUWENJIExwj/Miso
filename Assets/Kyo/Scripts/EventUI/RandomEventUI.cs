using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventScriptableObject;

public class RandomEventUI : EventUI
{
    [SerializeField] private RandomEventSO eventSO = null;

    public override void EventPlayPre(EventButton Event)
    {
        gameObject.SetActive(true);
        eventSO = Event.GetEventSO<RandomEventSO>();
        eventSO.EventStart();
    }

    public override void EventPlay()
    {
        eventSO.EventPlay();
    }
}
