using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Login : MonoBehaviour {
	[SerializeField] private InputField userNameInput = null;
	[SerializeField] private InputField emailInput = null;
	[SerializeField] private InputField passwordInput = null;

	public void CreateUser(Action<Response> response) {
		StartCoroutine(CO_CreateUser(userNameInput.text, emailInput.text, passwordInput.text,
			response));
	}

	private IEnumerator CO_CreateUser(string userName, string email, string password,
	                                  Action<Response> response) {
		WWWForm form = new WWWForm();
		form.AddField("userName", userName);
		form.AddField("email", email);
		form.AddField("password", password);

		WWW connection = new WWW("http://localhost/Game/createUser.php", form);

		yield return connection;

		response(JsonUtility.FromJson<Response>(connection.text));
	}
}

[Serializable]
public class Response {
	public bool done = false;
	public string message = "";
}