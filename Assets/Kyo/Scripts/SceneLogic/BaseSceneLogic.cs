using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseSceneLogic : MonoBehaviour
{
    public abstract void UpdateScene();

    public void OnEnable()
    {
        SetCurrentSenceLogic();
    }

    public void SetCurrentSenceLogic()
    {
        LogicManager.instance.SetCurrentSenceLogic(this);
    }
}
