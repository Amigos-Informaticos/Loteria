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
		Localization.Language = "English";
		this.emailPlaceHolder.text = Localization.GetMessage("Login", "Email");
		this.passwordPlaceHolder.text = Localization.GetMessage("Login", "Password");
		this.backButton.text = Localization.GetMessage("Login","Back");
		this.loginButton.text = Localization.GetMessage("Login", "Login");
	}

	public void CreateUser()
	{
		this.tcpSocket.SendCommand();
		string salida = this.tcpSocket.GetResponse();

		Debug.Log(salida);
	}

	public void BackToMainMenu()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene(0);
	}

	public void ProbarInput()
	{
		string email = ingresoEmail.text;
		string password = ingresoContrasenia.text;

		Debug.Log(email + " " + password);
	}
}