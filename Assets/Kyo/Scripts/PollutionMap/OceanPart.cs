using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class OceanPart : MonoBehaviour
{
    [SerializeField] protected Image image = null;
    [SerializeField] protected PollutionLevel level = PollutionLevel.Level_05;
    [SerializeField] protected float fadeTime = 0.5f;
    [SerializeField] protected CleanupView prefab = null;

    public virtual void Init()
    {
        image.color = GlobalInfo.instance.pollutionInfos[(int)level].color;
    }

    public void SetPollutionLevel(EventButton Event)
    {
        if (level > PollutionLevel.Level_00)
        {
            level -= 1;

            SetPollutionColor();
            AddCleanupPoint(Event);
        }
    }

    public void SetPollutionColor()
    {
        image.DOColor(GlobalInfo.instance.pollutionInfos[(int)level].color, fadeTime);
    }

    public virtual void AddCleanupPoint(EventButton Event)
    {
        int point = HelperFunction.RandomPointRange(GlobalInfo.instance.pollutionInfos[(int)level].pointRange);
        ShowCleanupView(point, Event);
        Player.instance.AddPoint(point);
    }

    public void ShowCleanupView(int Point, EventButton Event)
    {
        CleanupView view = Instantiate(prefab, EventButtonManager.instance.transform);
        view.ShowCleanupView(Point, Event);
    }

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
