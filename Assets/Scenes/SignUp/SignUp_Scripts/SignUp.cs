using System;
using System.Text;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SignUp : MonoBehaviour
{
	private TextMeshProUGUI txtEmail;
	private TextMeshProUGUI txtNickname;
	private TextMeshProUGUI txtPassword;
	private TextMeshProUGUI txtPasswordConfirm;
	private TextMeshProUGUI txtConfirmationCode;
	private TextMeshProUGUI txtName;
	private TextMeshProUGUI txtLastame;
	private TextMeshProUGUI emailPlaceHolder;
	private Text feedbackMessage;
	private void Start()
    {
		Localization.Language = "English";
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
			feedbackMessage.text = Localization.GetMessage("SigUp", "IncompleteFields");
        }		
		Debug.Log(response);		
	}    

    public void SendCodeToEmail()
	{
		Player player = InstancePlayer();			
		string response = player.SendCode();
        if (player.IsComplete())
        {
			if (String.Equals(response, "OK"))
			{
				this.feedbackMessage.text = "Código enviado con éxito";
			}
			else if (String.Equals(response, "WRONG EMAIL"))
			{
				this.feedbackMessage.text = "No se pudo enviar el correo, verifique su email";
			}
		} 
		else
        {
			this.feedbackMessage.text = Localization.GetMessage("SignUp", "IncompleteFields");
        }		
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
			case "Error":
				feedbackMessage.text = Localization.GetMessage("SignUp", "SignUpError");
				break;
			default:
				feedbackMessage.text = "";
		}
	}

	private void EvaluateResponseSendCode(string response)
	{
		switch (response)
		{
			case "OK":
				feedbackMessage.text = Localization.GetMessage("SignUp", "SignUpSuccess");
				break;
			case "UNIDENTIFIED ERROR":
				feedbackMessage.text = Localization.GetMessage("SignUp", "WrongCode");
				break;
			case "EMAIL NOT SET":
				feedbackMessage.text = Localization.GetMessage("SignUp", "AlreadyRegistered");
				break;
			case "ERROR":
				feedbackMessage.text = Localization.GetMessage("SignUp", "SignUpError");
				break;
			case "WRONG EMAIL":
				feedbackMessage.text = Localization.GetMessage("SignUp", "");
				break;
			default:
				feedbackMessage.text = "";
				break;
		}
	}
}