using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LetsPlayScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtJoinGame;
    [SerializeField] private TextMeshProUGUI btnCreateParty;
    [SerializeField] private TextMeshProUGUI btnBack;
    [SerializeField] private TextMeshProUGUI txtCode;
    [SerializeField] private TextMeshProUGUI phCode;
    [SerializeField] private TextMeshProUGUI feedbackMessage;
    
    void Start()
    {
        this.txtJoinGame.text = Localization.GetMessage("LetsPlay","Join Game");
        this.btnCreateParty.text = Localization.GetMessage("LetsPlay","Create Party");
        this.btnBack.text = Localization.GetMessage("LetsPlay","Back");
        this.phCode.text = Localization.GetMessage("LetsPlay","Code");
    }

    public void BackToSignedIn()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("SignedIn");
    }

    public void CreateParty()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("CreateParty");
    }

    public void JoinToParty()
    {
        string response = UserConfiguration.Player.EnterToLobby(txtCode.text);;
        
        if (response.Equals("OK"))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Lobby");
        }
        else
        {
            this.feedbackMessage.text = "Room doesn't exist";
        }
    }
}
