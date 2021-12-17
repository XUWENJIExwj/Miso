using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EventScriptableObject;
using System;

// �e�V�[���ɂ���Canvas
public enum CanvasType
{
    MainGame_Camera,
    MainGame_Overlay,
    Max,
}

[Serializable]
public struct AMAMainEvents
{
    public List<MainEventSO> mainEvents;
}

public class GlobalInfo : Monosingleton<GlobalInfo>
{
    public Vector2 refScreenSize = new Vector2(1080.0f, 1920.0f); // �Q�Ɖ�ʃT�C�Y
    public Canvas[] canvases = null; // �eCanvas
    public Vector2 mapSize = new Vector2(2560.0f, 1920.0f);
    public Vector2 halfMapSize = new Vector2(1280.0f, 960.0f);
    public List<AMASO> amaList = null;
    public List<BaseEventSO> baseList = null;
    public AMAMainEvents[] mainEventLists;
    public List<SubEventSO> subEventList = null;
    public List<RandomEventSO> randomEventList = null;
    public float[] eventRatio = new float[] { 0.2f, 0.3f, 0.5f };
    public PollutionLevelInfo[] pollutionInfos;
    public PlayerData playerData; // Editor�Ŋm�F�p�A���Ƃō폜

    public override void InitAwake()
    {
        // CanvasType�������������m��
        canvases = new Canvas[(int)CanvasType.Max];
    }

    // �Q�Ɖ�ʃT�C�Y�̃A�X�y�N�g��̎擾
    public float GetRefAspectRatio()
    {
        return refScreenSize.x / refScreenSize.y;
    }

    // �Q�Ɖ�ʃT�C�Y�̃A�X�y�N�g��̋t���̎擾
    public float GetRefAspectRatioReciprocal()
    {
        return refScreenSize.y / refScreenSize.x;
    }

    // Canvas�̔z��Ɍ��݂�Canvas���i�[
    public void SetCanvas(CanvasType Type, Canvas This)
    {
        canvases[(int)Type] = This;
    }

    public void SetMapSize(Vector2 MapSize)
    {
        mapSize = MapSize;
        halfMapSize = mapSize * 0.5f;
    }

    // EventButton��Event�̏���^����
    public EventSO CreateEventSO(BaseIndex Index)
    {
        return baseList[(int)Index];
    }

    public EventSO CreateEventSO()
    {
        float ratio = UnityEngine.Random.Range(0.0f, 1.0f);
        if (ratio < eventRatio[(int)EventSOType.MainEvent])
        {
            int amaIndex = (int)Player.instance.GetCurrentAMA();
            int eventIndex = UnityEngine.Random.Range(0, mainEventLists[amaIndex].mainEvents.Count);
            return mainEventLists[amaIndex].mainEvents[eventIndex];
        }
        else if (ratio < eventRatio[(int)EventSOType.MainEvent] + eventRatio[(int)EventSOType.SubEvent])
        {
            return subEventList[UnityEngine.Random.Range(0, subEventList.Count)];
        }
        else
        {
            return CreateRandomEventSO();
        }
    }

    public EventSO CreateRandomEventSO()
    {
        return randomEventList[UnityEngine.Random.Range(0, randomEventList.Count)];
    }

    public int GetMaxAMAEnegry()
    {
        int[] enegry = new int[amaList.Count];
        for (int i = 0; i < amaList.Count; ++i)
        {
            enegry[i] = amaList[i].energy;
        }
        return Mathf.Max(enegry);
    }
}
