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
        ui.Title.SetAlpha(0.0f);
        ui.Title.SetText("");
        ui.SummaryFrame.color = HelperFunction.ChangeAlpha(ui.SummaryFrame.color, 0.0f);
        ui.Summary.SetAlpha(0.0f);
        ui.Summary.SetText("");
        ui.ReportFrame.color = HelperFunction.ChangeAlpha(ui.ReportFrame.color, 0.0f);
        ui.Report.SetAlpha(0.0f);
        ui.Report.SetText("");
        ui.PointText.color = HelperFunction.ChangeAlpha(ui.PointText.color, 0.0f);
        ui.Point.color = HelperFunction.ChangeAlpha(ui.Point.color, 0.0f);
        ui.Point.text = "";
    }
}
