using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignedIn : MonoBehaviour
{
    public void GoToPlay()
    {
        
    }

    public void GoToScores()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(6);
    }
    
    public void GoToSettings()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(5);
    }

    public void BackToMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
