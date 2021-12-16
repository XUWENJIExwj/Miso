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
            Save();
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
    public void Save()
    {
        // ���O�C����ʌo�R�����AMainGame�ɓ���ƁASaveData��null�̂܂܂Ȃ̂�
        if (saveData != null)
        {
            //���݂̃v���C���[�f�[�^��ۑ�
            saveData.savedata = JsonUtility.ToJson(Player.instance.GetPlayerData());
            saveData.save();
            // �Q�[���J�n�O�̏�Ԃɖ߂�
            Initialize();

        }
    }

    public void Load()
    {
        // ���O�C����ʌo�R�����AMainGame�ɓ���ƁAhighScore��null�̂܂܂Ȃ̂�
        if (saveData != null)
        {
            saveData.fetch();
            json = saveData.savedata;
            playerSave = JsonUtility.FromJson<PlayerData>(json);
            // �Q�[���J�n�O�̏�Ԃɖ߂�
            Initialize();
        }
    }

    public PlayerData GetSaveData()
    {
        Load();
        return playerSave;
    }
}