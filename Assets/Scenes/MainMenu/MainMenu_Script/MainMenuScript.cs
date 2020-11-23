using TMPro;
using UnityEngine;

public class MainMenuScript : MonoBehaviour
{
	public TextMeshProUGUI loginButton;
	public TextMeshProUGUI signUpButton;
	public TextMeshProUGUI exitButton;

	public void Awake()
	{
		this.loginButton.text = Localization.GetMessage("MainMenu", "Login");
		this.signUpButton.text = Localization.GetMessage("MainMenu", "SignUp");
		this.exitButton.text = Localization.GetMessage("MainMenu", "Exit");
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