using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SignUp : MonoBehaviour
{
	public TextMeshProUGUI TxtEmail;
	public TextMeshProUGUI TxtNickname;
	public TextMeshProUGUI TxtPassword;
	public TextMeshProUGUI TxtPasswordConfirm;
	public TextMeshProUGUI TxtConfirmationCode;
	public TextMeshProUGUI TxtName;
	public TextMeshProUGUI TxtLastname;
	public TextMeshProUGUI PhEmail;
	public TextMeshProUGUI PhPassword;
	public TextMeshProUGUI PhRepeatPassword;
	public TextMeshProUGUI PhNickname;
	public TextMeshProUGUI PhNames;
	public TextMeshProUGUI PhLastname;
	public TextMeshProUGUI PhCode;

	public TextMeshProUGUI BtnSignUp;
	public TextMeshProUGUI BtnGoBack;
	public TextMeshProUGUI BtnSendCode;	
	public Text feedbackMessage;

	public void Start()
	{
        try
        {
			this.PhEmail.text = Localization.GetMessage("SignUp", "Email");
			this.PhPassword.text = Localization.GetMessage("SignUp", "Password");
			this.PhRepeatPassword.text = Localization.GetMessage("SignUp", "RepeatPassword");
			this.PhNickname.text = Localization.GetMessage("SignUp", "Username");
			this.PhNames.text = Localization.GetMessage("SignUp", "Name");
			this.PhLastname.text = Localization.GetMessage("SignUp", "LastName");
			this.PhCode.text = Localization.GetMessage("SignUp", "Code");
			this.BtnSignUp.text = Localization.GetMessage("SignUp", "SignUp");
			this.BtnGoBack.text = Localization.GetMessage("SignUp", "Back");
			this.BtnSendCode.text = Localization.GetMessage("SignUp", "SendCode");
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
			if (String.Equals(this.TxtPassword.text, this.TxtPasswordConfirm.text))
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
		player.Email = Regex.Replace(TxtEmail.text, @"[^\u0000-\u007F]+", string.Empty);
		player.NickName = Regex.Replace(TxtNickname.text, @"[^\u0000-\u007F]+", string.Empty);
		player.Password = Regex.Replace(TxtPassword.text, @"[^\u0000-\u007F]+", string.Empty);
		player.Names = Regex.Replace(TxtName.text, @"[^\u0000-\u007F]+", string.Empty);
		player.LastName = Regex.Replace(TxtLastname.text, @"[^\u0000-\u007F]+", string.Empty);
		player.Code = Regex.Replace(TxtConfirmationCode.text, @"[^\u0000-\u007F]+", string.Empty);
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