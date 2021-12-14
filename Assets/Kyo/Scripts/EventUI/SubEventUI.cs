using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventScriptableObject;
using UnityEngine.UI;
using System;
using TMPro;

[Serializable]
public struct SubEventUIElement
{
    public Image TitleFrame;
    public TextOutline Title;
    public Image SummaryFrame;
    public TextOutline Summary;
    public Image ReportFrame;
    public TextOutline Report;
    public Image PointText;
    public Text Point;
}

public class SubEventUI : EventUI
{
    [SerializeField] private SubEventSO eventSO = null;
    [SerializeField] private SubEventUIElement ui;

    public override void EventPlayPre(EventButton Event)
    {
        gameObject.SetActive(true);
        eventSO = Event.GetEventSO<SubEventSO>();
        eventSO.EventStart();
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
