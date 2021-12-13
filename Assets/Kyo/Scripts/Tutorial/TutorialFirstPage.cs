using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialFirstPage : TutorialPage
{
    public override void OnEnable()
    {
        Button[] turnButtons = TutorialView.instance.GetTurnButtons();
        turnButtons[0].interactable = false;
        turnButtons[0].GetComponent<Image>().color = Color.grey;
        turnButtons[1].interactable = true;
        turnButtons[1].GetComponent<Image>().color = Color.white;
    }
}
