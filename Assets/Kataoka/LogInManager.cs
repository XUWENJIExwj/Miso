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

    

    // ���O�C����ʂ̂Ƃ�true, �V�K�o�^��ʂ̂Ƃ�false
    private bool isLogIn;

    // �{�^�����������ƑΉ�����ϐ���true�ɂȂ�
    public bool logInButton;
    public bool signUpMenuButton;
    public bool signUpButton;
    public bool backButton;

    // �e�L�X�g�{�b�N�X�œ��͂���镶������i�[
    public string id;
    public string pw;
    public string mail;

    public bool sceneLoaded = false;

    public GameObject Rank_Canv;
    public GameObject Login_Canv;

    void Start()
    {

        FindObjectOfType<UserAuth>().logOut();


        isLogIn = true;
        Bottun1.SetActive(true);
        Bottun2.SetActive(true);
        Bottun3.SetActive(false);
        Bottun1.SetActive(false);
        Rank_Canv.SetActive(false);
        Login_Canv.SetActive(true);

    }

    void Update()
    {
        IdReflect();
        PassReflect();

        // ���O�C�����
        if (isLogIn)
        {
            Bottun1.SetActive(true);
            Bottun2.SetActive(true);
            Bottun3.SetActive(false);
            Bottun4.SetActive(false);
            //// ���O�C���{�^���������ꂽ��
            //if (logInButton)
            //    FindObjectOfType<UserAuth>().logIn(id, pw);

            // �V�K�o�^��ʂɈړ�����{�^���������ꂽ��
            if (signUpMenuButton)
                isLogIn = false;
        }

        // �V�K�o�^���
        else
        {

            Bottun1.SetActive(false);
            Bottun2.SetActive(false);
            Bottun3.SetActive(true);
            Bottun4.SetActive(true);
            // �V�K�o�^�{�^���������ꂽ��
            if (signUpButton)
                FindObjectOfType<UserAuth>().signUp(id, pw);

            // �߂�{�^���������ꂽ��
            if (backButton)
                isLogIn = true;
        }

        // currentPlayer�𖈃t���[���Ď����A���O�C��������������
        // Login����������Ȃ�������if������񓮂��̂ŁA
        // ��񂵂�Scene��Load���Ȃ��悤�ɂ���
        if (FindObjectOfType<UserAuth>().currentPlayer() != null)
        {
            if (!sceneLoaded)
            {
                LogicManager.instance.GameStart();
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
        Text error_text = login_check.GetComponent<Text>();
        error_text.text = "";
        Rank_Canv.SetActive(true);
        Login_Canv.SetActive(false);
    }

}
