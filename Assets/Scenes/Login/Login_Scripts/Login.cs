using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Login : MonoBehaviour
{
    [SerializeField] private Image imgEmail;
    [SerializeField] private Image imgPassword;
    [SerializeField] private TextMeshProUGUI txtEmail;
    [SerializeField] private TextMeshProUGUI txtPassword;
    [SerializeField] private TextMeshProUGUI phEmail;
    [SerializeField] private TextMeshProUGUI phPassword;
    [SerializeField] private TextMeshProUGUI btnBack;
    [SerializeField] private TextMeshProUGUI btnLogin;
    [SerializeField] private TextMeshProUGUI txtFeedBackMessage;
    private Command _command;
    private TCPSocket _tcpSocket;

    private void Start()
    {
        this.phEmail.text = Localization.GetMessage("Login", "Email");
        this.phPassword.text = Localization.GetMessage("Login", "Password");
        this.btnBack.text = Localization.GetMessage("Login", "Back");
        this.btnLogin.text = Localization.GetMessage("Login", "Login");
    }

    public void LogIn()
    {
        Player player = new Player
        {
            Email = Regex.Replace(this.txtEmail.text, @"[^\u0000-\u007F]+", string.Empty),
            Password = Regex.Replace(this.txtPassword.text, @"[^\u0000-\u007F]+", string.Empty)
        };
        string response = player.LogIn();
        Debug.Log(response);
        if (EvaluateResponseLogIn(response))
        {
            if (player.GetPlayerFromServer())
            {
                Memory.Save("player", player);
                UnityEngine.SceneManagement.SceneManager.LoadScene("SignedIn");
            }
        }
    }

    public void BackToMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    private bool EvaluateResponseLogIn(string response)
    {
        bool isLoggedIn = false;
        try
        {
            switch (response)
            {
                case "WRONG PASSWORD":
                    this.txtFeedBackMessage.text = Localization.GetMessage("SignedIn", "WrongPassword");
                    this.imgPassword.GetComponent<Image>().color = Util.GetHexColor("#ffbaba");
                    break;
                case "EMAIL NOT REGISTERED":
                    this.txtFeedBackMessage.text =
                        Localization.GetMessage("LogIn", "EmailNotRegistered");
                    this.imgEmail.GetComponent<Image>().color = Util.GetHexColor("#ffbaba");
                    break;
                case "ERROR":
                case "ERROR. TIMEOUT":
                    this.txtFeedBackMessage.text = Localization.GetMessage("SignUp", "WrongConnection");
                    break;
                case "OK":
                    isLoggedIn = true;
                    break;
                default:
                    isLoggedIn = false;
                    break;
            }
        }
        catch (KeyNotFoundException)
        {
            this.txtFeedBackMessage.text = "ERROR";
        }
        return isLoggedIn;
    }
}