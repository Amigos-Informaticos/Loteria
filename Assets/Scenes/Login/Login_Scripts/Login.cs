using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public class Login : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI txtEmail;
	[SerializeField] private TextMeshProUGUI txtPassword;
	[SerializeField] private TextMeshProUGUI phEmail;
	[SerializeField] private TextMeshProUGUI phPassword;
	[SerializeField] private TextMeshProUGUI btnBack;
	[SerializeField] private TextMeshProUGUI btnLogin;
	[SerializeField] private TextMeshProUGUI txtFeedBackMessage;
	private Command command;
	private TCPSocket tcpSocket;

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
		if (response.Equals("OK"))
		{
			UnityEngine.SceneManagement.SceneManager.LoadScene(3);
		} 
		else
		{
			Debug.Log(response);
		}
	}

	public void BackToMainMenu()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene(0);
	}

	private void EvaluateResponseLogIn(string response)
	{
		switch (response)
		{			
			case "WRONG PASSWORD":
				txtFeedBackMessage.text = Localization.GetMessage("LogIn", "WrongPassword");
				break;
			case "EMAIL NOT REGISTERED":
				txtFeedBackMessage.text = Localization.GetMessage("LogIn", "EmailNotRegistered");
				break;
			case "ERROR":
				txtFeedBackMessage.text = Localization.GetMessage("LogIn", "Error");
				break;
			default:
				txtFeedBackMessage.text = "";
				break;
		}
	}
}