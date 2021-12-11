using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementView : Monosingleton<AchievementView>
{
    [SerializeField] private SubEventAchievementFrame[] frames = null;

    public void Init()
    {
        foreach (SubEventAchievementFrame frame in frames)
        {
            frame.Init();
        }

        gameObject.SetActive(false);
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
