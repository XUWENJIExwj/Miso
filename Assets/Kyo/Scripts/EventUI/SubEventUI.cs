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
    public TMP_Text Title;
    public TMP_Text Summary;
}

public class SubEventUI : EventUI
{
    [SerializeField] private SubEventSO eventSO = null;
    [SerializeField] private SubEventUIElement ui;

    public override void InitEventInfo(EventButton Event)
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
        ui.Title.color = HelperFunction.ChangeAlpha(ui.Title.color, 0.0f);
        ui.Title.text = "";
        ui.Summary.color = HelperFunction.ChangeAlpha(ui.Summary.color, 0.0f);
        ui.Summary.text = "";
    }
}
