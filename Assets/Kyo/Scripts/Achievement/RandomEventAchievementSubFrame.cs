using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomEventAchievementSubFrame : SubEventAchievementSubFrame
{
    public override void Init(int LineIndex, AMAs AMA = AMAs.Max)
    {
        for (int i = 0; i < achievements.Length; ++i)
        {
            int id = LineIndex * achievements.Length + i;

            if (id < GlobalInfo.instance.randomEventList.Count)
            {
                achievements[i].Init(GlobalInfo.instance.randomEventList[id]);
            }
            else
            {
                achievements[i].Hide();
            }
        }
    }
}
