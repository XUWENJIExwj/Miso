using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventScriptableObject;

public class SingleMainEventAchievementButton : SingleSubEventAchievementButton
{
    public override void SetAchievementInfo(AMAs AMA)
    {
        if (eventSO)
        {
            if (Player.instance.GetPlayerData().achievements.main[(int)AMA].completed[eventSO.id])
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

    public override void PlaySE()
    {
        SoundManager.instance.SE_MainEvent();
    }

    public override void StartAnimateMetalFrame()
    {
        MainEventSO main = (MainEventSO)eventSO;
        //if (Player.instance.GetPlayerData().achievements.main[(int)main.ama].completed[main.id])
        {
            iconFrame.StartAnimateFrame();
        }
    }

    public override void StopAnimateMetalFrame()
    {
        MainEventSO main = (MainEventSO)eventSO;
        //if (Player.instance.GetPlayerData().achievements.main[(int)main.ama].completed[main.id])
        {
            iconFrame.StopAnimateFrame();
        }
    }
}
