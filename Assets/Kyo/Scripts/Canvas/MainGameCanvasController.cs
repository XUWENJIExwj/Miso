using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameCanvasController : BaseCanvasController
{
    [SerializeField] private CanvasType canvasType;

    public override void Init()
    {
        // Canvas�̔z��Ɍ��݂�Canvas���i�[
        GlobalInfo.instance.SetCanvas(canvasType, GetComponent<Canvas>());
    }
}
