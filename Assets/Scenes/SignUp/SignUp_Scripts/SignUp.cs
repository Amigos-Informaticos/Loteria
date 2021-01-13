using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SignUp : MonoBehaviour
{
	[SerializeField] private Image imgEmail;
	[SerializeField] private Image imgPassword;
	[SerializeField] private Image imgPasswordConfirm;
	[SerializeField] private Image imgName;
	[SerializeField] private Image imgLastname;
	[SerializeField] private Image imgConfirmationCode;
	[SerializeField] private TextMeshProUGUI txtEmail;
	[SerializeField] private TextMeshProUGUI txtNickname;
	[SerializeField] private TMP_InputField txtPassword;
	[SerializeField] private TMP_InputField txtPasswordConfirm;
	[SerializeField] private TextMeshProUGUI txtConfirmationCode;
	[SerializeField] private TextMeshProUGUI txtName;
	[SerializeField] private TextMeshProUGUI txtLastname;
	[SerializeField] private TextMeshProUGUI phEmail;
	[SerializeField] private TextMeshProUGUI phPassword;
	[SerializeField] private TextMeshProUGUI phRepeatPassword;
	[SerializeField] private TextMeshProUGUI phNickname;
	[SerializeField] private TextMeshProUGUI phNames;
	[SerializeField] private TextMeshProUGUI phLastname;
	[SerializeField] private TextMeshProUGUI phCode;
	[SerializeField] private TextMeshProUGUI btnSignUp;
	[SerializeField] private TextMeshProUGUI btnGoBack;
	[SerializeField] private TextMeshProUGUI btnSendCode;
	[SerializeField] private Text feedbackMessage;

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
		catch (KeyNotFoundException)
		{
			this.feedbackMessage.text = "Translate error";
		}
	}

	public void OnValueChangedEmail()
	{
		if (Player.IsEmail(Regex.Replace(this.txtEmail.text, @"[^\u0000-\u007F]+", string.Empty)))
		{
			this.imgEmail.GetComponent<Image>().color = Util.GetHexColor("#6ceb8e");
		}
		else
		{
			this.imgEmail.GetComponent<Image>().color = Util.GetHexColor("#ffbaba");
		}
	}

	public void OnValueChangedName()
	{
		if (Player.IsName(Regex.Replace(this.txtName.text, @"[^\u0000-\u007F]+", string.Empty)))
		{
			this.imgName.GetComponent<Image>().color = Util.GetHexColor("#6ceb8e");
		}
		else
		{
			this.imgName.GetComponent<Image>().color = Util.GetHexColor("#ffbaba");
		}
	}

	public void OnValueChangedLastname()
	{
		if (Player.IsName(Regex.Replace(this.txtLastname.text, @"[^\u0000-\u007F]+", string.Empty)))
		{
			this.imgLastname.GetComponent<Image>().color = Util.GetHexColor("#6ceb8e");
		}
		else
		{
			this.imgLastname.GetComponent<Image>().color = Util.GetHexColor("#ffbaba");
		}
	}

	public void SignUpPlayer()
	{
		Player player = InstancePlayer();
		if (player.IsComplete())
		{
			if (String.Equals(this.txtPassword.text, this.txtPasswordConfirm.text))
			{
				string response = player.SignUp();
				EvaluateResponseSignUp(response);
			}
			else
			{
				try
				{
					feedbackMessage.text = Localization.GetMessage("SignUp", "UnmatchedPassword");
				}
				catch (SerializationException)
				{
					this.feedbackMessage.text = "Translate error";
				}

				this.imgPassword.GetComponent<Image>().color = Util.GetHexColor("#ffbaba");
				this.imgPasswordConfirm.GetComponent<Image>().color = Util.GetHexColor("#ffbaba");
			}
		}
		else
		{
			try
			{
				feedbackMessage.text = Localization.GetMessage("SignUp", "IncompleteFields");
			}
			catch (SerializationException)
			{
				this.feedbackMessage.text = "Translate error";
			}
		}
	}

	public void SendCodeToEmail()
	{
		string code = Regex.Replace(txtEmail.text, @"[^\u0000-\u007F]+", string.Empty);
		Player player = InstancePlayer();
		if (!code.Equals(""))
		{
			string response = player.SendCode();
			EvaluateResponseSendCode(response);	
		}
		else
		{
			this.imgConfirmationCode.GetComponent<Image>().color = Util.GetHexColor("#ffbaba");
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
		player.Password = txtPassword.text;
		player.Names = Regex.Replace(txtName.text, @"[^\u0000-\u007F]+", string.Empty);
		player.LastName = Regex.Replace(txtLastname.text, @"[^\u0000-\u007F]+", string.Empty);
		player.Code = Regex.Replace(txtConfirmationCode.text, @"[^\u0000-\u007F]+", string.Empty);
		return player;
	}

	private void EvaluateResponseSignUp(string response)
	{
		try
		{
			switch (response)
			{
				case "OK":
					feedbackMessage.text = Localization.GetMessage("SignUp", "SignUpSuccess");
					break;
				case "WRONG CODE":
					feedbackMessage.text = Localization.GetMessage("SignUp", "WrongCode");
					break;
				case "ALREADY REGISTERED":
					feedbackMessage.text = Localization.GetMessage("SignUp", "AlreadyRegistered");
					break;
				case "NICKNAME OCCUPIED":
					feedbackMessage.text = Localization.GetMessage("SignUp", "NicknameOccupied");
					break;
				case "ERROR":
					feedbackMessage.text = Localization.GetMessage("SignUp", "SignUpError");
					break;
				case "ERROR. TIMEOUT":
					feedbackMessage.text = Localization.GetMessage("SignUp", "WrongConnection");
					break;
				default:
					feedbackMessage.text = "ERROR";
					break;
			}
		}
		catch (KeyNotFoundException)
		{
			this.feedbackMessage.text = "Translate error";
		}
	}

	private void EvaluateResponseSendCode(string response)
	{
		try
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
				case "ERROR. TIMEOUT":
					feedbackMessage.text = Localization.GetMessage("SignUp", "WrongConnection");
					break;
				default:
					feedbackMessage.text = "ERROR.";
					break;
			}
		}
		catch (KeyNotFoundException)
		{
			this.feedbackMessage.text = "Translate error";
		}
	}
}