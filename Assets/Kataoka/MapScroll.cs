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
    [SerializeField] private RectTransform rectTransform = null;
    [SerializeField] private Image image = null;
    [SerializeField] private UVScrollProperty uv;
    [SerializeField] private float scrollSpeed = 2.0f;
    [SerializeField] private bool onDrag = true;

    public override void InitAwake()
    {
        // 現在のScreenSizeに合わせて、Mapのサイズを比例的に拡大縮小
        rectTransform.sizeDelta = new Vector2(GlobalInfo.instance.refScreenSize.y / rectTransform.sizeDelta.y * rectTransform.sizeDelta.x, GlobalInfo.instance.refScreenSize.y);

        // 拡大縮小の初期化
        image.material.SetVector("_Tiling", uv.Tiling);

        // UVScrollの初期化
        image.material.SetVector("_Offset", uv.Offset);
    }

    private void Start()
    {
        GlobalInfo.instance.SetMapSize(rectTransform.sizeDelta);
        EventButtonManager.instance.CreateEventButton();
    }

    public void OnDrag()
    {
        if (Input.GetMouseButton(0) && onDrag)
        {
            // UVScroll
            Vector2 offset = new Vector2(scrollSpeed * Time.deltaTime * Input.GetAxis("Mouse X"), scrollSpeed * Time.deltaTime * Input.GetAxis("Mouse Y"));
            uv.Offset -= offset;
            image.material.SetVector("_Offset", uv.Offset);
            GridScroll.instance.Move(offset);
            EventButtonManager.instance.Move(offset);
            RouteManager.instance.DrawRoute();
        }
    }

    public void SetOnDrag(bool OnDrag)
    {
        onDrag = OnDrag;
    }
}
