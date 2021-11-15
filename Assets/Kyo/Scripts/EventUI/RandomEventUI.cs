using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventScriptableObject;

public class RandomEventUI : EventUI
{
    [SerializeField] private RandomEventSO eventSO = null;
    [SerializeField] private SubEventUIElement ui;

    public override void EventPlayPre(EventButton Event)
    {
        eventSO = Event.GetEventSO<RandomEventSO>();

        if (Player.instance.Encounter())
        {
            gameObject.SetActive(true);
            eventSO.EventStart();
        }
        else
        {
            eventSO.Through();
        }
    }

    public override void EventPlay()
    {
        eventSO.EventPlay();
    }

    public SubEventUIElement GetEventUIElement()
    {
        return ui;
    }

    public void OnEnable()
    {
        ui.TitleFrame.color = HelperFunction.ChangeAlpha(ui.TitleFrame.color, 0.0f);
        ui.Title.color = HelperFunction.ChangeAlpha(ui.Title.color, 0.0f);
        ui.Title.text = "";
        ui.Summary.color = HelperFunction.ChangeAlpha(ui.Summary.color, 0.0f);
        ui.Summary.text = "";
        ui.ReportFrame.color = HelperFunction.ChangeAlpha(ui.ReportFrame.color, 0.0f);
        ui.Report.color = HelperFunction.ChangeAlpha(ui.Report.color, 0.0f);
        ui.Report.text = "";
        ui.Point.color = HelperFunction.ChangeAlpha(ui.Point.color, 0.0f);
        ui.Point.text = "";
    }
}
