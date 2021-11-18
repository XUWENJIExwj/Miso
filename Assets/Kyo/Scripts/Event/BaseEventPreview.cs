using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BaseEventPreview : EventPreview
{
    [SerializeField] private Image ama;
    [SerializeField] private TMP_Text got;

    public void SetAMASprite(Sprite AMA)
    {
        ama.sprite = AMA;
    }

    public void SetGot(string Got)
    {
        got.text = Got;
    }

    public override void ResetPreview()
    {
        eventDesc.text = "";
        moveability.text = "";
        amaType.text = "";
        got.text = "";
        //ama.sprite = null;
    }
}
