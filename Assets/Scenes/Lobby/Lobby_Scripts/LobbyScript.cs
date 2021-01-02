using System.Collections;
using System.Collections.Generic;
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
    private Room _room;
    TCPSocket _tcpSocket;
    private readonly bool _keepWaiting = true;

    void Start()
    {
        TCPSocketConfiguration.BuildDefaultConfiguration(out _tcpSocket);
        _room = (Room) Memory.Load("room");
        txtCode.text = _room.IdRoom;
        this.btnLetsGo.text = Localization.GetMessage("Lobby", "LetsGo");
        this.btnBack.text = Localization.GetMessage("Lobby", "Back");
        ClearChecks();
        StartPlayerList();
        SetPlayerList();
        UpdateChecks();
        
        if (PrepareNotifyOnJoinRoom().Equals("OK"))
        {
            IEnumerator waitingForPlayers = WaitingForPlayers();
            StartCoroutine(waitingForPlayers);
        }
    }

    public void StartPlayerList()
    {
        txtPlayers[0].text = Localization.GetMessage("Lobby", "PlayerOne");
        txtPlayers[1].text = Localization.GetMessage("Lobby", "PlayerTwo");
        txtPlayers[2].text = Localization.GetMessage("Lobby", "PlayerThree");
        txtPlayers[3].text = Localization.GetMessage("Lobby", "PlayerFour");
    }

    private IEnumerator WaitingForPlayers()
    {
        string response = _tcpSocket.GetResponse(true, 5000);
        while (response.Equals("ERROR") || response.Equals("WRONG ARGUMENTS") || response.Equals("ERROR. TIMEOUT"))
        {
            response = _tcpSocket.GetResponse(true, 5000);
            yield return new WaitForSeconds(0.1f);
        }
        _room.GetPlayersInRoom(response);
        StartPlayerList();
        SetPlayerList();
    }

    public string PrepareNotifyOnJoinRoom()
    {
        Command notifyOnJoinRoom = new Command("notify_on_join_room");
        notifyOnJoinRoom.AddArgument("user_email",((Player)Memory.Load("player")).Email);
        notifyOnJoinRoom.AddArgument("room_id",((Room)Memory.Load("room")).IdRoom);
        _tcpSocket.AddCommand(notifyOnJoinRoom);
        _tcpSocket.SendCommand();
        string reponse = _tcpSocket.GetResponse(true, 5000);
        Debug.Log("salida de prepare" + reponse);
        return reponse;
    }

    void SetPlayerList()
    {
        for (int i = 0; i < _room.Players.Count; i++)
        {
            txtPlayers[i].text = _room.Players[i].NickName;
        }
    }

    void UpdateChecks()
    {
        for (int i = 0; i < _room.Players.Count; i++)
        {
            if (_room.Players[i].IsReady.Equals("T"))
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

    void ClearChecks()
    {
        for (int i = 0; i < imgChecks.Length; i++)
        {
            imgChecks[i].enabled = false;
        }
    }
}