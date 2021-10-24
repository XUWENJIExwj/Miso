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

    public void MovePath()
    {
        EventButton nextPoint = RouteManager.instance.GetNextRoutePoint();

        if (nextPoint)
        {
            MainGameLogic logic = LogicManager.instance.GetSceneLogic<MainGameLogic>();
            logic.SetNextSate(MainGameState.RouteMove);

            tweener = transform.DOLocalMove(nextPoint.transform.localPosition, 1.0f);
            tweener.SetEase(Ease.Linear);
            tweener.OnComplete(() =>
            {
                if (!nextPoint.IsBase())
                {
                    nextPoint.DoScaleDown();
                }
                
                logic.SetCurrentEvent(nextPoint);
                logic.SetNextSate(MainGameState.EventPlayPre);
            });
        }

        EventUIManager.instance.ResetEventInfo();
    }
}
