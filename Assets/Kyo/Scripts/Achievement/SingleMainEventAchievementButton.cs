using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleMainEventAchievementButton : SingleSubEventAchievementButton
{
    public override void SetAchievementInfo(AMAs AMA)
    {
        if (eventSO && Player.instance.GetPlayerData().achievements.main[(int)AMA].completed[eventSO.id])
        {
            icon.color = Color.white;
        }
        else
        {
            icon.color = Color.grey;
        }
    }
}
