using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NCMB;

namespace NCMB
{
    public class HighScore
    {
        public int score { get; set; }
        public string name { get; private set; }

        // �R���X�g���N�^ -----------------------------------
        public HighScore(int _score, string _name)
        {
            score = _score;
            name = _name;
        }

        // �T�[�o�[�Ƀn�C�X�R�A��ۑ� -------------------------
        public void save()
        {
            // �f�[�^�X�g�A�́uHighScore�v�N���X����AName���L�[�ɂ��Č���
            NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("HighScore");
            query.WhereEqualTo("Name", name);
            query.FindAsync((List<NCMBObject> objList, NCMBException e) => {

                //��������������
                if (e == null)
                {
                    objList[0]["Score"] = score;
                    objList[0].SaveAsync();
                }
            });
        }

        // �T�[�o�[����n�C�X�R�A���擾  -----------------
        public void fetch()
        {
            // �f�[�^�X�g�A�́uHighScore�v�N���X����AName���L�[�ɂ��Č���
            NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("HighScore");
            query.WhereEqualTo("Name", name);
            query.FindAsync((List<NCMBObject> objList, NCMBException e) => {

                //��������������
                if (e == null)
                {
                    // �n�C�X�R�A�����o�^��������
                    if (objList.Count == 0)
                    {
                        NCMBObject obj = new NCMBObject("HighScore");
                        obj["Name"] = name;
                        obj["Score"] = 0;
                        obj.SaveAsync();
                        score = 0;
                    }
                    // �n�C�X�R�A���o�^�ς݂�������
                    else
                    {
                        score = System.Convert.ToInt32(objList[0]["Score"]);
                    }
                }
            });
        }

    }
}