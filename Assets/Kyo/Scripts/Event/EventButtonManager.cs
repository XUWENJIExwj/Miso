using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventScriptableObject;

public class EventButtonManager : Monosingleton<EventButtonManager>
{
    [SerializeField] private EventPreview eventPreview = null;
    [SerializeField] private BaseEventPreview baseEventPreview = null;
    [SerializeField] private OceanEventButtons[] oceanEventButtons = null;
    [SerializeField] private List<EventButton> events = null;
    [SerializeField] private List<EventButton> mainEvents = null;

    public override void InitAwake()
    {
        oceanEventButtons = GetComponentsInChildren<OceanEventButtons>();
        events = new List<EventButton>();
        mainEvents = new List<EventButton>();
    }

    public void Init()
    {
        eventPreview.Init();
        baseEventPreview.Init();

        foreach (OceanEventButtons oceanEventButton in oceanEventButtons)
        {
            oceanEventButton.Init();
            events.AddRange(oceanEventButton.GetEventButtons());
        }
    }

    public void CreateEvents()
    {
        foreach (EventButton eventButton in events)
        {
            eventButton.CreateEvent();
        }
    }

    public void ResetEvents()
    {
        mainEvents.Clear();
        foreach (EventButton eventButton in events)
        {
            eventButton.ResetEvent();
        }
    }

    public EventPreview GetEventPreview()
    {
        return eventPreview;
    }

    public BaseEventPreview GetBaseEventPreview()
    {
        return baseEventPreview;
    }

    public void Move(Vector2 Offset)
    {
        foreach (EventButton eventButton in events)
        {
            eventButton.Move(Offset);
        }
    }

    public void MovePath(Vector2 Offset, float Time)
    {
        foreach (EventButton eventButton in events)
        {
            eventButton.MovePath(Offset, Time);
        }
    }

    public void ActiveEventButton()
    {
        foreach (EventButton eventButton in events)
        {
            eventButton.gameObject.SetActive(true);
        }
    }

    public void AddMainEvent(EventButton Event)
    {
        mainEvents.Add(Event);
    }

    public void LinkMainEventsToAMA(AMAs AMA)
    {
        foreach (EventButton button in mainEvents)
        {
            button.LinkMainEventToAMA(AMA);
        }
    }
}
