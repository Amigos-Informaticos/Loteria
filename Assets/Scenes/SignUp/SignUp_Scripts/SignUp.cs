using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SignUp : MonoBehaviour
{
	public TextMeshProUGUI txtEmail;
	public TextMeshProUGUI txtNickname;
	public TextMeshProUGUI txtPassword;
	public TextMeshProUGUI txtPasswordConfirm;
	public TextMeshProUGUI txtConfirmationCode;
	public TextMeshProUGUI txtName;
	public TextMeshProUGUI txtLastname;
	public TextMeshProUGUI phEmail;
	public TextMeshProUGUI phPassword;
	public TextMeshProUGUI phRepeatPassword;
	public TextMeshProUGUI phNickname;
	public TextMeshProUGUI phNames;
	public TextMeshProUGUI phLastname;
	public TextMeshProUGUI phCode;

	public TextMeshProUGUI btnSignUp;
	public TextMeshProUGUI btnGoBack;
	public TextMeshProUGUI btnSendCode;	
	public Text feedbackMessage;

	public void Start()
	{
        try
        {
			this.phEmail.text = Localization.GetMessage("SignUp", "Email");
			this.phPassword.text = Localization.GetMessage("SignUp", "Password");
			this.phRepeatPassword.text = Localization.GetMessage("SignUp", "RepeatPassword");
			this.phNickname.text = Localization.GetMessage("SignUp", "Username");
			this.phNames.text = Localization.GetMessage("SignUp", "Name");
			this.phLastname.text = Localization.GetMessage("SignUp", "LastName");
			this.phCode.text = Localization.GetMessage("SignUp", "Code");
			this.btnSignUp.text = Localization.GetMessage("SignUp", "SignUp");
			this.btnGoBack.text = Localization.GetMessage("SignUp", "Back");
			this.btnSendCode.text = Localization.GetMessage("SignUp", "SendCode");
		}
        catch (KeyNotFoundException exception)
        {
			Debug.LogError(exception.StackTrace);
        }				
	}
	
	public void SignUpPlayer()
	{		
		Player player = InstancePlayer();
		string response = null;
		if (player.IsComplete())
        {
			if (String.Equals(this.txtPassword.text, this.txtPasswordConfirm.text))
			{
				response = player.SignUp();
				EvaluateResponseSignUp(response);
			}
			else
			{
				feedbackMessage.text = Localization.GetMessage("SignUp", "UnmatchedPassword");
			}
		} 
		else
        {
			feedbackMessage.text = Localization.GetMessage("SignUp", "IncompleteFields");
		}		
		Debug.Log(response);		
	}    

    public void SendCodeToEmail()
	{
		Player player = InstancePlayer();			
		string response = player.SendCode();
		Debug.Log(response);
	}

	public void BackToMainMenu()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene(0);
	}

	private Player InstancePlayer()
    {
		Player player = new Player();		
		player.Email = Regex.Replace(txtEmail.text, @"[^\u0000-\u007F]+", string.Empty);
		player.NickName = Regex.Replace(txtNickname.text, @"[^\u0000-\u007F]+", string.Empty);
		player.Password = Regex.Replace(txtPassword.text, @"[^\u0000-\u007F]+", string.Empty);
		player.Names = Regex.Replace(txtName.text, @"[^\u0000-\u007F]+", string.Empty);
		player.LastName = Regex.Replace(txtLastname.text, @"[^\u0000-\u007F]+", string.Empty);
		player.Code = Regex.Replace(txtConfirmationCode.text, @"[^\u0000-\u007F]+", string.Empty);
		return player;
    }

	private void EvaluateResponseSignUp(string response)
	{
		switch (response)
		{
			case "OK":
				feedbackMessage.text = Localization.GetMessage("SignUp", "SignUpSuccess");
				break;
			case "WRONG CODE":
				feedbackMessage.text = Localization.GetMessage("SignUp", "WrongCode");
				break;
			case "Already registered":
				feedbackMessage.text = Localization.GetMessage("SignUp", "AlreadyRegistered");
				break;
			case "Nickname occupied":
				feedbackMessage.text = Localization.GetMessage("SignUp","NicknameOccupied");
				break;
			case "Error":
				feedbackMessage.text = Localization.GetMessage("SignUp", "SignUpError");
				break;
			default:
				feedbackMessage.text = "";
			break;
		}
	}

	private void EvaluateResponseSendCode(string response)
	{
		switch (response)
		{
			case "OK":
				feedbackMessage.text = Localization.GetMessage("SignUp", "SendCodeSuccess");
				break;
			case "ERROR":				
			case "UNIDENTIFIED ERROR":
				feedbackMessage.text = Localization.GetMessage("SignUp", "UnidentifiedError");
				break;
			case "EMAIL NOT SET":
				feedbackMessage.text = Localization.GetMessage("SignUp", "EmailNotSet");
				break;			
			case "WRONG EMAIL":
				feedbackMessage.text = Localization.GetMessage("SignUp", "WrongEmail");
				break;
			default:
				feedbackMessage.text = "";
			break;
		}
	}
}