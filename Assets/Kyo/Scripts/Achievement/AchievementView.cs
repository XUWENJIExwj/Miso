using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementView : Monosingleton<AchievementView>
{
    [SerializeField] private ScrollRect view = null;
    [SerializeField] private SubEventAchievementFrame[] prefabs = null; // 0: Main, 1: Sub, 2: Random
    [SerializeField] private List<SubEventAchievementFrame> frames = null;

    public void Init()
    {
        frames = new List<SubEventAchievementFrame>();

        InitMainEventAchievementFrame();
        InitSubEventAchievementFrame();
        InitRandomEventAchievementFrame();

        gameObject.SetActive(false);
    }

    public void InitMainEventAchievementFrame()
    {
        for (AMAs i = AMAs.Higashi; i < AMAs.Max; ++i)
        {
            SubEventAchievementFrame frame = Instantiate(prefabs[0], view.content.transform);
            frame.Init(i);
            frames.Add(frame);
        }
    }

    public void InitSubEventAchievementFrame()
    {
        SubEventAchievementFrame frame = Instantiate(prefabs[1], view.content.transform);
        frame.Init();
        frames.Add(frame);
    }

    public void InitRandomEventAchievementFrame()
    {
        SubEventAchievementFrame frame = Instantiate(prefabs[2], view.content.transform);
        frame.Init();
        frames.Add(frame);
    }

    public void ActiveAchievementView()
    {
        // ‰¼
        MainGameLogic logic = LogicManager.instance.GetSceneLogic<MainGameLogic>();
        logic.SetNextSate(MainGameState.Achievement);

        foreach (SubEventAchievementFrame frame in frames)
        {
            frame.SetAchivementsInfo();
        }

        gameObject.SetActive(true);
    }

    public void EndAchievementView()
    {
        // ‰¼
        MainGameLogic logic = LogicManager.instance.GetSceneLogic<MainGameLogic>();
        logic.SetNextSate(MainGameState.RouteSelect);

        gameObject.SetActive(false);
    }
}
