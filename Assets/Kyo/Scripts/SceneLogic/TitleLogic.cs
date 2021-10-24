using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleLogic : BaseSceneLogic
{
    public override void UpdateScene()
    {
        ChangeScene();
    }

    public void ChangeScene()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SceneManager.LoadScene("GameMap", LoadSceneMode.Additive);
            SceneManager.UnloadSceneAsync("TitleScene");
        }
    }
}
