using UnityEngine;
using System.Collections;
using NCMB;
using System.Collections.Generic;

public class UserAuth : MonoBehaviour
{

    private string currentPlayerName;

    // mobile backend�ɐڑ����ă��O�C�� ------------------------

    public void logIn(string id, string pw)
    {

        NCMBUser.LogInAsync(id, pw, (NCMBException e) => {
            // �ڑ�����������
            if (e == null)
            {
                currentPlayerName = id;
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