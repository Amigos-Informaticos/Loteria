using TMPro;
using UnityEngine;

public class Login : MonoBehaviour
{
	public TextMeshProUGUI ingresoEmail;
	public TextMeshProUGUI ingresoContrasenia;


	private Command command;
	private TCPSocket tcpSocket;

	public void BackToMainMenu()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene(0);
	}

	private void Start()
	{
		this.command = new Command("login");
		this.command.AddArgument("email", "edsonmanuelcarballovera@gmail.com");
		this.command.AddArgument("password", "relojito");

		this.tcpSocket = new TCPSocket("201.105.200.72", 42069);
		this.tcpSocket.AddCommand(this.command);
	}

	public void CreateUser()
	{
		this.tcpSocket.SendCommand();
		string salida = this.tcpSocket.GetResponse();

		Debug.Log(salida);
	}

	public void ProbarInput()
	{
		string email = ingresoEmail.text;
		string password = ingresoContrasenia.text;

		Debug.Log(email + " " + password);
	}
}