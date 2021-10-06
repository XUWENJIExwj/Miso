using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EventScriptableObject;

// 各シーンにあるCanvas
public enum CanvasType
{
    MainGame_Camera,
    MainGame_Overlay,
    Max,
}

public struct PlayerData
{
    EventButton playerBase;
    int score;
}

public class GlobalInfo : Monosingleton<GlobalInfo>
{
    public Vector2 refScreenSize = new Vector2(1080.0f, 1920.0f); // 参照画面サイズ
    public Canvas[] canvases = null; // 各Canvas
    public Vector2 mapSize = new Vector2(2560.0f, 1920.0f);
    public Vector2 halfMapSize = new Vector2(1280.0f, 960.0f);
    public List<EventSO> baseList = null;
    public List<MainEventSO> mainEventList = null;
    public List<SubEventSO> subEventList = null;
    public float[] eventRatio = new float[] { 0.2f, 0.3f, 0.5f };

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


    // EventButtonにEventの情報を与える
    public EventSO SetEventInfo(int X, int Y)
    {
        // 特定の座標であれば、Baseを返す
        Vector2 pos = GridScroll.instance.GetUVTiling() * 0.5f;
        if (X == (int)pos.x && Y == (int)pos.y)
        {
            return baseList[0];
        }

        // その他の座標であれば、Eventを返す
        float ratio = Random.Range(0.0f, 1.0f);
        if (ratio < eventRatio[(int)EventButtonType.MainEvent])
        {
            return mainEventList[Random.Range(0, mainEventList.Count)];
        }
        else if (ratio < eventRatio[(int)EventButtonType.MainEvent] + eventRatio[(int)EventButtonType.SubEvent])
        {
            return subEventList[Random.Range(0, subEventList.Count)];
        }

        return null;
    }
}
