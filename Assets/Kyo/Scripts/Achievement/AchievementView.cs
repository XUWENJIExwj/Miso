using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[Serializable]
public struct EventAchievements
{
    public bool[] completed;

    public void Init(int Count)
    {
        completed = new bool[Count];
        for (int i = 0; i < completed.Length; ++i)
        {
            completed[i] = false;
        }
    }

    public float Progress()
    {
        int count = 0;
        foreach (bool c in completed)
        {
            if (c) ++count;
        }
        return (float)count / completed.Length;
    }
}

[Serializable]
public struct AchievementProgress
{
    public EventAchievements[] main;
    public EventAchievements sub;
    public EventAchievements random;

    public void Init()
    {
        InitMainEventAchievements();
        InitSubEventAchievements();
        InitRandomEventAchievements();
    }

    private void InitMainEventAchievements()
    {
        main = new EventAchievements[GlobalInfo.instance.amaList.Count];
        for (int i = 0; i < main.Length; ++i)
        {
            main[i].Init(GlobalInfo.instance.mainEventLists[i].mainEvents.Count);
        }
    }

    private void InitSubEventAchievements()
    {
        sub.Init(GlobalInfo.instance.subEventList.Count);
    }

    private void InitRandomEventAchievements()
    {
        random.Init(GlobalInfo.instance.randomEventList.Count);
    }
}

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
        SoundManager.instance.SE_Tap();

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
        SoundManager.instance.SE_Tap();

        // ‰¼
        MainGameLogic logic = LogicManager.instance.GetSceneLogic<MainGameLogic>();
        logic.SetNextSate(MainGameState.RouteSelect);

        gameObject.SetActive(false);
    }
}
