using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum CanvasType
{
    MainGame,
    Max,
}

public class GlobalInfo : Monosingleton<GlobalInfo>
{
    public Vector2 refScreenSize = new Vector2(1080.0f, 1920.0f);
    public Canvas[] canvases;

    public override void Init()
    {
        canvases = new Canvas[(int)CanvasType.Max];
    }

    public float GetRefAspectRatio()
    {
        return refScreenSize.x / refScreenSize.y;
    }

    public float GetRefAspectRatioReciprocal()
    {
        return refScreenSize.y / refScreenSize.x;
    }

    public void SetCanvas(CanvasType Type, Canvas This)
    {
        canvases[(int)Type] = This;
    }
}
