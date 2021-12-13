using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainEventAchievementSubFrame : SubEventAchievementSubFrame
{
    public override void Init(int LineIndex, AMAs AMA)
    {
        for (int i = 0; i < achievements.Length; ++i)
        {
            int id = LineIndex * achievements.Length + i;

            if (id < GlobalInfo.instance.mainEventLists[(int)AMA].mainEvents.Count)
            {
                achievements[i].Init(GlobalInfo.instance.mainEventLists[(int)AMA].mainEvents[id]);
            }
            else
            {
                achievements[i].Hide();
            }
        }
    }
}
