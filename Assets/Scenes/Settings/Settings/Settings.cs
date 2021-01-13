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

	void Start()
	{
		try
		{
			this.txtLanguage.text = Localization.GetMessage("Settings", "Language");
			this.btnSave.text = Localization.GetMessage("Settings", "Save");
			this.btnBack.text = Localization.GetMessage("Settings", "Back");
		}
		catch (KeyNotFoundException keyNotFoundException)
		{
			this.txtFeedbackMessage.text = "Translate Error;";
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

	public void SaveChanges()
	{
		UserConfiguration.SetSetting("MusicVolume", "100");
		UserConfiguration.SetSetting("FSXVolume", "100");
		UserConfiguration.SetSetting("Language", this.GetLanguage());
		UserConfiguration.SaveSettings();
	}
}