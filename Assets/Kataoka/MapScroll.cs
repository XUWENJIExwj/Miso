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
    [SerializeField] private RectTransform[] rectTransform = null;
    [SerializeField] private Image[] image = null;
    [SerializeField] private UVScrollProperty uv;
    [SerializeField] private float scrollSpeed = 2.0f;
    [SerializeField] private bool onDrag = true;

    public override void InitAwake()
    {
        for (int i = 0; i < rectTransform.Length; ++i)
        {
            // 現在のScreenSizeに合わせて、Mapのサイズを比例的に拡大縮小
            rectTransform[i].sizeDelta = new Vector2(GlobalInfo.instance.refScreenSize.y / rectTransform[i].sizeDelta.y * rectTransform[i].sizeDelta.x, GlobalInfo.instance.refScreenSize.y);

            // 拡大縮小の初期化
            image[i].material.SetVector("_Tiling", uv.Tiling);

            // UVScrollの初期化
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
        image[0].material.SetVector("_Offset", uv.Offset);
        image[1].material.SetVector("_Offset", uv.Offset);
    }

    public void MovePath(Vector2 Offset, float Time)
    {
        uv.Offset += Offset;
        image[0].material.DOVector(uv.Offset, "_Offset", Time).SetEase(Ease.Linear);
        image[1].material.DOVector(uv.Offset, "_Offset", Time).SetEase(Ease.Linear);
    }

    public void SetOnDrag(bool OnDrag)
    {
        onDrag = OnDrag;
    }
}
