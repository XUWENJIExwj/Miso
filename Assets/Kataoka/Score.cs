using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{

    // �X�R�A���i�[���� Text �R���|�[�l���g
    public Text scoreGUIText;
    // �n�C�X�R�A���i�[���� Text �R���|�[�l���g 
    public Text highScoreGUIText;

    private int score;
    private NCMB.HighScore highScore;
    private bool isNewRecord;

    public NumberDisplay numberDisplay;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();

        // �n�C�X�R�A���擾����B�ۑ�����ĂȂ����0�_�B
        string name = FindObjectOfType<UserAuth>().currentPlayer();
        highScore = new NCMB.HighScore(0, name);
        highScore.fetch();
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
        score = FindObjectOfType<Count>().count;
        if (highScore.score < score)
        {
            isNewRecord = true; // �t���O�𗧂Ă�
            highScore.score = score;
        }

        //// �X�R�A�E�n�C�X�R�A��\������
        //scoreGUIText.text = score.ToString();
        //highScoreGUIText.text = "HighScore : " + highScore.score.ToString();

        numberDisplay.SetValue(score);
    }

    public void Save()
    {
        // �n�C�X�R�A��ۑ�����i�������L�^�̍X�V���������Ƃ������j
        if (isNewRecord)
            highScore.save();

        // �Q�[���J�n�O�̏�Ԃɖ߂�
        Initialize();
    }
}
