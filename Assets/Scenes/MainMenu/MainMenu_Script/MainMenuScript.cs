using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainMenuScript : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI btnLogin;
	[SerializeField] private TextMeshProUGUI btnSignUp;
	[SerializeField] private TextMeshProUGUI btnExit;
	[SerializeField] private TextMeshProUGUI feedbackMessage;

	public void Start()
	{
		try
		{
			this.btnLogin.text = Localization.GetMessage("MainMenu", "Login");
			this.btnSignUp.text = Localization.GetMessage("MainMenu", "SignUp");
			this.btnExit.text = Localization.GetMessage("MainMenu", "Exit");
		}
		catch (KeyNotFoundException)
		{
			this.feedbackMessage.text = "Translate error";
		}
	}

	public void GoToSignIn()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene(1);
	}

	public void GoToSignUp()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene(2);
	}
}