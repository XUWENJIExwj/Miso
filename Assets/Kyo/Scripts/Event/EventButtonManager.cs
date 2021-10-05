using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventButtonManager : Monosingleton<EventButtonManager>
{
    [SerializeField] private GameObject eventPrefab = null;
    [SerializeField] private List<EventButton> events = null;

    public override void InitAwake()
    {
        events = new List<EventButton>();
    }

    public void Init()
    {
        CreateEventButton();
    }

    public void CreateEventButton()
    {
        Vector2 gridTiling = GridScroll.instance.GetUVTiling();
        Vector2 gridOffset = GridScroll.instance.GetFixedUVOffset();
        Vector2 interval = GlobalInfo.instance.mapSize / gridTiling;

        for (int i = 0; i < gridTiling.y; ++i)
        {
            for (int j = 0; j < gridTiling.x; ++j)
            {
                EventButton eventButton = Instantiate(eventPrefab, transform).GetComponent<EventButton>();
                eventButton.transform.localPosition = new Vector3(
                    -GlobalInfo.instance.halfMapSize.x + j * interval.x - gridOffset.x * interval.x,
                    GlobalInfo.instance.halfMapSize.y - i * interval.y - gridOffset.y * interval.y,
                    0.0f);
                eventButton.gameObject.name = "EventButton_" + (i * gridTiling.x + j).ToString("000");
                eventButton.FixPostion();
                events.Add(eventButton);
            }
        }
    }

    public void Move(Vector2 Offset)
    {
        Vector2 offset = Offset * GlobalInfo.instance.mapSize;

        foreach (EventButton eventButton in events)
        {
            eventButton.Move(offset);
        }
    }
}
