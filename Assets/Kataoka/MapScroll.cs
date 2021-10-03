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
    [SerializeField] private bool onFixed = false;

    public override void InitAwake()
    {
        // ���݂�ScreenSize�ɍ��킹�āAMap�̃T�C�Y����I�Ɋg��k��
        if(onFixed)
        {
            rectTransform.sizeDelta = new Vector2(GlobalInfo.instance.refScreenSize.y / rectTransform.sizeDelta.y * rectTransform.sizeDelta.x, GlobalInfo.instance.refScreenSize.y);
        } 

        // �g��k���̏�����
        image.material.SetVector("_Tiling", uv.Tiling);

        // UVScroll�̏�����
        image.material.SetVector("_Offset", uv.Offset);

        GridScroll.instance.Init();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
            OnDrag();
    }

    public void OnDrag()
    {
        // UVScroll
        Vector2 offset = new Vector2(scrollSpeed * Time.deltaTime * Input.GetAxis("Mouse X"), scrollSpeed * Time.deltaTime * Input.GetAxis("Mouse Y"));
        uv.Offset -= offset;
        image.material.SetVector("_Offset", uv.Offset);
        GridScroll.instance.Move(offset);
    }
}
