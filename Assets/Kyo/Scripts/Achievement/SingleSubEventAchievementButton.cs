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
    [SerializeField] protected TMP_Text id = null;
    [SerializeField] protected Image titleFrame = null;
    [SerializeField] protected TMP_Text title = null;

    public void Init(SubEventSO Event)
    {
        eventSO = Event;
        icon.sprite = eventSO.achievement;
        icon.color = Color.grey;
        id.text = "No." + (eventSO.id + 1).ToString("00");
        title.text = eventSO.eventTitle;
        titleFrame.gameObject.SetActive(false);
    }

    public void Hide()
    {
        interactable = false;
        icon.color = HelperFunction.ChangeAlpha(icon.color, 0.0f);
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
            }
            else
            {
                icon.color = Color.grey;
            }
        }
    }

    // MouseÇ™ButtonÇÃè„Ç…ì¸ÇÈéû
    public override void OnPointerEnter(PointerEventData E)
    {
        if (eventSO)
        {
            titleFrame.gameObject.SetActive(true);
        }
    }

    // MouseÇ™ButtonÇÃè„Ç©ÇÁó£ÇÍÇÈéû
    public override void OnPointerExit(PointerEventData E)
    {
        if (eventSO)
        {
            titleFrame.gameObject.SetActive(false);
        }
    }
}
