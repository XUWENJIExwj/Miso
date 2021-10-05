using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 各シーンにあるCanvas
public enum CanvasType
{
    MainGame_Camera,
    MainGame_Overlay,
    Max,
}

public class GlobalInfo : Monosingleton<GlobalInfo>
{
    public Vector2 refScreenSize = new Vector2(1080.0f, 1920.0f); // 参照画面サイズ
    public Canvas[] canvases; // 各Canvas
    public Vector2 mapSize;
    public Vector2 halfMapSize;

    public override void InitAwake()
    {
        // CanvasType数だけメモリ確保
        canvases = new Canvas[(int)CanvasType.Max];
    }

    // 参照画面サイズのアスペクト比の取得
    public float GetRefAspectRatio()
    {
        return refScreenSize.x / refScreenSize.y;
    }

    // 参照画面サイズのアスペクト比の逆数の取得
    public float GetRefAspectRatioReciprocal()
    {
        return refScreenSize.y / refScreenSize.x;
    }

    // Canvasの配列に現在のCanvasを格納
    public void SetCanvas(CanvasType Type, Canvas This)
    {
        canvases[(int)Type] = This;
    }

    public void SetMapSize(Vector2 MapSize)
    {
        mapSize = MapSize;
        halfMapSize = mapSize * 0.5f;
    }
}
