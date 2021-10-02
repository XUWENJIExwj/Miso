using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameCanvasController : BaseCanvasController
{
    public override void Init()
    {
        // Canvas�̔z��Ɍ��݂�Canvas���i�[
        GlobalInfo.instance.SetCanvas(CanvasType.MainGame, GetComponent<Canvas>());
    }
}
