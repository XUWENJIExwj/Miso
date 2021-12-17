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
        if (onTest)
        {
            //SceneManager.LoadScene("GameMap", LoadSceneMode.Additive);
        }
        else
        {
            SceneManager.LoadScene("LogIn", LoadSceneMode.Additive);
        }
    }

    public void Start()
    {
        InitSound();
        ChangeBGM.instance.BGM_Route();
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

    public void ToStartGame()
    {
        StartCoroutine(WaitLoadSaveData());
    }

    public void GameStart()
    {
        SceneManager.LoadScene("GameMap", LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync("LogIn");
    }

    public IEnumerator WaitLoadSaveData()
    {
        yield return new WaitForSeconds(5.0f);
        //yield return new WaitUntil(() => Score.instance.ComleteFetch());
        GameStart();
    }

    public bool OnTest()
    {
        return onTest;
    }
}
