using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubEventAchievementSubFrame : MonoBehaviour
{
    [SerializeField] private SingleSubEventAchievementButton[] achievements = null;

    public void Init(int LineIndex)
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
                achievements[i].gameObject.SetActive(false);
            }
        }
    }

    public virtual void SetAchivementsInfo()
    {
        foreach (SingleSubEventAchievementButton achievement in achievements)
        {
            achievement.SetAchievementInfo();
        }
    }

    public int AchievementCount()
    {
        return achievements.Length;
    }
}
