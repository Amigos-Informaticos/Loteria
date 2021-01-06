using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyScript : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI txtCode;
	[SerializeField] private TextMeshProUGUI[] txtPlayers = new TextMeshProUGUI[4];
	[SerializeField] private TextMeshProUGUI txtLetsGo;
	[SerializeField] private GameObject btnLetsGo;
	[SerializeField] private TextMeshProUGUI btnBack;
	private Room _room;
	TCPSocket _tcpSocket;
	private readonly bool _keepWaiting = true;

    void Start()
    {
        TCPSocketConfiguration.BuildDefaultConfiguration(out _tcpSocket);
        _room = (Room) Memory.Load("room");
        txtCode.text = _room.IdRoom;
        this.txtLetsGo.text = Localization.GetMessage("Lobby", "LetsGo");
        this.btnBack.text = Localization.GetMessage("Lobby", "Back");
        ConfigureWindow();
        StartPlayerList();
        SetPlayerList();

        IEnumerator waitingForPlayers = WaitingForPlayers();
	    StartCoroutine(waitingForPlayers);
    }

    public void ConfigureWindow()
    {
	    Player player = (Player) Memory.Load("player");
	    if (!player.IsHost)
	    {
		    this.btnLetsGo.SetActive(false);
	    }
    }

    public void StartPlayerListTwo()
    {
	    txtPlayers[0].text = Localization.GetMessage("Lobby", "WaitingForPlayer");
	    txtPlayers[1].text = Localization.GetMessage("Lobby", "WaitingForPlayer");
	    txtPlayers[2].text = Localization.GetMessage("Lobby", "WaitingForPlayer");
	    txtPlayers[3].text = Localization.GetMessage("Lobby", "WaitingForPlayer");
    }
    
    public void StartPlayerList()
    {
	    for (int i = 0; i < _room.NumberPlayers; i++)
	    {
		    txtPlayers[i].text = Localization.GetMessage("Lobby", "WaitingForPlayer");
	    }
    }
    
    public IEnumerator WaitingForPlayers()
    {
        while (true)
        {
	        yield return new WaitForSeconds(2.0f);
	        _room.GetPlayersInRoom();
	        StartPlayerList();
	        SetPlayerList();
        }
    }

    void SetPlayerList()
	{
		for (int i = 0; i < _room.Players.Count; i++)
		{
			txtPlayers[i].text = _room.Players[i].NickName;
		}
	}

    public void OnClickBackToLetsPlay()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene("LetsPlay");
		((Room) Memory.Load("room")).ExitRoom(((Player) Memory.Load("player")).Email);
	}
}