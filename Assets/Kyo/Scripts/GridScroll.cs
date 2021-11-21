using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

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

    public void Init()
    {
        rectTransform.sizeDelta = GlobalInfo.instance.mapSize;
    }

    public void Move(Vector2 Offset)
    {
        Vector2 offset = Offset * uv.Tiling;
        uv.Offset -= offset;
        image.material.SetVector("_Offset", uv.Offset);
    }

    public void MovePath(Vector2 Offset, float Time)
    {
        uv.Offset += Offset;
        image.material.DOVector(uv.Offset, "_Offset", Time).SetEase(Ease.Linear);
    }

    // Inspector�ɂ��鑮����ҏW�����Editor�ɔ��f���Ă����R�[���o�b�N
    void OnValidate()
    {
        image.material.SetVector("_Tiling", uv.Tiling);
        image.material.SetVector("_Offset", uv.Offset);
    }

    public Vector2 GetGridInterval()
    {
        return rectTransform.sizeDelta / uv.Tiling;
    }

    public float GetIntervalLength2()
    {
        return HelperFunction.Length2(GetGridInterval());
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
