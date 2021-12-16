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
    //保存したプレイヤーデータを移す場所
    PlayerData playerSave = new PlayerData();


    // Start is called before the first frame update
    void Start()
    {
        Initialize();

        // ログイン画面経由せず、MainGameに入ると、UserAuthが見つからないので
        UserAuth userAuth = FindObjectOfType<UserAuth>();
        if (userAuth)
        {
            // セーブデータを取得する
            string name = userAuth.currentPlayer();
            saveData = new NCMB.SaveData("", name);
            saveData.fetch();
        }
    }

    private void Initialize()
    {
        json = "";
    }

    // Update is called once per frame
    void Update()

    {
        if (Input.GetKey(KeyCode.Space))
        {
            Save();
        }

        if (Input.GetKey(KeyCode.Return))
        {
            Load();
            Debug.Log(JsonUtility.ToJson(playerSave, true));
            Debug.Log("トータルポイント" + playerSave.totalPoint);
            Debug.Log(json);
        }
    }

    //
    public void Save()
    {
        // ログイン画面経由せず、MainGameに入ると、SaveDataがnullのままなので
        if (saveData != null)
        {
            //現在のプレイヤーデータを保存
            saveData.savedata = JsonUtility.ToJson(Player.instance.GetPlayerData());
            saveData.save();
            // ゲーム開始前の状態に戻す
            Initialize();

        }
    }

    public void Load()
    {
        // ログイン画面経由せず、MainGameに入ると、highScoreがnullのままなので
        if (saveData != null)
        {
            saveData.fetch();
            json = saveData.savedata;
            playerSave = JsonUtility.FromJson<PlayerData>(json);
            // ゲーム開始前の状態に戻す
            Initialize();
        }
    }

    public PlayerData GetSaveData()
    {
        Load();
        return playerSave;
    }
}