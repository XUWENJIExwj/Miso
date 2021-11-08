using UnityEngine;
using System.Collections;
using NCMB;
using System.Collections.Generic;
using UnityEngine.UI;

public class UserAuth : MonoBehaviour
{

    private string currentPlayerName;
    public GameObject login_check = null;

    // mobile backend�ɐڑ����ă��O�C�� ------------------------

    public void logIn(string id, string pw)
    {

        NCMBUser.LogInAsync(id, pw, (NCMBException e) => {
            // �ڑ�����������
            if (e == null)
            {
                currentPlayerName = id;
            }
            else
            {
                Text error_text = login_check.GetComponent<Text>();
                // �e�L�X�g�̕\�������ւ���
                error_text.text = "���O�C���Ɏ��s���܂����B\nID�ƃp�X���[�h�����m�F���������B";
            }

        });
    }

    // mobile backend�ɐڑ����ĐV�K����o�^ ------------------------

    public void signUp(string id, string pw)
    {

        NCMBUser user = new NCMBUser();
        user.UserName = id;
        //user.Email = mail;
        user.Password = pw;
        user.SignUpAsync((NCMBException e) => {

            if (e == null)
            {
                currentPlayerName = id;
            }
            
            else
            {
                Text error_text = login_check.GetComponent<Text>();
                // �e�L�X�g�̕\�������ւ���
                error_text.text = "����ID�͊��ɑ��݂��邩�A\nID�ƃp�X���[�h�����͂���Ă��܂���B";
            }

        });
    }

    // mobile backend�ɐڑ����ă��O�A�E�g ------------------------

    public void logOut()
    {

        NCMBUser.LogOutAsync((NCMBException e) => {
            if (e == null)
            {
                currentPlayerName = null;
            }
        });
    }

    // ���݂̃v���C���[����Ԃ� --------------------
    public string currentPlayer()
    {
        return currentPlayerName;
    }

    //�V���O���g��
    private UserAuth instance = null;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            string name = gameObject.name;
            gameObject.name = name + "(Singleton)";

            GameObject duplicater = GameObject.Find(name);
            if (duplicater != null)
            {
                Destroy(gameObject);
            }
            else
            {
                gameObject.name = name;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

}