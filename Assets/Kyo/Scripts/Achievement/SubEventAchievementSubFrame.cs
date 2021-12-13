using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubEventAchievementSubFrame : MonoBehaviour
{
    [SerializeField] protected SingleSubEventAchievementButton[] achievements = null;

    public virtual void Init(int LineIndex, AMAs AMA = AMAs.Max)
    {
        for (int i = 0; i < achievements.Length; ++i)
        {
            int id = LineIndex * achievements.Length + i;

            if (id < GlobalInfo.instance.subEventList.Count)
            {
                achievements[i].Init(GlobalInfo.instance.subEventList[id]);
            }
            else
            {
                achievements[i].Hide();
            }
        }
    }

    public void SetAchivementsInfo(AMAs AMA = AMAs.Max)
    {
        foreach (SingleSubEventAchievementButton achievement in achievements)
        {
            achievement.SetAchievementInfo(AMA);
        }
    }

    public int AchievementCount()
    {
        return achievements.Length;
    }
}
