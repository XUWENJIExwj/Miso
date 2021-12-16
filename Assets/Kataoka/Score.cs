using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : Monosingleton<Score>
{
    
    private int score;
    private NCMB.HighScore highScore = null;
    private bool isNewRecord;

    private string json;
    private NCMB.SaveData saveData = null;
    //保存したプレイヤーデータを移す場所
    PlayerData playerSave = new PlayerData();


    // Start is called before the first frame update
    public void Init()
    {
        Initialize();

        // ログイン画面経由せず、MainGameに入ると、UserAuthが見つからないので
        UserAuth userAuth = FindObjectOfType<UserAuth>();
        if (userAuth)
        {
            // ハイスコアを取得する。保存されてなければ0点。
            string name = userAuth.currentPlayer();
            highScore = new NCMB.HighScore(0, name);
            highScore.fetch();
            saveData = new NCMB.SaveData("", name);
            saveData.fetch();
            
        }
    }

    private void Initialize()
    {
        // スコアを0に戻す
        score = 0;
        json = "";
        // フラグを初期化する
        isNewRecord = false;
    }

    // Update is called once per frame
    void Update()

    {

        if (Input.GetKey(KeyCode.Return))
        {
            Load();
            Debug.Log(JsonUtility.ToJson(playerSave, true));
            Debug.Log("トータルポイント" + playerSave.totalPoint);
            Debug.Log(json);
        }

    }

    public void Save(PlayerData Data)
    {
        // ログイン画面経由せず、MainGameに入ると、highScoreがnullのままなので
        if (highScore != null)
        {
            score = Player.instance.GetTotalPoint();
            if (highScore.score < score)
            {
                highScore.score = score;
                highScore.save();
            }
            //現在のプレイヤーデータを保存
            saveData.savedata = JsonUtility.ToJson(Data);
            saveData.save();
            // ゲーム開始前の状態に戻す
            Initialize();
            //// ハイスコアを保存する（ただし記録の更新があったときだけ）
            ////if (isNewRecord)
            ////    highScore.save();

            //// ゲーム開始前の状態に戻す
            //Initialize();
        }
    }

    public void Load()
    {
        // ログイン画面経由せず、MainGameに入ると、highScoreがnullのままなので
        if (saveData != null)
        {
            if (saveData.newUser)
            {
                CreateNewSaveData();
            }
            else
            {
                if (saveData.savedata == null || saveData.savedata == "")
                {
                    CreateNewSaveData();
                }
                else
                {
                    playerSave = JsonUtility.FromJson<PlayerData>(saveData.savedata);
                }
            } 
        }
        else
        {
            CreateNewSaveData();
        }
    }

    public PlayerData GetSaveData()
    {
        Load();
        return playerSave;
    }

    public bool ComleteFetch()
    {
        return saveData.CompleteFetch();
    }

    public void CreateNewSaveData()
    {
        playerSave.Init();
        Save(playerSave);
    }

    public bool IsNewUser()
    {
        return saveData.IsNewUser();
    }
}
