using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI txtMusicVolume;
	[SerializeField] private TextMeshProUGUI txtSFXVolume;
	[SerializeField] private TextMeshProUGUI txtLanguage;
	[SerializeField] private TextMeshProUGUI btnSave;
	[SerializeField] private TextMeshProUGUI btnBack;

	[SerializeField] private Transform languageDD;

	void Start()
	{
		this.txtMusicVolume.text = Localization.GetMessage("Settings", "Music Volume");
		this.txtSFXVolume.text = Localization.GetMessage("Settings", "SFX Volume");
		this.txtLanguage.text = Localization.GetMessage("Settings", "Language");
		this.btnSave.text = Localization.GetMessage("Settings", "Save");
		this.btnBack.text = Localization.GetMessage("Settings", "Back");
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