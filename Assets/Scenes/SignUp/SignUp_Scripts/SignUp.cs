using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SignUp : MonoBehaviour
{
    public TextMeshProUGUI txtEmail;

    public TextMeshProUGUI txtNickname;

    public TextMeshProUGUI txtPassword;

    public TextMeshProUGUI txtPasswordConfirm;

    public TextMeshProUGUI txtConfirmationCode;

    public TextMeshProUGUI txtName;
    
    public TextMeshProUGUI txtLastame;
    
    TCPSocket _socket;

    public void SignUpPlayer()
    {
        string emailText = txtEmail.text;
        string nicknameText = txtNickname.text;
        string passwordText = txtPassword.text;
        string passwordConfirmText = txtPasswordConfirm.text;
        string nameText = txtName.text;
        string lastameText = txtLastame.text;
        string codeText = txtConfirmationCode.text;
        
        Command command = new Command("sign_up");
        command.AddArgument("email",emailText);
        command.AddArgument("nickname",nicknameText);
        command.AddArgument("password",passwordText);
        command.AddArgument("name",nameText);
        command.AddArgument("lastname",lastameText);
        command.AddArgument("code",codeText);
        
        TCPSocketConfiguration.BuildDefaultConfiguration(out _socket);
        _socket.AddCommand(command);
        _socket.SendCommand();
        
        Debug.Log(emailText);
        Debug.Log(nicknameText);
        Debug.Log(passwordText);
        Debug.Log(passwordConfirmText);
        Debug.Log(nameText);
        Debug.Log(lastameText);
        Debug.Log(codeText);
    }

    public void SendCodeToEmail()
    {
        string email = txtEmail.text;
        byte[] bytes = Encoding.Default.GetBytes(email);
        email = Encoding.UTF8.GetString(bytes);
        Command command = new Command("send_code_to_email");
        command.AddArgument("email",email);
        TCPSocketConfiguration.BuildDefaultConfiguration(out _socket);
        _socket.AddCommand(command);
        _socket.SendCommand();
        
        Debug.Log(_socket.GetResponse());
    }

    public void BackToMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
