using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventScriptableObject;
using UnityEngine.UI;
using TMPro;
using System;

[Serializable]
public struct MainEventUIElement
{
    public Image TitleFrame;
    public TMP_Text Title;
    public TMP_Text Summary;
    public Image Character;
    public Image TalkFrame;
    public TMP_Text Name;
    public TMP_Text Talk;
    public GameObject OptionParent;
    public Button[] Options;
}

public class MainEventUI : EventUI
{
    [SerializeField] private MainEventSO eventSO = null;
    [SerializeField] private MainEventUIElement ui;

    public override void EventPlay()
    {
        eventSO.EventPlay();
    }

    public override void InitEventInfo(EventButton Event)
    {
        eventSO = Event.GetEventSO<MainEventSO>();
        eventSO.EventStart();
        gameObject.SetActive(true);
    }

    public MainEventUIElement GetMainEventUIElement()
    {
        return ui;
    }

    public void OnEnable()
    {
        ui.TitleFrame.color = HelperFunction.ChangeAlpha(ui.TitleFrame.color, 0.0f);
        ui.Title.color = HelperFunction.ChangeAlpha(ui.Title.color, 0.0f);
        ui.Summary.color = HelperFunction.ChangeAlpha(ui.Summary.color, 0.0f);
        ui.Character.color = HelperFunction.ChangeAlpha(ui.Character.color, 0.0f);
        ui.TalkFrame.color = HelperFunction.ChangeAlpha(ui.TalkFrame.color, 0.0f);
        ui.Name.color = HelperFunction.ChangeAlpha(ui.Name.color, 0.0f);
        ui.Talk.color = HelperFunction.ChangeAlpha(ui.Talk.color, 0.0f);
        ui.OptionParent.SetActive(false);
    }
}
