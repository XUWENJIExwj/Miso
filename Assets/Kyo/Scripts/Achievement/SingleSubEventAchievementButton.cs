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

    public virtual void SetAchievementInfo(AMAs AMA = AMAs.Max)
    {
        if (eventSO && Player.instance.GetPlayerData().achievements.sub.completed[eventSO.id])
        {
            icon.color = Color.white;
        }
    }

    // MouseがButtonの上に入る時
    public override void OnPointerEnter(PointerEventData E)
    {
        titleFrame.gameObject.SetActive(true);
    }

    // MouseがButtonの上から離れる時
    public override void OnPointerExit(PointerEventData E)
    {
        titleFrame.gameObject.SetActive(false);
    }
}
