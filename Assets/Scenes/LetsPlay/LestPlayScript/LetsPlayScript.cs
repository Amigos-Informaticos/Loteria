using TMPro;
using UnityEngine;

public class LetsPlayScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtJoinGame;
    [SerializeField] private TextMeshProUGUI btnJoinGame;
    [SerializeField] private TextMeshProUGUI btnCreateParty;
    [SerializeField] private TextMeshProUGUI btnBack;
    [SerializeField] private TextMeshProUGUI txtCode;
    [SerializeField] private TextMeshProUGUI phCode;
    [SerializeField] private TextMeshProUGUI txtFeedbackMessage;
    
    void Start()
    {
        this.txtJoinGame.text = Localization.GetMessage("LetsPlay","Join Game");
        this.btnJoinGame.text = Localization.GetMessage("LetsPlay", "Join");
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
        string code = txtCode.text;
        string response = ((Player)Memory.Load("player")).EnterToLobby(code);
        
        Debug.Log(response);

        if (response.Equals("WRONG ID"))
        {
            this.txtFeedbackMessage.text = "Room doesn't exist";
        }
        else if(response.Equals("WRONG ARGUMENTS"))
        {
            this.txtFeedbackMessage.text = "Wrong arguments";
        }
        else if (response.Equals("ERROR"))
        {
            this.txtFeedbackMessage.text = "ERROR";
        }
        else
        {
            Room room = new Room();
            room.IdRoom = code;
            room.GetPlayersInRoom();
            Memory.Save("room",room);
            UnityEngine.SceneManagement.SceneManager.LoadScene("Lobby");
        }
    }
}
