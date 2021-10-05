using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridScroll : Monosingleton<GridScroll>
{
    [SerializeField] private RectTransform rectTransform = null;
    [SerializeField] private Image image = null;
    [SerializeField] private UVScrollProperty uv;

    public override void InitAwake()
    {
        // �g��k���̏�����
        image.material.SetVector("_Tiling", uv.Tiling);

        // UVScroll�̏�����
        image.material.SetVector("_Offset", uv.Offset);
    }

    private void Start()
    {
        rectTransform.sizeDelta = GlobalInfo.instance.mapSize;
    }

    public void Move(Vector2 Offset)
    {
        Vector2 offset = Offset * uv.Tiling;
        uv.Offset -= offset;
        image.material.SetVector("_Offset", uv.Offset);
    }
    
    // Inspector�ɂ��鑮����ҏW�����Editor�ɔ��f���Ă����R�[���o�b�N
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
