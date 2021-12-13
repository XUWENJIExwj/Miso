using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AMASwitchView : Monosingleton<AMASwitchView>
{
    [SerializeField] private PlayerData playerData;
    [SerializeField] private TMP_Text baseName = null;
    [SerializeField] private AMAInfo amaInfo;
    [SerializeField] private TMP_Text newFlag = null;
    [SerializeField] private AMAs currentAMA = AMAs.Max;

    public void Init()
    {
        gameObject.SetActive(false);
    }

    public void ActiveSwitchView(PlayerData Data)
    {
        SoundManager.instance.SE_Tap();

        playerData = Data;
        baseName.text = playerData.basePoint.GetBaseName();
        currentAMA = playerData.ama;

        SetAMAInfo();

        gameObject.SetActive(true);

        // 仮
        MainGameLogic logic = LogicManager.instance.GetSceneLogic<MainGameLogic>();
        logic.SetNextSate(MainGameState.AMASwitch);
    }

    public void SetAMAInfo()
    {
        CharacterTypes characterTypes = AMASO.ConvertAMAsToCharacterTypes(currentAMA);
        AMASO ama = GlobalInfo.instance.amaList[(int)currentAMA];
        amaInfo.image.sprite = DictionaryManager.instance.GetTargetIllustration(characterTypes, ExpressionTypes.Normal);
        amaInfo.name.text = ama.ama;
        amaInfo.type.text = DictionaryManager.instance.GetAMAType(ama.type);
        amaInfo.speed.text = ama.timePerGrid.ToString() + "s/マス";
        amaInfo.energy.text = ama.energy.ToString() + "マス";
        amaInfo.nature.text = ama.nature;

        newFlag.gameObject.SetActive(ama.newFlag);
        ama.newFlag = false;
    }

    public void AMAConfirm()
    {
        SoundManager.instance.SE_Tap();

        Player.instance.SetCurrentAMA(currentAMA);
        EventButtonManager.instance.LinkMainEventsToAMA(currentAMA);

        // 仮
        MainGameLogic logic = LogicManager.instance.GetSceneLogic<MainGameLogic>();
        logic.SetNextSate(MainGameState.RouteSelect);
        gameObject.SetActive(false);
    }

    public void AMACancel()
    {
        SoundManager.instance.SE_Tap();

        currentAMA = playerData.ama;

        // 仮
        MainGameLogic logic = LogicManager.instance.GetSceneLogic<MainGameLogic>();
        logic.SetNextSate(MainGameState.RouteSelect);
        gameObject.SetActive(false);
    }

    public void SwitchAMA(int Offset)
    {
        SoundManager.instance.SE_Tap();

        CheckUnlockedAMA(Offset);
        SetAMAInfo();
    }

    public void CheckUnlockedAMA(int Offset)
    {
        currentAMA = (AMAs)(((int)currentAMA + Offset + (int)AMAs.Max) % (int)AMAs.Max);

        if (!Player.instance.CheckUnlockedAMA(currentAMA))
        {
            CheckUnlockedAMA(Offset);
        }
    }
}
