using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleRandomEventAchievementButton : SingleSubEventAchievementButton
{
    public override void SetAchievementInfo(AMAs AMA = AMAs.Max)
    {
        if (eventSO && Player.instance.GetPlayerData().achievements.random.completed[eventSO.id])
        {
            icon.color = Color.white;
        }
        else
        {
            icon.color = Color.grey;
        }
    }
}
