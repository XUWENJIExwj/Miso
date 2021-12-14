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
    public TextOutline Title;
    public Image SummaryFrame;
    public TextOutline Summary;
    public Image Character;
    public Image TalkFrame;
    public Image NameFrame;
    public TextOutline Name;
    public TextOutline Talk;
    public Image PointText;
    public Text Point;
    public GameObject OptionParent;
    public Button[] Options;
}

public class MainEventUI : EventUI
{ 
    [SerializeField] private MainEventSO eventSO = null;
    [SerializeField] private MainEventUIElement ui;

    public override void EventPlayPre(EventButton Event)
    {
        if (Player.instance.MainEventPlayed())
        {
            eventSO.Through();
        }
        else
        {
            gameObject.SetActive(true);
            eventSO = Event.GetEventSO<MainEventSO>();
            eventSO.EventStart();
        }
    }

    public override void EventPlay()
    {
        eventSO.EventPlay();
    }

    public MainEventUIElement GetEventUIElement()
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
        ui.Character.color = HelperFunction.ChangeAlpha(ui.Character.color, 0.0f);
        ui.TalkFrame.color = HelperFunction.ChangeAlpha(ui.TalkFrame.color, 0.0f);
        ui.NameFrame.color = HelperFunction.ChangeAlpha(ui.NameFrame.color, 0.0f);
        ui.Name.SetAlpha(0.0f);
        ui.Name.SetText("");
        ui.Talk.SetAlpha(0.0f);
        ui.Talk.SetText("");
        ui.PointText.color = HelperFunction.ChangeAlpha(ui.PointText.color, 0.0f);
        ui.Point.color = HelperFunction.ChangeAlpha(ui.Point.color, 0.0f);
        ui.Point.text = "";
        ui.OptionParent.SetActive(false);
    }
}
