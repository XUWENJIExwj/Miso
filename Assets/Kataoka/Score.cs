using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{

    // スコアを格納する Text コンポーネント
    public Text scoreGUIText;
    // ハイスコアを格納する Text コンポーネント 
    public Text highScoreGUIText;

    private int score;
    private NCMB.HighScore highScore;
    private bool isNewRecord;

    public NumberDisplay numberDisplay;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();

        // ハイスコアを取得する。保存されてなければ0点。
        string name = FindObjectOfType<UserAuth>().currentPlayer();
        highScore = new NCMB.HighScore(0, name);
        highScore.fetch();
    }

    private void Initialize()
    {
        // スコアを0に戻す
        score = 0;
        // フラグを初期化する
        isNewRecord = false;
    }

    // Update is called once per frame
    void Update()

    {
        score = FindObjectOfType<Count>().count;
        if (highScore.score < score)
        {
            isNewRecord = true; // フラグを立てる
            highScore.score = score;
        }

        //// スコア・ハイスコアを表示する
        //scoreGUIText.text = score.ToString();
        //highScoreGUIText.text = "HighScore : " + highScore.score.ToString();

        numberDisplay.SetValue(score);
    }

    public void Save()
    {
        // ハイスコアを保存する（ただし記録の更新があったときだけ）
        if (isNewRecord)
            highScore.save();

        // ゲーム開始前の状態に戻す
        Initialize();
    }
}
