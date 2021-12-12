using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomEventAchievementFrame : SubEventAchievementFrame
{
    public override void Init(AMAs AMA = AMAs.Max)
    {
        int line = Mathf.CeilToInt((float)GlobalInfo.instance.randomEventList.Count / prefab.AchievementCount());
        frames = new SubEventAchievementSubFrame[line];
        for (int i = 0; i < line; ++i)
        {
            frames[i] = Instantiate(prefab, transform);
            frames[i].Init(i);
        }
    }

    public override void SetAchivementsInfo()
    {
        progress.text = Player.instance.GetPlayerData().achievements.random.Progress().ToString("P");

        foreach (SubEventAchievementSubFrame frame in frames)
        {
            frame.SetAchivementsInfo();
        }
    }
}
