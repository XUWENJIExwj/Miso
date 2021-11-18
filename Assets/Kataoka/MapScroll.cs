using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[Serializable]
public struct UVScrollProperty
{
    public Vector2 Tiling;
    public Vector2 Offset;
}

public class MapScroll : Monosingleton<MapScroll>
{
    [SerializeField] private RectTransform[] rectTransform = null;
    [SerializeField] private Image[] image = null;
    [SerializeField] private UVScrollProperty uv;
    [SerializeField] private float scrollSpeed = 2.0f;
    [SerializeField] private bool onDrag = true;

    public override void InitAwake()
    {
        for (int i = 0; i < rectTransform.Length; ++i)
        {
            // Œ»Ý‚ÌScreenSize‚É‡‚í‚¹‚ÄAMap‚ÌƒTƒCƒY‚ð”ä—á“I‚ÉŠg‘åk¬
            rectTransform[i].sizeDelta = new Vector2(GlobalInfo.instance.refScreenSize.y / rectTransform[i].sizeDelta.y * rectTransform[i].sizeDelta.x, GlobalInfo.instance.refScreenSize.y);

            // Šg‘åk¬‚Ì‰Šú‰»
            image[i].material.SetVector("_Tiling", uv.Tiling);

            // UVScroll‚Ì‰Šú‰»
            image[i].material.SetVector("_Offset", uv.Offset);
        }
    }

    public void Init()
    {
        GlobalInfo.instance.SetMapSize(rectTransform[0].sizeDelta);
    }

    public void OnDrag()
    {
        if (Input.GetMouseButton(0) && onDrag)
        {
            // UVScroll
            Vector2 offset = new Vector2(scrollSpeed * Time.deltaTime * Input.GetAxis("Mouse X"), 0.0f);
            uv.Offset -= offset;
            image[0].material.SetVector("_Offset", uv.Offset);
            image[1].material.SetVector("_Offset", uv.Offset);
            GridScroll.instance.Move(offset);

            offset *= GlobalInfo.instance.mapSize;
            PollutionMap.instance.Move(offset);
            EventButtonManager.instance.Move(offset);
            Player.instance.Move(offset);
            RouteManager.instance.DrawRoute();
        }
    }

    public void SetOnDrag(bool OnDrag)
    {
        onDrag = OnDrag;
    }
}
