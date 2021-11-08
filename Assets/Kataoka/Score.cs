using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{

    private int score;
    private NCMB.HighScore highScore = null;
    private bool isNewRecord;


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
        }
    }

    private void Initialize()
    {
        // �X�R�A��0�ɖ߂�
        score = 0;
        // �t���O������������
        isNewRecord = false;
    }

    // Update is called once per frame
    void Update()

    {
        // Update�Ńt���[�����Ǝ擾����K�v���Ȃ��Ǝv���̂�
        // Save()���Ăяo���ꂽ���_�Ŏ��s����΂���
        // �t���O�𗧂Ă�K�v���Ȃ��Ȃ�
        //score = Player.instance.GetTotalPoint();
        //if (highScore.score < score)
        //{
        //    isNewRecord = true; // �t���O�𗧂Ă�
        //    highScore.score = score;
        //}
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
            // �n�C�X�R�A��ۑ�����i�������L�^�̍X�V���������Ƃ������j
            //if (isNewRecord)
            //    highScore.save();

            // �Q�[���J�n�O�̏�Ԃɖ߂�
            Initialize();
            Debug.Log(score);
        }
    }
}
