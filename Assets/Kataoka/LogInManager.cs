using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LogInManager : MonoBehaviour
{
    public GameObject Bottun1;
    public GameObject Bottun2;

    public GameObject Bottun3;
    public GameObject Bottun4;


    public InputField inputField1;
    public InputField inputField2;

    public GameObject login_check = null;

    private GameObject guiTextLogIn;   // ログインテキスト
    private GameObject guiTextSignUp;  // 新規登録テキスト

    // ログイン画面のときtrue, 新規登録画面のときfalse
    private bool isLogIn;

    // ボタンが押されると対応する変数がtrueになる
    public bool logInButton;
    public bool signUpMenuButton;
    public bool signUpButton;
    public bool backButton;

    // テキストボックスで入力される文字列を格納
    public string id;
    public string pw;
    public string mail;

    public bool sceneLoaded = false;

    void Start()
    {

        FindObjectOfType<UserAuth>().logOut();

        // ゲームオブジェクトを検索し取得する
        guiTextLogIn = GameObject.Find("GUITextLogIn");
        guiTextSignUp = GameObject.Find("GUITextSignUp");

        isLogIn = true;
        guiTextSignUp.SetActive(false);
        guiTextLogIn.SetActive(true);
        Bottun1.SetActive(true);
        Bottun2.SetActive(true);
        Bottun3.SetActive(false);
        Bottun1.SetActive(false);


    }

    void Update()
    {
        IdReflect();
        PassReflect();

        // ログイン画面
        if (isLogIn)
        {
            Bottun1.SetActive(true);
            Bottun2.SetActive(true);
            Bottun3.SetActive(false);
            Bottun4.SetActive(false);
            //// ログインボタンが押されたら
            //if (logInButton)
            //    FindObjectOfType<UserAuth>().logIn(id, pw);

            // 新規登録画面に移動するボタンが押されたら
            if (signUpMenuButton)
                isLogIn = false;
        }

        // 新規登録画面
        else
        {

            Bottun1.SetActive(false);
            Bottun2.SetActive(false);
            Bottun3.SetActive(true);
            Bottun4.SetActive(true);
            // 新規登録ボタンが押されたら
            if (signUpButton)
                FindObjectOfType<UserAuth>().signUp(id, pw);

            // 戻るボタンが押されたら
            if (backButton)
                isLogIn = true;
        }

        // currentPlayerを毎フレーム監視し、ログインが完了したら
        // Loginを押したらなぜかこのif文が二回動くので、
        // 一回しかSceneをLoadしないようにした
        if (FindObjectOfType<UserAuth>().currentPlayer() != null)
        {
            if (!sceneLoaded)
            {
                SceneManager.LoadScene("GlobalLogic");
                sceneLoaded = true;
            }
        }
    
}


    public void IdReflect()
    {
        id = inputField1.text;
        
    }
    public void PassReflect()
    {
        pw = inputField2.text;

    }

    public void LogIn()
    {
        FindObjectOfType<UserAuth>().logIn(id, pw);
    }
    public void GO_SignUp()
    {
        isLogIn = false;
        inputField1.text = "";
        inputField2.text = "";
        Text error_text = login_check.GetComponent<Text>();
        error_text.text = "";

    }
    public void SingUp()
    {
        FindObjectOfType<UserAuth>().signUp(id, pw);
    }
    public void Back_Login()
    {
        isLogIn = true;
        inputField1.text = "";
        inputField2.text = "";
        Text error_text = login_check.GetComponent<Text>();
        error_text.text = "";
    }

    public void Go_Rank()
    {
        SceneManager.LoadScene("Rank");
    }

}
