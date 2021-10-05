using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventButtonManager : Monosingleton<EventButtonManager>
{
    [SerializeField] private GameObject eventPrefab = null;
    [SerializeField] private List<EventButton> events = null;

    public void Init()
    {
        CreateEventButton();
    }

    public void CreateEventButton()
    {
        events = new List<EventButton>();
        Vector2 mapSize = MapScroll.instance.GetMapSize();
        Vector2 halfMapSize = mapSize * 0.5f;
        Vector2 gridTiling = GridScroll.instance.GetUVTiling();
        Vector2 gridOffset = GridScroll.instance.GetFixedUVOffset();
        Vector2 interval = MapScroll.instance.GetMapSize() / gridTiling;
        Vector2 fixedInterval = mapSize / gridTiling;

        for (int i = 0; i < gridTiling.y; ++i)
        {
            for (int j = 0; j < gridTiling.x; ++j)
            {
                EventButton eventButton = Instantiate(eventPrefab, transform).GetComponent<EventButton>();
                eventButton.transform.localPosition = new Vector3(
                    -halfMapSize.x + j * interval.x - gridOffset.x * fixedInterval.x,
                    halfMapSize.y - i * interval.y - gridOffset.y * fixedInterval.y,
                    0.0f);
                eventButton.gameObject.name = "EventButton_" + (i * gridTiling.x + j).ToString("000");
                eventButton.FixPostion(mapSize, halfMapSize);
                events.Add(eventButton);
            }
        }
    }

    public void Move(Vector2 Offset)
    {
        Vector2 mapSize = MapScroll.instance.GetMapSize();
        Vector2 offset = Offset * mapSize;

        foreach (EventButton eventButton in events)
        {
            eventButton.Move(offset, mapSize, mapSize * 0.5f);
        }
    }
}
