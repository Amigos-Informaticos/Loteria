using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public class Login : MonoBehaviour
{
	public TextMeshProUGUI ingresoEmail;
	public TextMeshProUGUI ingresoContrasenia;
	public TextMeshProUGUI emailPlaceHolder;
	public TextMeshProUGUI passwordPlaceHolder;
	public TextMeshProUGUI backButton;
	public TextMeshProUGUI loginButton;
	private Command command;
	private TCPSocket tcpSocket;

	private void Start()
	{
		this.emailPlaceHolder.text = Localization.GetMessage("Login", "Email");
		this.passwordPlaceHolder.text = Localization.GetMessage("Login", "Password");
		this.backButton.text = Localization.GetMessage("Login","Back");
		this.loginButton.text = Localization.GetMessage("Login", "Login");
	}

	public void LogIn()
	{
		Player player = new Player();
		player.Email = Regex.Replace(ingresoEmail.text, @"[^\u0000-\u007F]+", string.Empty);
		player.Password = Regex.Replace(ingresoContrasenia.text, @"[^\u0000-\u007F]+", string.Empty);
		string response = player.LogIn();
        if (response.Equals("OK"))
        {
			UnityEngine.SceneManagement.SceneManager.LoadScene(3);
		}
	}

	public void BackToMainMenu()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene(0);
	}
}