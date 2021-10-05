using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridScroll : Monosingleton<GridScroll>
{
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private Image image;
    [SerializeField] private UVScrollProperty uv;

    public override void InitAwake()
    {
        //rectTransform.sizeDelta = new Vector2(GlobalInfo.instance.refScreenSize.y / rectTransform.sizeDelta.y * rectTransform.sizeDelta.x, GlobalInfo.instance.refScreenSize.y);

        // 拡大縮小の初期化
        image.material.SetVector("_Tiling", uv.Tiling);

        // UVScrollの初期化
        image.material.SetVector("_Offset", uv.Offset);
    }

    public void Move(Vector2 Offset)
    {
        Vector2 offset = Offset * uv.Tiling;
        uv.Offset -= offset;
        image.material.SetVector("_Offset", uv.Offset);
    }
    
    // Inspectorにある属性を編集するとEditorに反映してくれるコールバック
    void OnValidate()
    {
        image.material.SetVector("_Tiling", uv.Tiling);
        image.material.SetVector("_Offset", uv.Offset);
    }

    public Vector2 GetUVTiling()
    {
        return uv.Tiling;
    }

    public Vector2 GetFixedUVOffset()
    {
        return uv.Offset - new Vector2(0.5f, 0.5f);
    }
}
