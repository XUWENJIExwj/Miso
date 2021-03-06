using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventScriptableObject;
using UnityEngine.UI;
using TMPro;

public class BaseEventUI : EventUI
{
    [SerializeField] private BaseEventSO eventSO = null;
    [SerializeField] private ResultConfirmButton button = null;
    [SerializeField] private ScrollRect resultView = null;
    [SerializeField] private Result[] prefabs = null;
    [SerializeField] private Queue<Result> results = null;
    [SerializeField] private Result currentResult = null;
    [SerializeField] private int resultCount = 0;
    [SerializeField] private Text pointText = null;
    [SerializeField] private int currentPoint = 0;
    [SerializeField] private float fadeTime = 0.3f;

    public void Awake()
    {
        results = new Queue<Result>();
    }

    public override void EventPlayPre(EventButton Event)
    {
        gameObject.SetActive(true);
        eventSO = Event.GetEventSO<BaseEventSO>();
        eventSO.EventStart();
    }

    public override void EventPlay()
    {
        eventSO.EventPlay();
    }

    public void AddResult(EventSO Event)
    {
        Result result = Instantiate(prefabs[(int)Event.type], resultView.content);
        result.Init(Event);
        results.Enqueue(result);

        resultView.verticalScrollbar.value = 1.0f;
        resultCount = results.Count;
    }

    public void AddResult(CleanupObserver Observer)
    {
        Result result = Instantiate(prefabs[prefabs.Length - 1], resultView.content);
        result.Init(Observer);
        results.Enqueue(result);

        resultView.verticalScrollbar.value = 1.0f;
        resultCount = results.Count;
    }

    // Resultの出現（アニメーション付き）
    public void AppearResult()
    {
        if (results.Count > 0)
        {
            SoundManager.instance.SE_Result2();

            if (resultCount - results.Count >= 4)
            {
                resultView.verticalScrollbar.value -= 1.0f / (resultCount - 4);
            }
            currentResult = results.Dequeue();
            currentResult.Appear();

            StartCoroutine(AddPoint(currentPoint + currentResult.GetPoint(), fadeTime));
        }
        else
        {
            currentResult = null;
        }
    }

    // Resultの出現（アニメーションなし）
    public void ShowResult()
    {
        SoundManager.instance.SE_Tap();

        StopAllCoroutines();

        if (currentResult)
        {
            currentResult.Show();
            currentResult = null;
        }

        foreach (Result result in results)
        {
            result.Show();
        }
        results.Clear();

        resultView.verticalScrollbar.value = 0.0f;
        pointText.text = Player.instance.GetCurrentPoint().ToString();
    }

    public void OnClick()
    {
        if (ResultTweenerActive())
        {
            ShowResult();
            return;
        }

        SoundManager.instance.SE_Tap();

        resultCount = 0;
        currentPoint = 0;
        resultView.verticalScrollbar.value = 1.0f;

        Player.instance.ResetCurrentPoint();
        Player.instance.CompleteCourse();
        RouteManager.instance.ActiveButtons(true);

        eventSO.SetNextPhase(BaseEventPhase.Phase_End);

        // DataBase登録
        Player.instance.Save();
    }

    public bool ConfirmButtonPointerEnter()
    {
        return button.PointerEnter();
    }

    public bool ResultTweenerActive()
    {
        return Result.TweenerActive();
    }

    public IEnumerator AddPoint(int TargetPoint, float Time, int Step = 20)
    {
        float interval = Time / Step;
        int step = (TargetPoint - currentPoint) / Step;

        while (currentPoint != TargetPoint)
        {
            currentPoint += step;

            if (step > 0)
            {
                if (currentPoint >= TargetPoint)
                {
                    currentPoint = TargetPoint;
                    pointText.text = currentPoint.ToString();
                    yield break;
                }
            }
            else
            {
                if (currentPoint <= TargetPoint)
                {
                    currentPoint = TargetPoint;
                    pointText.text = currentPoint.ToString();
                    yield break;
                }
            }

            pointText.text = currentPoint.ToString();
            yield return new WaitForSeconds(interval);
        }
    }
}
