using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class NewAMAView : Monosingleton<NewAMAView>
{
    [SerializeField] private Image frame = null;
    [SerializeField] private Image ama = null;
    [SerializeField] private Text newText = null;
    [SerializeField] private float fadeTime = 0.5f;

    public void Init()
    {
        gameObject.SetActive(false);
    }

    public void ActiveNewAMAView(AMAs AMA)
    {
        // 仮
        MainGameLogic logic = LogicManager.instance.GetSceneLogic<MainGameLogic>();
        logic.SetNextSate(MainGameState.NewAMAPre);

        AMASO amaSO = GlobalInfo.instance.amaList[(int)AMA];
        ama.sprite = amaSO.icon;
        newText.text = "AMAの" + amaSO.ama + "をゲット！\nAMA切替画面で確認しよう！";
        gameObject.SetActive(true);

        Tweener tweener = frame.DOFade(50.0f / 255.0f, fadeTime);
        tweener.OnUpdate(() =>
        {
            ama.color = HelperFunction.ChangeAlpha(ama.color, frame.color.a / (50.0f / 255.0f));
            newText.color = HelperFunction.ChangeAlpha(newText.color, frame.color.a / (50.0f / 255.0f));
        });
        tweener.OnComplete(() =>
        {
            ama.color = HelperFunction.ChangeAlpha(ama.color, frame.color.a / (50.0f / 255.0f));
            newText.color = HelperFunction.ChangeAlpha(newText.color, frame.color.a / (50.0f / 255.0f));

            // 仮
            logic.SetNextSate(MainGameState.NewAMA);
        });
    }

    public void EndNewAMAView()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // 仮
            MainGameLogic logic = LogicManager.instance.GetSceneLogic<MainGameLogic>();
            logic.SetNextSate(MainGameState.RouteSelect);

            gameObject.SetActive(false);
        }
    }

    public void OnEnable()
    {
        frame.color = HelperFunction.ChangeAlpha(frame.color, 0.0f);
        ama.color = HelperFunction.ChangeAlpha(ama.color, 0.0f);
        newText.color = HelperFunction.ChangeAlpha(newText.color, 0.0f);
    }
}
