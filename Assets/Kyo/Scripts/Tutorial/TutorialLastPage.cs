using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialLastPage : TutorialPage
{
    public override void OnEnable()
    {
        Button[] turnButtons = TutorialView.instance.GetTurnButtons();
        turnButtons[1].interactable = false;
        turnButtons[1].GetComponent<Image>().color = Color.grey;
    }
}
