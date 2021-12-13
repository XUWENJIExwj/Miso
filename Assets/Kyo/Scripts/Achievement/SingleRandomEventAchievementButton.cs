using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleRandomEventAchievementButton : SingleSubEventAchievementButton
{
    public override void SetAchievementInfo(AMAs AMA)
    {
        if (eventSO)
        {
            if (Player.instance.GetPlayerData().achievements.random.completed[eventSO.id])
            {
                icon.color = Color.white;
                iconFrame.SetColor(Color.white);
            }
            else
            {
                icon.color = Color.grey;
                iconFrame.SetColor(Color.grey);
            }
        }
    }

    public override void StartAnimateMetalFrame()
    {
        if (Player.instance.GetPlayerData().achievements.random.completed[eventSO.id])
        {
            iconFrame.StartAnimateFrame();
        }
    }

    public override void StopAnimateMetalFrame()
    {
        if (Player.instance.GetPlayerData().achievements.random.completed[eventSO.id])
        {
            iconFrame.StopAnimateFrame();
        }
    }
}
