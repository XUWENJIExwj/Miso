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

    private GameObject guiTextLogIn;   // ���O�C���e�L�X�g
    private GameObject guiTextSignUp;  // �V�K�o�^�e�L�X�g

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

    void Start()
    {

        FindObjectOfType<UserAuth>().logOut();

        // �Q�[���I�u�W�F�N�g���������擾����
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
                SceneManager.LoadScene("GlobalLogic");
                sceneLoaded = true;
            }
        }
    
}

    //void OnGUI()
    //{

    //    // ���O�C�����
    //    if (isLogIn)
    //    {

    //        drawLogInMenu();

    //        // ���O�C���{�^���������ꂽ��
    //        if (logInButton)
    //            FindObjectOfType<UserAuth>().logIn(id, pw);

    //        // �V�K�o�^��ʂɈړ�����{�^���������ꂽ��
    //        if (signUpMenuButton)
    //            isLogIn = false;
    //    }

    //    // �V�K�o�^���
    //    else
    //    {

    //        drawSignUpMenu();

    //        // �V�K�o�^�{�^���������ꂽ��
    //        if (signUpButton)
    //            FindObjectOfType<UserAuth>().signUp(id, pw);

    //        // �߂�{�^���������ꂽ��
    //        if (backButton)
    //            isLogIn = true;
    //    }

    //    // currentPlayer�𖈃t���[���Ď����A���O�C��������������
    //    // Login����������Ȃ�������if������񓮂��̂ŁA
    //    // ��񂵂�Scene��Load���Ȃ��悤�ɂ���
    //    if (FindObjectOfType<UserAuth>().currentPlayer() != null)
    //    {
    //        if (!sceneLoaded)
    //        {
    //            SceneManager.LoadScene("GlobalLogic");
    //            sceneLoaded = true;
    //        }
    //    }
    //}

    //private void drawLogInMenu()
    //{
    //    // �e�L�X�g�؂�ւ�
    //    guiTextSignUp.SetActive(false);
    //    guiTextLogIn.SetActive(true);

    //    // �e�L�X�g�{�b�N�X�̐ݒu�Ɠ��͒l�̎擾
    //    GUI.skin.textField.fontSize = 20;
        
    //    int txtW = 150, txtH = 40;
    //    id = GUI.TextField(new Rect(Screen.width * 1 / 2, Screen.height * 1 / 3 - txtH * 1 / 2, txtW, txtH), id);
    //    pw = GUI.PasswordField(new Rect(Screen.width * 1 / 2, Screen.height * 1 / 2 - txtH * 1 / 2, txtW, txtH), pw, '*');

    //    // �{�^���̐ݒu
    //    int btnW = 180, btnH = 50;
    //    GUI.skin.button.fontSize = 20;
    //    logInButton = GUI.Button(new Rect(Screen.width * 1 / 4 - btnW * 1 / 2, Screen.height * 3 / 4 - btnH * 1 / 2, btnW, btnH), "Log In");
    //    signUpMenuButton = GUI.Button(new Rect(Screen.width * 3 / 4 - btnW * 1 / 2, Screen.height * 3 / 4 - btnH * 1 / 2, btnW, btnH), "Sign Up");

    //}

    //private void drawSignUpMenu()
    //{
    //    // �e�L�X�g�؂�ւ�
    //    guiTextLogIn.SetActive(false);
    //    guiTextSignUp.SetActive(true);

    //    // �e�L�X�g�{�b�N�X�̐ݒu�Ɠ��͒l�̎擾
    //    int txtW = 150, txtH = 35;
    //    GUI.skin.textField.fontSize = 20;
        
    //    id = GUI.TextField(new Rect(Screen.width * 1 / 2, Screen.height * 1 / 4 - txtH * 1 / 2, txtW, txtH), id);
    //    pw = GUI.PasswordField(new Rect(Screen.width * 1 / 2, Screen.height * 2 / 5 - txtH * 1 / 2, txtW, txtH), pw, '*');
    //    //mail = GUI.TextField(new Rect(Screen.width * 1 / 2, Screen.height * 11 / 20 - txtH * 1 / 2, txtW, txtH), mail);

    //    // �{�^���̐ݒu
    //    int btnW = 180, btnH = 50;
    //    GUI.skin.button.fontSize = 20;
    //    signUpButton = GUI.Button(new Rect(Screen.width * 1 / 4 - btnW * 1 / 2, Screen.height * 3 / 4 - btnH * 1 / 2, btnW, btnH), "Sign Up");
    //    backButton = GUI.Button(new Rect(Screen.width * 3 / 4 - btnW * 1 / 2, Screen.height * 3 / 4 - btnH * 1 / 2, btnW, btnH), "Back");
    //}

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

}
