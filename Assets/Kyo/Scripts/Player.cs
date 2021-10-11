using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour
{
    [SerializeField] private Tween tweener = null;

    public void SetPosition(Vector2 Offset)
    {
        transform.localPosition += new Vector3(Offset.x, Offset.y, 0.0f);

        // 画面外になったら、ループさせる
        FixPostion();
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

        // Y軸
        if (transform.localPosition.y < -GlobalInfo.instance.halfMapSize.y)
        {
            fixPos.y = transform.localPosition.y + GlobalInfo.instance.mapSize.y;
        }
        else if (transform.localPosition.y > GlobalInfo.instance.halfMapSize.y)
        {
            fixPos.y = transform.localPosition.y - GlobalInfo.instance.mapSize.y;
        }

        transform.localPosition = fixPos;
    }

    public void MovePath(Vector3[] Points)
    {
        if(!tweener.IsActive())
        {
            tweener = transform.DOLocalPath(Points, Points.Length, PathType.Linear, PathMode.Ignore).SetEase(Ease.Linear);
            tweener.OnComplete(() => { tweener = transform.DOLocalPath(Points, Points.Length, PathType.Linear, PathMode.Ignore).SetEase(Ease.Linear); });
        }
    }
}
