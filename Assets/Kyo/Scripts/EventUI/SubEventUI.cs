using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventScriptableObject;

public class SubEventUI : EventUI
{
    [SerializeField] private SubEventSO eventSO = null;

    public override void EventPlay()
    {
        eventSO.EventPlay();
    }

    public override void InitEventInfo(EventButton Event)
    {
        eventSO = Event.GetEventSO<SubEventSO>();

        gameObject.SetActive(true);
    }
}
