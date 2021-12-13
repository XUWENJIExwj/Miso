using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NCMB;

namespace NCMB
{
    public class SaveData
    {

        public string savedata { get; set; }
        public string name { get; private set; }

        // �R���X�g���N�^ -----------------------------------
        public SaveData(string _savedata, string _name)
        {
            savedata = _savedata;
            name = _name;
        }

        // �T�[�o�[�ɃZ�[�u�f�[�^��ۑ� -------------------------
        public void save()
        {
            // �f�[�^�X�g�A�́uSaveData�v�N���X����AName���L�[�ɂ��Č���
            NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("SaveData");
            query.WhereEqualTo("Name", name);
            query.FindAsync((List<NCMBObject> objList, NCMBException e) => {

                //��������������
                if (e == null)
                {
                    objList[0]["Data"] = savedata;
                    objList[0].SaveAsync();
                }
            });
        }

        // �T�[�o�[����n�C�X�R�A���擾  -----------------
        public void fetch()
        {
            // �f�[�^�X�g�A�́uSaveData�v�N���X����AName���L�[�ɂ��Č���
            NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("SaveData");
            query.WhereEqualTo("Name", name);
            query.FindAsync((List<NCMBObject> objList, NCMBException e) => {

                //��������������
                if (e == null)
                {
                    // �n�C�X�R�A�����o�^��������
                    if (objList.Count == 0)
                    {
                        NCMBObject obj = new NCMBObject("SaveData");
                        obj["Name"] = name;
                        obj["Data"] = "";
                        obj.SaveAsync();
                        savedata = "";
                    }
                    // �n�C�X�R�A���o�^�ς݂�������
                    else
                    {
                        savedata = System.Convert.ToString(objList[0]["Data"]);
                       
                    }
                }
            });
        }

    }
}