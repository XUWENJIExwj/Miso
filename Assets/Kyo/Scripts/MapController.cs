using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapController : Monosingleton<MapController>
{
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private Image image;

    public override void Init()
    {
        rectTransform.sizeDelta = 
            new Vector2(GlobalInfo.instance.refScreenSize.y / rectTransform.sizeDelta.y * rectTransform.sizeDelta.x, GlobalInfo.instance.refScreenSize.y);

        // 拡大縮小
        image.material.mainTextureScale = new Vector2(1.0f, 1.0f);
    }

    private void Update()
    {
        Move();
    }

    public void Move()
    {
        // UVアニメーション
        image.material.mainTextureOffset = 
            new Vector2(image.material.mainTextureOffset.x + 0.1f * Time.deltaTime, image.material.mainTextureOffset.y);
    }
}
