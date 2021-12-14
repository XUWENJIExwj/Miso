using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using EventScriptableObject;
using UnityEngine.EventSystems;

public class SingleSubEventAchievementButton : Button
{
    [SerializeField] protected SubEventSO eventSO = null;
    [SerializeField] protected Image icon = null;
    [SerializeField] protected AchievementMetalFrame iconFrame = null;
    [SerializeField] protected Text id = null;
    [SerializeField] protected Image titleFrame = null;
    [SerializeField] protected Text title = null;

    public void Init(SubEventSO Event)
    {
        eventSO = Event;
        icon.sprite = eventSO.achievement;
        icon.color = Color.grey;
        iconFrame.SetColor(Color.grey);
        id.text = "No." + (eventSO.id + 1).ToString("00");
        title.text = eventSO.eventTitle;
        titleFrame.gameObject.SetActive(false);
    }

    public void Hide()
    {
        interactable = false;
        icon.color = HelperFunction.ChangeAlpha(icon.color, 0.0f);
        iconFrame.Hide();
        id.color = HelperFunction.ChangeAlpha(id.color, 0.0f);
        titleFrame.gameObject.SetActive(false);
    }

    public virtual void SetAchievementInfo(AMAs AMA)
    {
        if (eventSO)
        {
            if (Player.instance.GetPlayerData().achievements.sub.completed[eventSO.id])
            {
                icon.color = Color.white;
                iconFrame.SetColor(Color.white);
            }
            else
            {
                icon.color = Color.grey;
                iconFrame.SetColor(Color.grey);
            }
        }
    }

    public virtual void PlaySE()
    {
        SoundManager.instance.SE_SubEvent();
    }

    // MouseÇ™ButtonÇÃè„Ç…ì¸ÇÈéû
    public override void OnPointerEnter(PointerEventData E)
    {
        if (eventSO)
        {
            PlaySE();
            StartAnimateMetalFrame();

            titleFrame.gameObject.SetActive(true);
        }
    }

    // MouseÇ™ButtonÇÃè„Ç©ÇÁó£ÇÍÇÈéû
    public override void OnPointerExit(PointerEventData E)
    {
        if (eventSO)
        {
            StopAnimateMetalFrame();
            titleFrame.gameObject.SetActive(false);
        }
    }

    public virtual void StartAnimateMetalFrame()
    {
        if (Player.instance.GetPlayerData().achievements.sub.completed[eventSO.id])
        {
            iconFrame.StartAnimateFrame();
        }
    }

    public virtual void StopAnimateMetalFrame()
    {
        if (Player.instance.GetPlayerData().achievements.sub.completed[eventSO.id])
        {
            iconFrame.StopAnimateFrame();
        }
    }
}
