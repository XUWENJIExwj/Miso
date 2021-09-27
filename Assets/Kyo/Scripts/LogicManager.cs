using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogicManager : Monosingleton<LogicManager>
{
    public override void Init()
    {
        SceneManager.LoadScene("GameMap", LoadSceneMode.Additive);
    }
}
