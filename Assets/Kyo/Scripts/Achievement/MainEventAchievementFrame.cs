using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainEventAchievementFrame : SubEventAchievementFrame
{
    [SerializeField] private AMAs ama = AMAs.Max;

    public override void Init(AMAs AMA = AMAs.Max)
    {
        ama = AMA;
        eventType.text += "Åi" + GlobalInfo.instance.amaList[(int)ama].ama + "Åj";

        int line = Mathf.CeilToInt((float)GlobalInfo.instance.mainEventLists[(int)ama].mainEvents.Count / prefab.AchievementCount());
        frames = new SubEventAchievementSubFrame[line];
        for (int i = 0; i < line; ++i)
        {
            frames[i] = Instantiate(prefab, transform);
            frames[i].Init(i, ama);
        }
    }

    public override void SetAchivementsInfo()
    {
        progress.text = Player.instance.GetPlayerData().achievements.main[(int)ama].Progress().ToString("P");

        foreach (SubEventAchievementSubFrame frame in frames)
        {
            frame.SetAchivementsInfo(ama);
        }
    }
}
