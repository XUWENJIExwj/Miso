using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum TestFirstScene
{
    LoginScene,
    MainGameScene,
}

public class LogicManager : Monosingleton<LogicManager>
{
    [SerializeField] TestFirstScene testFirstScene = TestFirstScene.MainGameScene;
    [SerializeField] bool onTest = true;

    public override void InitAwake()
    {
        Test();
    }

    private void Test()
    {
        if(!onTest)
        {
            switch(testFirstScene)
            {
                case TestFirstScene.LoginScene:
                    break;
                case TestFirstScene.MainGameScene:
                    SceneManager.LoadScene("GameMap", LoadSceneMode.Additive);
                    break;
                default:
                    break;
            }
        }
    }
}
