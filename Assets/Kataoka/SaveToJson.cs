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

        // ���O�C����ʌo�R�����AMainGame�ɓ���ƁAUserAuth��������Ȃ��̂�
        UserAuth userAuth = FindObjectOfType<UserAuth>();
        if (userAuth)
        {
            // �n�C�X�R�A���擾����B�ۑ�����ĂȂ����0�_�B
            string name = userAuth.currentPlayer();
            saveData = new NCMB.SaveData("", name);
            saveData.fetch();
        }
    }

    private void Initialize()
    {
        // �X�R�A��0�ɖ߂�
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
            Debug.Log("����");
        }
    }

    public void Save()
    {
        // ���O�C����ʌo�R�����AMainGame�ɓ���ƁAhighScore��null�̂܂܂Ȃ̂�
        if (saveData != null)
        {
            saveData.savedata = JsonUtility.ToJson(Player.instance.GetPlayerData());
            saveData.save();


            // �Q�[���J�n�O�̏�Ԃɖ߂�
            Debug.Log(json);
            Initialize();
            
        }
    }

    public void Load()
    {
        // ���O�C����ʌo�R�����AMainGame�ɓ���ƁAhighScore��null�̂܂܂Ȃ̂�
        if (saveData != null)
        {
            saveData.savedata = JsonUtility.ToJson(Player.instance.GetPlayerData());
            saveData.save();


            // �Q�[���J�n�O�̏�Ԃɖ߂�
            Debug.Log(json);
            Initialize();

        }
    }
}

