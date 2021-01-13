using System;
using System.Collections.Generic;
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
	[SerializeField] private TextMeshProUGUI txtCurrentPassword;
	[SerializeField] private TextMeshProUGUI txtNewPassword;
	[SerializeField] private TextMeshProUGUI txtNewPasswordAgain;
	[SerializeField] private TextMeshProUGUI btnChangePassword;
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
			this.btnSave.text = Localization.GetMessage("Settings", "Save");
			this.btnBack.text = Localization.GetMessage("Settings", "Back");
		}
		catch (KeyNotFoundException)
		{
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
		string newPassword = txtNewPassword.text;
		string newPasswordAgain = txtNewPasswordAgain.text;
		string oldPassword = txtCurrentPassword.text;
		if (newPassword.Equals(newPasswordAgain))
		{
			string response = _player.ChangePassword(oldPassword,newPassword);
			txtFeedbackMessage.text = Localization.GetMessage("Settings", response);
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