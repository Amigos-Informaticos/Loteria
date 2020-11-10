using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Login : MonoBehaviour {

	public TextMeshProUGUI ingresoEmail;
	public TextMeshProUGUI ingresoContrasenia;
	
	public void CreateUser(Action<Response> response) {
		Command command = new Command("save_user");
		command.AddArgument("name", "efrain");
		command.AddArgument("last_name", "arenas");
		command.AddArgument("nickname", "efraincami");
		command.AddArgument("email", "efrain@correo.com");
		command.AddArgument("password", "beethoven");

		TCPSocket tcp = new TCPSocket("201.105.200.72", 42069);
		//string salida = tcp.Chat(command);

		//Console.WriteLine(salida);
	}

	public void ProbarInput()
	{
		string email = ingresoEmail.text;
		string password = ingresoContrasenia.text;

		Debug.Log(email + " " +  password);
	}

	
}

[Serializable]
public class Response {
	public bool done = false;
	public string message = "";
}