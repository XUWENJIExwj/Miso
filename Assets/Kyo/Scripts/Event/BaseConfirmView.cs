using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventScriptableObject;
using TMPro;
using System;
using UnityEngine.UI;

[Serializable]
public struct AMAInfo
{
    public Image image;
    public TMP_Text name;
    public TMP_Text type;
    public TMP_Text speed;
    public TMP_Text energy;
    public TMP_Text nature;
}

public class BaseConfirmView : Monosingleton<BaseConfirmView>
{
    [SerializeField] private BaseButton baseSelected = null;
    [SerializeField] private TMP_Text baseName = null;
    [SerializeField] private AMAInfo amaInfo;

    public void Init()
    {
        gameObject.SetActive(false);
    }

    public void ActiveConfirmView(BaseButton Base)
    {
        baseSelected = Base;
        baseName.text = baseSelected.GetBaseName();
        InitAMAInfo();

        gameObject.SetActive(true);

        // 仮
        MainGameLogic logic = LogicManager.instance.GetSceneLogic<MainGameLogic>();
        logic.SetNextSate(MainGameState.BaseConfirm);
    }

    public void InitAMAInfo()
    {
        AMAs amaIndex = baseSelected.GetAMA();
        CharacterTypes characterTypes = AMASO.ConvertAMAsToCharacterTypes(amaIndex);
        AMASO ama = GlobalInfo.instance.amaList[(int)amaIndex];
        amaInfo.image.sprite = DictionaryManager.instance.GetTargetIllustration(characterTypes, ExpressionTypes.Normal);
        amaInfo.name.text = ama.ama;
        amaInfo.type.text = DictionaryManager.instance.GetAMAType(ama.type);
        amaInfo.speed.text = ama.timePerGrid.ToString() + "s/マス";
        amaInfo.energy.text = ama.energy.ToString() + "マス";
        amaInfo.nature.text = ama.nature;
    }

    public void BaseConfirm()
    {
        // Playerの初期位置
        Player.instance.SetFirstBase(baseSelected);
        Player.instance.AddAMA(baseSelected.GetAMA(), true);

        // Eventの初期化
        EventButtonManager.instance.CreateEvents();

        // PlayerのBaseをStartPointに登録
        RouteManager.instance.SetStartPoint();

        //FuelGauge.instance.ResetMaxValue();

        baseSelected = null;

        // 仮
        MainGameLogic logic = LogicManager.instance.GetSceneLogic<MainGameLogic>();
        logic.SetNextSate(MainGameState.RouteSelectPre);
        gameObject.SetActive(false);
    }

    public void BaseCancel()
    {
        baseSelected.DoScaleDown();
        baseSelected = null;

        // 仮
        MainGameLogic logic = LogicManager.instance.GetSceneLogic<MainGameLogic>();
        logic.SetNextSate(MainGameState.BaseSelect);

        gameObject.SetActive(false);
    }
}
