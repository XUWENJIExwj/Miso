using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OceanEventButtons : MonoBehaviour
{
    [SerializeField] private Oceans ocean = Oceans.None;
    [SerializeField] private OceanAreas oceanArea = OceanAreas.None;
    [SerializeField] private List<EventButton> eventButtons = null;

    public void Init()
    {
        eventButtons = new List<EventButton>();
        eventButtons.AddRange(GetComponentsInChildren<EventButton>());

        foreach (EventButton eventButton in eventButtons)
        {
            eventButton.Init(ocean, oceanArea);
        }
    }

    public void Load()
    {
        eventButtons = new List<EventButton>();
        eventButtons.AddRange(GetComponentsInChildren<EventButton>());
    }

    public List<EventButton> GetEventButtons()
    {
        return eventButtons;
    }
}
