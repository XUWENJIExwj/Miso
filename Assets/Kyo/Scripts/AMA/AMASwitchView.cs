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
        playerData = Data;
        baseName.text = playerData.basePoint.GetBaseName();
        currentAMA = playerData.ama;

        SetAMAInfo();

        gameObject.SetActive(true);

        // ��
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
        amaInfo.speed.text = ama.timePerGrid.ToString() + "s/�}�X";
        amaInfo.energy.text = ama.energy.ToString() + "�}�X";
        amaInfo.nature.text = ama.nature;

        newFlag.gameObject.SetActive(ama.newFlag);
        ama.newFlag = false;
    }

    public void AMAConfirm()
    {
        Player.instance.SetCurrentAMA(currentAMA);
        EventButtonManager.instance.LinkMainEventsToAMA(currentAMA);

        // ��
        MainGameLogic logic = LogicManager.instance.GetSceneLogic<MainGameLogic>();
        logic.SetNextSate(MainGameState.RouteSelect);
        gameObject.SetActive(false);
    }

    public void AMACancel()
    {
        currentAMA = playerData.ama;

        // ��
        MainGameLogic logic = LogicManager.instance.GetSceneLogic<MainGameLogic>();
        logic.SetNextSate(MainGameState.RouteSelect);
        gameObject.SetActive(false);
    }

    public void SwitchAMA(int Offset)
    {
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