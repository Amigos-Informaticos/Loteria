using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Login : MonoBehaviour {

	public TextMeshProUGUI ingresoEmail;
	public TextMeshProUGUI ingresoContrasenia;
	
	public void CreateUser() {
		Command command = new Command("login");
		command.AddArgument("email", "edsonmanuelcarballovera@gmail.com");
		command.AddArgument("password", "relojito");

		TCPSocket tcp = new TCPSocket("192.168.1.99", 42069);
		//Debug.Log(ingresoEmail.text + " " + ingresoContrasenia.text);
		string salida = tcp.Chat(command);

		Debug.Log(salida);
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