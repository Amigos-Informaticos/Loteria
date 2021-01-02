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
	Debug.Log(player.Password);
    string response = player.LogIn();
    if (response.Equals("OK"))
		{
			if (player.GetPlayerFromServer())
			{
				Memory.Save("player",player);
				UnityEngine.SceneManagement.SceneManager.LoadScene("SignedIn");	
			}
			else
			{
				txtFeedBackMessage.text = "Player Not Found";
			}
		} 
		else
		{
			txtFeedBackMessage.text = response;
		}
        Debug.Log(response);
	}

	public void BackToMainMenu()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
	}

	private void EvaluateResponseLogIn(string response)
	{
		switch (response)
		{
			case "WRONG PASSWORD":
				this.txtFeedBackMessage.text = Localization.GetMessage("LogIn", "WrongPassword");
				break;
			case "EMAIL NOT REGISTERED":
				this.txtFeedBackMessage.text =
					Localization.GetMessage("LogIn", "EmailNotRegistered");
				break;
			case "ERROR":
				this.txtFeedBackMessage.text = Localization.GetMessage("LogIn", "Error");
				break;
			default:
				this.txtFeedBackMessage.text = "";
				break;
		}
	}
}