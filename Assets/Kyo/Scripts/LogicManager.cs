using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogicManager : Monosingleton<LogicManager>
{
    [SerializeField] bool onTest = true;

    public override void Init()
    {
        if(!onTest)
        {
            SceneManager.LoadScene("GameMap", LoadSceneMode.Additive);
        }
    }
}
