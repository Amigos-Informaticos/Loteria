using TMPro;
using UnityEngine;

public class Login : MonoBehaviour
{
	public TextMeshProUGUI ingresoEmail;
	public TextMeshProUGUI ingresoContrasenia;

	private Command _command;
	private TCPSocket _tcpSocket;

	public void BackToMainMenu()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene(0);
	}

	private void Start()
	{
		this._command = new Command("login");
		this._command.AddArgument("email", "edsonmanuelcarballovera@gmail.com");
		this._command.AddArgument("password", "relojito");

		this._tcpSocket = new TCPSocket("201.105.200.72", 42069);
		this._tcpSocket.AddCommand(this._command);
	}

	public void CreateUser()
	{
		this._tcpSocket.SendCommand();
		string salida = this._tcpSocket.GetResponse();

		Debug.Log(salida);
	}

	public void ProbarInput()
	{
		string email = ingresoEmail.text;
		string password = ingresoContrasenia.text;

		Debug.Log(email + " " + password);
	}
}