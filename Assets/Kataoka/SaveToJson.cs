using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using UnityEngine;
using NCMB;

public class SaveToJson : MonoBehaviour
{
    private int score;
    private string json;
    private NCMB.SaveData saveData = null;


    // Start is called before the first frame update
    void Start()
    {
        Initialize();

        // ログイン画面経由せず、MainGameに入ると、UserAuthが見つからないので
        UserAuth userAuth = FindObjectOfType<UserAuth>();
        if (userAuth)
        {
            // ハイスコアを取得する。保存されてなければ0点。
            string name = userAuth.currentPlayer();
            saveData = new NCMB.SaveData("", name);
            saveData.fetch();
        }
    }

    private void Initialize()
    {
        // スコアを0に戻す
        json = "";
       
    }

    // Update is called once per frame
    void Update()

    {
       if(Input.GetKey(KeyCode.Space))
        {
            Save();
        }

        if (Input.GetKey(KeyCode.Return))
        {
            saveData.fetch();
            Debug.Log("完璧");
        }
    }

    public void Save()
    {
        // ログイン画面経由せず、MainGameに入ると、highScoreがnullのままなので
        if (saveData != null)
        {
            saveData.savedata = JsonUtility.ToJson(Player.instance.GetPlayerData());
            saveData.save();


            // ゲーム開始前の状態に戻す
            Debug.Log(json);
            Initialize();
            
        }
    }

    public void Load()
    {
        // ログイン画面経由せず、MainGameに入ると、highScoreがnullのままなので
        if (saveData != null)
        {
            saveData.savedata = JsonUtility.ToJson(Player.instance.GetPlayerData());
            saveData.save();


            // ゲーム開始前の状態に戻す
            Debug.Log(json);
            Initialize();

        }
    }
}

