using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class OceanPart : MonoBehaviour
{
    [SerializeField] Image image = null;

    public void Move(Vector2 Offset)
    {
        transform.localPosition += new Vector3(Offset.x, Offset.y, 0.0f);

        // 画面外になったら、ループさせる
        FixPostion();
    }

    public void MovePath(Vector2 Offset, float Time)
    {
        Tweener tweener = transform.DOLocalMoveX(transform.localPosition.x - Offset.x, Time);
        tweener.SetEase(Ease.Linear);
        tweener.OnComplete(() => { FixPostion(); });
    }

    // 画面外になったら、ループさせる
    public void FixPostion()
    {
        Vector2 fixPos = transform.localPosition;

        // X軸
        if (transform.localPosition.x < -GlobalInfo.instance.halfMapSize.x)
        {
            fixPos.x = transform.localPosition.x + GlobalInfo.instance.mapSize.x;
        }
        else if (transform.localPosition.x > GlobalInfo.instance.halfMapSize.x)
        {
            fixPos.x = transform.localPosition.x - GlobalInfo.instance.mapSize.x;
        }

        transform.localPosition = fixPos;
    }
}
