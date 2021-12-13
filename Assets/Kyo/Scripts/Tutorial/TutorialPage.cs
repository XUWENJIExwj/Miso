using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TutorialPage : MonoBehaviour
{
    public virtual void Init(Sprite Tutorial)
    {
        GetComponent<Image>().sprite = Tutorial;
        gameObject.SetActive(false);
    }

    public void TurnPage(TutorialPage NextPage)
    {
        ActiveNextPage(NextPage);
    }

    public void ActiveNextPage(TutorialPage NextPage)
    {
        NextPage.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    public virtual void OnEnable()
    {
        Button[] turnButtons = TutorialView.instance.GetTurnButtons();
        turnButtons[0].interactable = true;
        turnButtons[0].GetComponent<Image>().color = Color.white;
        turnButtons[1].interactable = true;
        turnButtons[1].GetComponent<Image>().color = Color.white;
    }
}
