using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using UnityEngine;
using NCMB;

public class SaveToJson : Monosingleton<SaveToJson>
{
    private int score;
    private string json;
    private NCMB.SaveData saveData = null;
    //�ۑ������v���C���[�f�[�^���ڂ��ꏊ
    private PlayerData playerSave = new PlayerData();

    public void Init()
    {
        Initialize();

        // ���O�C����ʌo�R�����AMainGame�ɓ���ƁAUserAuth��������Ȃ��̂�
        UserAuth userAuth = FindObjectOfType<UserAuth>();
        if (userAuth)
        {
            // �Z�[�u�f�[�^���擾����
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
            Save(Player.instance.GetPlayerData());
        }

        if (Input.GetKey(KeyCode.Return))
        {
            Load();
            Debug.Log(JsonUtility.ToJson(playerSave, true));
            Debug.Log("�g�[�^���|�C���g" + playerSave.totalPoint);
            Debug.Log(json);
        }
    }

    //
    public void Save(PlayerData SaveData)
    {
        // ���O�C����ʌo�R�����AMainGame�ɓ���ƁASaveData��null�̂܂܂Ȃ̂�
        if (saveData != null)
        {
            //���݂̃v���C���[�f�[�^��ۑ�
            saveData.savedata = JsonUtility.ToJson(SaveData);
            saveData.save();
            // �Q�[���J�n�O�̏�Ԃɖ߂�
            Initialize();

        }
    }

    public bool Load()
    {
        // ���O�C����ʌo�R�����AMainGame�ɓ���ƁAhighScore��null�̂܂܂Ȃ̂�
        if (saveData != null)
        {
            saveData.fetch();
            json = saveData.savedata;
            playerSave = JsonUtility.FromJson<PlayerData>(json);
            // �Q�[���J�n�O�̏�Ԃɖ߂�
            Initialize();
            return true;
        }
        return false;
    }

    public PlayerData GetSaveData()
    {
        if (Load())
        {
            return playerSave;
        }
        else
        {
            PlayerData playerData = new PlayerData();
            playerData.Init();
            return playerData;
        }
    }
}