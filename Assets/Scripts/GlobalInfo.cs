using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// �e�V�[���ɂ���Canvas
public enum CanvasType
{
    MainGame_Camera,
    MainGame_Overlay,
    Max,
}

public class GlobalInfo : Monosingleton<GlobalInfo>
{
    public Vector2 refScreenSize = new Vector2(1080.0f, 1920.0f); // �Q�Ɖ�ʃT�C�Y
    public Canvas[] canvases; // �eCanvas
    public Vector2 mapSize;
    public Vector2 halfMapSize;

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
}
