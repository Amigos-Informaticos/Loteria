using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField] private Transform languageDD;
    [SerializeField] private Text languageLabel;
    void Start()
    {
        //UserConfiguration.LoadSettings();
        int menuIndex = languageDD.GetComponent<Dropdown>().value;
        List<Dropdown.OptionData> menuOptions = languageDD.GetComponent<Dropdown>().options;
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
        UserConfiguration.MusicVolume = "100";
        UserConfiguration.FXSVolume = "100";
        UserConfiguration.Language = this.GetLanguage();
        UserConfiguration.SaveSettings();
    }
}
