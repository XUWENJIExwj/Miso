using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventScriptableObject;

public class EventUIManager : Monosingleton<EventUIManager>
{
    [SerializeField] private EventUI[] eventUIs = null;
    [SerializeField] private EventSOType currentEventType = EventSOType.None;

    public void Init()
    {
        foreach (EventUI eventUI in eventUIs)
        {
            eventUI.Init();
        }
    }

    public void EventPlayPre(EventButton Event)
    {
        currentEventType = Event.GetEventType();
        eventUIs[(int)currentEventType].EventPlayPre(Event);
    }

    public void EventPlay()
    {
        eventUIs[(int)currentEventType].EventPlay();
    }

    public void ResetEventInfo()
    {
        if (currentEventType != EventSOType.None)
        {
            eventUIs[(int)currentEventType].gameObject.SetActive(false);
        }
    }

    public T GetCurrentEventUI<T>()
    {
        if (eventUIs[(int)currentEventType].GetType() == typeof(T))
        {
            return (T)(object)eventUIs[(int)currentEventType];
        }

        Debug.Log("Žæ“¾‚µ‚æ‚¤‚Æ‚·‚éŒ^‚Æˆá‚¤!");
        return (T)(object)null;
    }

    public void AddResult(EventSO Event)
    {
        BaseEventUI eventUI = (BaseEventUI)eventUIs[(int)EventSOType.Base];
        eventUI.AddResult(Event);
    }
}
