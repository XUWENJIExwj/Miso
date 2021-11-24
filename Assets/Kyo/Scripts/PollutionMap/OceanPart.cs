using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(CleanupObserver))]
public class OceanPart : MonoBehaviour
{
    [SerializeField] protected Image image = null;
    [SerializeField] protected PollutionLevel level = PollutionLevel.Level_05;
    [SerializeField] protected float fadeTime = 0.5f;
    [SerializeField] protected CleanupView prefab = null;
    [SerializeField] protected CleanupObserver observer = null;

    public virtual void Init(Oceans Ocean, OceanAreas Area)
    {
        image.color = GlobalInfo.instance.pollutionInfos[(int)level].color;
        observer.Init(Ocean, Area);
    }

    public void SetPollutionLevel(EventButton Event)
    {
        if (level > PollutionLevel.Level_00)
        {
            int point = AddCleanupPoint(Event);
            UpdatePollutionLevel(point);
        }
    }

    public virtual void AddResult()
    {
        observer.AddResult();
        observer.ResetObserver();
    }

    public void UpdatePollutionLevel(int Point)
    {
        level -= 1;
        observer.ListenCleanup(level, Point);
        SetPollutionColor();
    }

    public void SetPollutionColor()
    {
        image.DOColor(GlobalInfo.instance.pollutionInfos[(int)level].color, fadeTime);
    }

    public virtual int AddCleanupPoint(EventButton Event)
    {
        int point = HelperFunction.RandomPointRange(GlobalInfo.instance.pollutionInfos[(int)level].pointRange);
        ShowCleanupView(point, Event);
        Player.instance.AddPoint(point);

        return point;
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
