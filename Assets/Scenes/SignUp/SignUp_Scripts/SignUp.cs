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
	public TextMeshProUGUI txtLastame;
	public TextMeshProUGUI emailPlaceHolder;
	public TextMeshProUGUI passwordPlaceHolder;
	public TextMeshProUGUI repeatPasswordPlaceHolder;
	public TextMeshProUGUI nickNameFieldPlaceHolder;
	public TextMeshProUGUI namesPlaceHolder;
	public TextMeshProUGUI lastNamePlaceHolder;
	public TextMeshProUGUI codePlaceHolder;

	public TextMeshProUGUI btnSignUp;
	public TextMeshProUGUI btnGoBack;
	public TextMeshProUGUI btnSendCode;	
	public Text feedbackMessage;

	public void Start()
	{
        try
        {
			this.emailPlaceHolder.text = Localization.GetMessage("SignUp", "Email");
			this.passwordPlaceHolder.text = Localization.GetMessage("SignUp", "Password");
			this.repeatPasswordPlaceHolder.text = Localization.GetMessage("SignUp", "RepeatPassword");
			this.nickNameFieldPlaceHolder.text = Localization.GetMessage("SignUp", "Username");
			this.namesPlaceHolder.text = Localization.GetMessage("SignUp", "Name");
			this.lastNamePlaceHolder.text = Localization.GetMessage("SignUp", "LastName");
			this.codePlaceHolder.text = Localization.GetMessage("SignUp", "Code");
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
		player.LastName = Regex.Replace(txtLastame.text, @"[^\u0000-\u007F]+", string.Empty);
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
			case "Already Registered":
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