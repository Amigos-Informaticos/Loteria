using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SceneManager : MonoBehaviour
{
    [SerializeField] private InputField userNameInput = null;
    [SerializeField] private InputField emailInput = null;
    [SerializeField] private InputField passwordInput = null;

    private Login login = null;

    private void Awake()
    {
        login = GameObject.FindObjectOfType<Login>();
    }

    public void SubmitLogin()
    {
        
    }
    
}
