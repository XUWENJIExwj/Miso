using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneType
{
    LoginScene,
    TitleScene,
    MainGameScene,
}

public class LogicManager : Monosingleton<LogicManager>
{
    [SerializeField] SceneType scene = SceneType.MainGameScene;
    [SerializeField] bool onTest = true;
    [SerializeField] BaseSceneLogic sceneLogic = null;

    public override void InitAwake()
    {
        Test();
    }

    void Update()
    {
        UpdateScene();
    }

    void UpdateScene()
    {
        if (sceneLogic)
        {
            sceneLogic.UpdateScene();
        }
    }

    private void Test()
    {
        if (!onTest)
        {
            switch(scene)
            {
                case SceneType.LoginScene:
                    break;
                case SceneType.TitleScene:
                    SceneManager.LoadScene("TitleScene", LoadSceneMode.Additive);
                    break;
                case SceneType.MainGameScene:
                    SceneManager.LoadScene("GameMap", LoadSceneMode.Additive);
                    break;
                default:
                    break;
            }
        }
    }

    public void SetCurrentSenceLogic(BaseSceneLogic SceneLogic)
    {
        sceneLogic = SceneLogic;
    }

    public T GetSceneLogic<T>()
    {
        if (sceneLogic.GetType() == typeof(T))
        {
            return (T)(object)sceneLogic;
        }

        Debug.Log("Žæ“¾‚µ‚æ‚¤‚Æ‚·‚éŒ^‚Æˆá‚¤!");
        return (T)(object)null;
    }
}
