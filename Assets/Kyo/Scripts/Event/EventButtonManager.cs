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

    public override void InitAwake()
    {
        oceanEventButtons = GetComponentsInChildren<OceanEventButtons>();
        events = new List<EventButton>();
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

    public void ActiveEventButton()
    {
        foreach (EventButton eventButton in events)
        {
            eventButton.gameObject.SetActive(true);
        }
    }
}
