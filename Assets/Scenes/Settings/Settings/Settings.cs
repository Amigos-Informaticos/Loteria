using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI MusicVolumeText;
	[SerializeField] private TextMeshProUGUI SFXVolumeText;
	[SerializeField] private TextMeshProUGUI LanguageText;
	[SerializeField] private TextMeshProUGUI SaveButton;
	[SerializeField] private TextMeshProUGUI BackButton;

	[SerializeField] private Transform languageDD;

	void Start()
	{
		this.MusicVolumeText.text = Localization.GetMessage("Settings", "Music Volume");
		this.SFXVolumeText.text = Localization.GetMessage("Settings", "SFX Volume");
		this.LanguageText.text = Localization.GetMessage("Settings", "Language");
		this.SaveButton.text = Localization.GetMessage("Settings", "Save");
		this.BackButton.text = Localization.GetMessage("Settings", "Back");
		languageDD.GetComponent<Dropdown>().value = 1;
	}

	public void BackToMenu()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene(3);
	}

	public string GetLanguage()
	{
		int menuIndex = languageDD.GetComponent<Dropdown>().value;
		List<Dropdown.OptionData> menuOptions = languageDD.GetComponent<Dropdown>().options;
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