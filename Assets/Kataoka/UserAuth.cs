using UnityEngine;
using System.Collections;
using NCMB;
using System.Collections.Generic;
using UnityEngine.UI;

public class UserAuth : MonoBehaviour
{

    private string currentPlayerName;
    public GameObject login_check = null;

    // mobile backendに接続してログイン ------------------------

    public void logIn(string id, string pw)
    {

        NCMBUser.LogInAsync(id, pw, (NCMBException e) => {
            // 接続成功したら
            if (e == null)
            {
                currentPlayerName = id;
            }
            else
            {
                Text error_text = login_check.GetComponent<Text>();
                // テキストの表示を入れ替える
                error_text.text = "ログインに失敗しました。\nIDとパスワードをご確認ください。";
            }

        });
    }

    // mobile backendに接続して新規会員登録 ------------------------

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
                // テキストの表示を入れ替える
                error_text.text = "このIDは既に存在するか、\nIDとパスワードが入力されていません。";
            }

        });
    }

    // mobile backendに接続してログアウト ------------------------

    public void logOut()
    {

        NCMBUser.LogOutAsync((NCMBException e) => {
            if (e == null)
            {
                currentPlayerName = null;
            }
        });
    }

    // 現在のプレイヤー名を返す --------------------
    public string currentPlayer()
    {
        return currentPlayerName;
    }

    //シングルトン
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