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
    [SerializeField] private TMP_Text pointText = null;
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

    // Resultの出現（アニメーション付き）
    public void AppearResult()
    {
        if (results.Count > 0)
        {
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
        pointText.text = "Point: " + Player.instance.GetCurrentPoint().ToString();
    }

    public void OnClick()
    {
        if (ResultTweenerActive())
        {
            ShowResult();
            return;
        }

        resultCount = 0;
        currentPoint = 0;

        Player.instance.ResetCurrentPoint();

        eventSO.SetNextPhase(BaseEventPhase.Phase_End);

        // DataBase登録
        //Player.instance.GetTotalPoint();
        FindObjectOfType<Score>().Save();
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

        while(currentPoint < TargetPoint)
        {
            currentPoint += step;

            if(currentPoint >= TargetPoint)
            {
                currentPoint = TargetPoint;
                pointText.text = "Point: " + currentPoint.ToString();
                yield break;
            }

            pointText.text = "Point: " + currentPoint.ToString();
            yield return new WaitForSeconds(interval);
        }
    }
}
