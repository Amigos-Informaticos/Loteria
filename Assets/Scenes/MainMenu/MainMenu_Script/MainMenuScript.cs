using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuScript : MonoBehaviour
{
    public void GoToSignIn()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    public void GoToSignUp()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(2);
    }
}
