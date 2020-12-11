using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SignedIn : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playButton;
    [SerializeField] private TextMeshProUGUI scoreButton;
    [SerializeField] private TextMeshProUGUI settingsButton;
    [SerializeField] private TextMeshProUGUI exitButton;

    private void Start()
    {
        this.playButton.text = Localization.GetMessage("SignedIn", "Play");
        this.scoreButton.text = Localization.GetMessage("SignedIn", "Score");
        this.settingsButton.text = Localization.GetMessage("SignedIn", "Settings");
        this.exitButton.text = Localization.GetMessage("SignedIn", "Exit");
    }

    public void GoToScores()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("GlobalScore");
    }
    
    public void GoToSettings()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Settings");
    }

    public void BackToMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}
