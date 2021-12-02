using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;

[Serializable]
public struct UVScrollProperty
{
    public Vector2 Tiling;
    public Vector2 Offset;
}

public class MapScroll : Monosingleton<MapScroll>
{
    [SerializeField] private RectTransform[] rectTransforms = null;
    [SerializeField] private Image[] images = null;
    [SerializeField] private UVScrollProperty uv;
    [SerializeField] private float scrollSpeed = 2.0f;
    [SerializeField] private bool onDrag = true;

    public override void InitAwake()
    {
        for (int i = 0; i < rectTransforms.Length; ++i)
        {
            // Œ»Ý‚ÌScreenSize‚É‡‚í‚¹‚ÄAMap‚ÌƒTƒCƒY‚ð”ä—á“I‚ÉŠg‘åk¬
            rectTransforms[i].sizeDelta = new Vector2(GlobalInfo.instance.refScreenSize.y / rectTransforms[i].sizeDelta.y * rectTransforms[i].sizeDelta.x, GlobalInfo.instance.refScreenSize.y);

            // Šg‘åk¬‚Ì‰Šú‰»
            images[i].material.SetVector("_Tiling", uv.Tiling);

            // UVScroll‚Ì‰Šú‰»
            images[i].material.SetVector("_Offset", uv.Offset);
        }
    }

    public void Init()
    {
        GlobalInfo.instance.SetMapSize(rectTransforms[0].sizeDelta);
    }

    public void OnDrag()
    {
        if (Input.GetMouseButton(0) && onDrag)
        {
            // UVScroll
            Vector2 offset = new Vector2(scrollSpeed * Time.deltaTime * Input.GetAxis("Mouse X"), 0.0f);
            Move(offset);
            GridScroll.instance.Move(offset);

            offset *= GlobalInfo.instance.mapSize;
            PollutionMap.instance.Move(offset);
            EventButtonManager.instance.Move(offset);
            Player.instance.Move(offset);
            RouteManager.instance.DrawRoute();
        }
    }

    public void Move(Vector2 Offset)
    {
        uv.Offset -= Offset;
        foreach (Image image in images)
        {
            image.material.SetVector("_Offset", uv.Offset);
        }
    }

    public void MovePath(Vector2 Offset, float Time)
    {
        uv.Offset += Offset;
        foreach (Image image in images)
        {
            image.material.DOVector(uv.Offset, "_Offset", Time).SetEase(Ease.Linear);
        }

    }

    public void SetOnDrag(bool OnDrag)
    {
        onDrag = OnDrag;
    }
}
