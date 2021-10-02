using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

[Serializable]
public struct UVScrollProperty
{
    public Vector2 Tiling;
    public Vector2 Offset;
}

public class MapScroll : Monosingleton<MapScroll>
{
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private Image image;
    [SerializeField] private UVScrollProperty uv;
    [SerializeField] private float scrollSpeed;

    public override void Init()
    {
        // 参照画面サイズをもとに、Mapのサイズを比例的に拡大縮小
        rectTransform.sizeDelta =
            new Vector2(GlobalInfo.instance.refScreenSize.y / rectTransform.sizeDelta.y * rectTransform.sizeDelta.x, GlobalInfo.instance.refScreenSize.y);

        // 拡大縮小の初期化
        image.material.SetVector("_Tiling", uv.Tiling);

        // UVScrollの初期化
        image.material.SetVector("_Offset", uv.Offset);
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
            OnDrag();
    }

    public void OnDrag()
    {
        // UVScroll
        uv.Offset.x -= scrollSpeed * Time.deltaTime * Input.GetAxis("Mouse X");
        uv.Offset.y -= scrollSpeed * Time.deltaTime * Input.GetAxis("Mouse Y");
        image.material.SetVector("_Offset", uv.Offset);
    }
}
