using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SignedIn : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI btnPlay;
    [SerializeField] private TextMeshProUGUI btnScore;
    [SerializeField] private TextMeshProUGUI btnSettings;
    [SerializeField] private TextMeshProUGUI btnExit;

    private void Start()
    {
        this.btnPlay.text = Localization.GetMessage("SignedIn", "Play");
        this.btnScore.text = Localization.GetMessage("SignedIn", "Score");
        this.btnSettings.text = Localization.GetMessage("SignedIn", "Settings");
        this.btnExit.text = Localization.GetMessage("SignedIn", "Exit");
    }

    public void GoToPlay()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("LetsPlay");
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
