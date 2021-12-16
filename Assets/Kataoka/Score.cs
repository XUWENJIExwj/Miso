using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    
    private int score;
    private NCMB.HighScore highScore = null;
    private bool isNewRecord;

    private string json;
    private NCMB.SaveData saveData = null;
    //�ۑ������v���C���[�f�[�^���ڂ��ꏊ
    PlayerData playerSave = new PlayerData();


    // Start is called before the first frame update
    void Start()
    {
        Initialize();

        // ���O�C����ʌo�R�����AMainGame�ɓ���ƁAUserAuth��������Ȃ��̂�
        UserAuth userAuth = FindObjectOfType<UserAuth>();
        if (userAuth)
        {
            // �n�C�X�R�A���擾����B�ۑ�����ĂȂ����0�_�B
            string name = userAuth.currentPlayer();
            highScore = new NCMB.HighScore(0, name);
            highScore.fetch();
            saveData = new NCMB.SaveData("", name);
            saveData.fetch();
            
        }
    }

    private void Initialize()
    {
        // �X�R�A��0�ɖ߂�
        score = 0;
        json = "";
        // �t���O������������
        isNewRecord = false;
    }

    // Update is called once per frame
    void Update()

    {

        if (Input.GetKey(KeyCode.Return))
        {
            Load();
            Debug.Log(JsonUtility.ToJson(playerSave, true));
            Debug.Log("�g�[�^���|�C���g" + playerSave.totalPoint);
            Debug.Log(json);
        }

    }

    public void Save()
    {
        // ���O�C����ʌo�R�����AMainGame�ɓ���ƁAhighScore��null�̂܂܂Ȃ̂�
        if (highScore != null)
        {
            score = Player.instance.GetTotalPoint();
            if (highScore.score < score)
            {
                highScore.score = score;
                highScore.save();
            }
            //���݂̃v���C���[�f�[�^��ۑ�
            saveData.savedata = JsonUtility.ToJson(Player.instance.GetPlayerData());
            saveData.save();
            // �Q�[���J�n�O�̏�Ԃɖ߂�
            Initialize();
            // �n�C�X�R�A��ۑ�����i�������L�^�̍X�V���������Ƃ������j
            //if (isNewRecord)
            //    highScore.save();

            // �Q�[���J�n�O�̏�Ԃɖ߂�
            Initialize();
        }
    }

    public void Load()
    {
        // ���O�C����ʌo�R�����AMainGame�ɓ���ƁAhighScore��null�̂܂܂Ȃ̂�
        if (saveData != null)
        {
            if (highScore.score != 0)
            {
                saveData.fetch();
                json = saveData.savedata;
                playerSave = JsonUtility.FromJson<PlayerData>(json);
                // �Q�[���J�n�O�̏�Ԃɖ߂�
                Initialize();
            }
            else
            {
                Save();
            }
            

        }
    }

    public PlayerData GetSaveData()
    {
        Load();
        return playerSave;
    }
}
