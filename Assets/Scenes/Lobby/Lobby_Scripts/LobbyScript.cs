using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtCode;
    [SerializeField] private TextMeshProUGUI[] txtPlayers = new TextMeshProUGUI[4];
    [SerializeField] private Image[] imgChecks = new Image[4];
    [SerializeField] private TextMeshProUGUI btnLetsGo;
    [SerializeField] private TextMeshProUGUI btnBack;
    private Room room;

    void Start()
    {
        room = (Room) Memory.Load("room");
        txtCode.text = room.IdRoom;
        txtPlayers[0].text = Localization.GetMessage("Lobby", "PlayerOne");
        txtPlayers[1].text = Localization.GetMessage("Lobby", "PlayerTwo");
        txtPlayers[2].text = Localization.GetMessage("Lobby", "PlayerThree");
        txtPlayers[3].text = Localization.GetMessage("Lobby", "PlayerFour");
        this.btnLetsGo.text = Localization.GetMessage("Lobby", "LetsGo");
        this.btnBack.text = Localization.GetMessage("Lobby", "Back");
        
        SetPlayerList();
        UpdateChecks();
    }

    void SetPlayerList()
    {
        Room room = ((Room) Memory.Load("room"));
        for (int i = 0; i < room.Players.Count; i++)
        {
            txtPlayers[i].text = room.Players[i].nickName;
        }
    }

    void UpdateChecks()
    {
        for (int i = 0; i < room.Players.Count; i++)
        {
            if (room.Players[i].isReady.Equals("T"))
            {
                if (!imgChecks[i].enabled)
                {
                    imgChecks[i].enabled = true;    
                }
            }
            else
            {
                if (imgChecks[i].enabled)
                {
                    imgChecks[i].enabled = false;    
                }
            }
        }
    }
}