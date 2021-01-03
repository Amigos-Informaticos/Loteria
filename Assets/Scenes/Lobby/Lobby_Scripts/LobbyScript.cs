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
    private TCPSocket _tcpSocket;
    private bool _keepWaiting = true;
    private int count = 0;
    private string coroutineResponse;

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
        
        if (PrepareNotifyOnJoinRoom())
        {
            IEnumerator waitingForPlayers = WaitingForPlayersThree();
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
    public IEnumerator WaitingForPlayersThree()
    {
        while (true)
        {
            string response = _tcpSocket.GetResponse(true, 1000);
            Debug.Log(response);
            if (!response.Equals("ERROR") && !response.Equals("WRONG ARGUMENTS") &&
                !response.Equals("ERROR. TIMEOUT"))
            {
                _room.GetPlayersInRoom(response);
                SetPlayerList();
            }
            yield return new WaitForSeconds(1.0f);
        }
    }
    
    public bool PrepareNotifyOnJoinRoom()
    {
        
        Command notifyOnJoinRoom = new Command("notify_me");
        notifyOnJoinRoom.AddArgument("event","join_room_notification");
        notifyOnJoinRoom.AddArgument("user_email",((Player)Memory.Load("player")).Email);
        notifyOnJoinRoom.AddArgument("extra",((Room)Memory.Load("room")).IdRoom);
        _tcpSocket.AddCommand(notifyOnJoinRoom);
        _tcpSocket.SendCommand();
        string response1 = _tcpSocket.GetResponse(true, 5000);
        Debug.Log("response 1: " + response1);
        
        Command notifyOnExitRoom = new Command("notify_me");
        notifyOnExitRoom.AddArgument("event","exit_room_notification");
        notifyOnExitRoom.AddArgument("user_email",((Player)Memory.Load("player")).Email);
        notifyOnExitRoom.AddArgument("extra",((Room)Memory.Load("room")).IdRoom);
        _tcpSocket.AddCommand(notifyOnExitRoom);
        _tcpSocket.SendCommand();
        string response2 = _tcpSocket.GetResponse(true, 5000);
        Debug.Log("response 2: " + response2);

        return (response1.Equals("OK"));
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
    public void OnClickBackToLetsPlay()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("LetsPlay");
        ((Room)Memory.Load("room")).ExitRoom(((Player)Memory.Load("player")).Email);
    }
}