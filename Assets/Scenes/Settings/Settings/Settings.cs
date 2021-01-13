using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI txtLanguage;
	[SerializeField] private TextMeshProUGUI btnSave;
	[SerializeField] private TextMeshProUGUI btnBack;
	[SerializeField] private Transform languageDd;
	[SerializeField] private TextMeshProUGUI txtFeedbackMessage;
	[SerializeField] private TextMeshProUGUI phCurrentPassword;
	[SerializeField] private TextMeshProUGUI phNewPassword;
	[SerializeField] private TextMeshProUGUI phNewPasswordAgain;
	[SerializeField] private TMP_InputField txtCurrentPassword;
	[SerializeField] private TMP_InputField txtNewPassword;
	[SerializeField] private TMP_InputField txtNewPasswordAgain;
	[SerializeField] private TextMeshProUGUI btnChangePassword;
	[SerializeField] private TextMeshProUGUI txtChangePassword;
	private Player _player;

	void Start()
	{
		_player = (Player) Memory.Load("player");
		try
		{
			this.phCurrentPassword.text = Localization.GetMessage("Settings","CurrentPassword");
			this.phNewPassword.text = Localization.GetMessage("Settings","NewPassword");
			this.phNewPasswordAgain.text = Localization.GetMessage("Settings","NewPasswordAgain");
			this.txtLanguage.text = Localization.GetMessage("Settings", "Language");
			this.txtChangePassword.text = Localization.GetMessage("Settings","Change password");
			this.btnChangePassword.text = Localization.GetMessage("Settings","Change");
			this.btnSave.text = Localization.GetMessage("Settings", "Save");
			this.btnBack.text = Localization.GetMessage("Settings", "Back");
		}
		catch (KeyNotFoundException keyNotFoundException)
		{
			Debug.Log(keyNotFoundException);
			this.txtFeedbackMessage.text = "Translate Error";
		}
		languageDd.GetComponent<Dropdown>().value = 1;
	}

	public void BackToMenu()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene(3);
	}

	public string GetLanguage()
	{
		int menuIndex = languageDd.GetComponent<Dropdown>().value;
		List<Dropdown.OptionData> menuOptions = languageDd.GetComponent<Dropdown>().options;
		string language = menuOptions[menuIndex].text;
		return language;
	}

	public void ChangePassword()
	{
		string newPassword = Regex.Replace(txtNewPassword.text, @"[^\u0000-\u007F]+", string.Empty);;
		string newPasswordAgain = Regex.Replace(txtNewPasswordAgain.text, @"[^\u0000-\u007F]+", string.Empty);;
		string oldPassword = Regex.Replace(txtCurrentPassword.text, @"[^\u0000-\u007F]+", string.Empty);;
		Debug.Log("Done");
		if (newPassword.Equals(newPasswordAgain))
		{
			Player playerToHash = new Player() {Password = oldPassword};
			if (playerToHash.Password.Equals(_player.Password))
			{
				string response = _player.ChangePassword(newPassword);
				Debug.Log(response);
				txtFeedbackMessage.text = Localization.GetMessage("Settings", response);	
			}
			else
			{
				txtFeedbackMessage.text = Localization.GetMessage("Settings", "WRONG OLD PASSWORD");
			}
		}
		else
		{
			txtFeedbackMessage.text = Localization.GetMessage("Settings", "PasswordDontMartch");
		}
	}

	public void SaveChanges()
	{
		UserConfiguration.SetSetting("MusicVolume", "100");
		UserConfiguration.SetSetting("FSXVolume", "100");
		UserConfiguration.SetSetting("Language", this.GetLanguage());
		UserConfiguration.SaveSettings();
	}
}