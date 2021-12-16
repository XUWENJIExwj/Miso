using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventScriptableObject;
using System;

[Serializable]
public struct AllEventButtonsData
{
    public EventButtonData[] datas;

    public void Init(int Count)
    {
        datas = new EventButtonData[Count];
    }
}

public class EventButtonManager : Monosingleton<EventButtonManager>
{
    [SerializeField] private EventPreview eventPreview = null;
    [SerializeField] private BaseEventPreview baseEventPreview = null;
    [SerializeField] private OceanEventButtons[] oceanEventButtons = null;
    [SerializeField] private List<EventButton> events = null;
    [SerializeField] private List<EventButton> mainEvents = null;
    [SerializeField] private EventButton[] baseEvents = null;
    [SerializeField] private AllEventButtonsData eventDatas;

    public override void InitAwake()
    {
        oceanEventButtons = GetComponentsInChildren<OceanEventButtons>();
        events = new List<EventButton>();
        mainEvents = new List<EventButton>();
        baseEvents = new BaseButton[GlobalInfo.instance.baseList.Count];
    }

    public void Init()
    {
        eventPreview.Init();
        baseEventPreview.Init();

        for (int i = 0; i < oceanEventButtons.Length; ++i)
        {
            oceanEventButtons[i].Init();
            events.AddRange(oceanEventButtons[i].GetEventButtons());
        }

        CreateEvents();

        eventDatas.Init(events.Count);
    }

    public void Load(AllEventButtonsData Data)
    {
        eventPreview.Init();
        baseEventPreview.Init();

        for (int i = 0; i < oceanEventButtons.Length; ++i)
        {
            oceanEventButtons[i].Load();
            events.AddRange(oceanEventButtons[i].GetEventButtons());
        }

        if (Data.datas.Length > 0)
        {
            for (int i = 0; i < events.Count; ++i)
            {
                events[i].Load(Data.datas[i]);
            }

            eventDatas = Data;
        }
        else
        {
            for (int i = 0; i < oceanEventButtons.Length; ++i)
            {
                oceanEventButtons[i].Init();
            }
            CreateEvents();

            eventDatas.Init(events.Count);
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

    public void AddBaseEvent(EventButton Event)
    {
        baseEvents[Event.GetEventSO<BaseEventSO>().id] = Event;
    }

    public BaseButton GetBaseEventButton(int Index)
    {
        return (BaseButton)baseEvents[Index];
    }

    public void LinkMainEventsToAMA(AMAs AMA)
    {
        foreach (EventButton button in mainEvents)
        {
            button.LinkMainEventToAMA(AMA);
        }
    }

    public AllEventButtonsData GetAllEventButtonsData()
    {
        for (int i = 0; i< events.Count; ++i)
        {
            eventDatas.datas[i] = events[i].GetEventData();
        }
        return eventDatas;
    }
}
