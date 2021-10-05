using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameCanvasController : BaseCanvasController
{
    [SerializeField] private CanvasType canvasType;

    public override void Init()
    {
        // Canvas‚Ì”z—ñ‚ÉŒ»İ‚ÌCanvas‚ğŠi”[
        GlobalInfo.instance.SetCanvas(canvasType, GetComponent<Canvas>());
    }
}
