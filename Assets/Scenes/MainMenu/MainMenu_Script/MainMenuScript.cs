using TMPro;
using UnityEngine;

public class MainMenuScript : MonoBehaviour
{
	public TextMeshProUGUI btnLogin;
	public TextMeshProUGUI btnSignUp;
	public TextMeshProUGUI btnExit;

	public void Awake()
	{
		this.btnLogin.text = Localization.GetMessage("MainMenu", "Login");
		this.btnSignUp.text = Localization.GetMessage("MainMenu", "SignUp");
		this.btnExit.text = Localization.GetMessage("MainMenu", "Exit");
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