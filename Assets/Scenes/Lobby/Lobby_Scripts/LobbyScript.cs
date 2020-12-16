using TMPro;
using UnityEngine;

public class LobbyScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] players = new TextMeshProUGUI[4];
    [SerializeField] private TextMeshProUGUI btnLetsGo;
    [SerializeField] private TextMeshProUGUI btnBack;

    void Start()
    {
        
        players[0].text = Localization.GetMessage("Lobby", "PlayerOne");
        players[1].text = Localization.GetMessage("Lobby", "PlayerTwo");
        players[2].text = Localization.GetMessage("Lobby", "PlayerThree");
        players[3].text = Localization.GetMessage("Lobby", "PlayerFour");
        this.btnLetsGo.text = Localization.GetMessage("Lobby", "LetsGo");
        this.btnBack.text = Localization.GetMessage("Lobby", "Back");
        
        SetPlayerList();
    }

    void SetPlayerList()
    {
        Room room = ((Room) Memory.Load("room"));
        for (int i = 0; i < room.Players.Count; i++)
        {
            players[i].text = room.Players[i].nickName;
        }
    }
}