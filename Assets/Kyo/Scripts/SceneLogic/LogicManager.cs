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

    public void Start()
    {
        InitSound();
        ChangeBGM.instance.BGM_Route();

        if (onTest)
        {
            //SceneManager.LoadScene("GameMap", LoadSceneMode.Additive);
        }
        else
        {
            SceneManager.LoadScene("LogIn", LoadSceneMode.Additive);
        }
    }

    public void InitSound()
    {
        ChangeBGM.instance.Init();
        SoundManager.instance.Init();
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

    public void GameStart()
    {
        SceneManager.LoadScene("GameMap", LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync("LogIn");
    }
}
