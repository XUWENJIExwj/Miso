using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class EventUI : MonoBehaviour
{
    [SerializeField] protected RectTransform rectTransform = null;
    [SerializeField] protected Image image = null;

    public void Init()
    {
        // 現在のScreenSizeに合わせて、BGのサイズを比例的に拡大縮小
        rectTransform.sizeDelta = GlobalInfo.instance.refScreenSize;
        gameObject.SetActive(false);
    }

    public abstract void InitEventInfo(EventButton Event);

    public abstract void EventPlay();
}
