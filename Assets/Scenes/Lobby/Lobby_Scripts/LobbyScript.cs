using System;
using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyScript : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI txtCode;
	[SerializeField] private TextMeshProUGUI[] txtPlayers = new TextMeshProUGUI[4];
	[SerializeField] private TextMeshProUGUI txtLetsGo;
	[SerializeField] private TextMeshProUGUI feedbackMessage;
	[SerializeField] private GameObject btnLetsGo;
	[SerializeField] private GameObject[] btnKick = new GameObject[4];
	[SerializeField] private TextMeshProUGUI btnBack;
	private Room _room;
	private Player _player;
	private readonly bool _keepWaiting = true;

	void Start()
    {
	    _room = (Room) Memory.Load("room");
        _player = (Player) Memory.Load("player");
        txtCode.text = _room.IdRoom;
        txtLetsGo.text = Localization.GetMessage("Lobby", "LetsGo");
        btnBack.text = Localization.GetMessage("Lobby", "Back");
        ConfigureWindow();
        StartPlayerList();
        SetPlayerList();

        IEnumerator waitingForPlayers = WaitingForPlayers();
	    StartCoroutine(waitingForPlayers);
	    IEnumerator waitingForStart = WaitingForStart();
	    StartCoroutine(waitingForStart);
    }
	
    public void ConfigureWindow()
    {
	    if (!_player.IsHost)
	    {
		    for (int i = 0; i < 4; i++)
		    {
			    btnKick[i].SetActive(false);
			    btnLetsGo.SetActive(false);
		    }
	    }
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
	        if (_room.GetPlayersInRoom())
	        {
		        StartPlayerList();
		        SetPlayerList();    
	        }
	        else
	        {
		        OnClickBackToLetsPlay();
	        }
        }
    }

    public IEnumerator WaitingForStart()
    {
	    while (true)
	    {
		    yield return new WaitForSeconds(2.0f);
		    string response = _room.CheckPartyOn(_player.Email);
		    Debug.Log("Waiting for players: " + response);
		    if (response.Equals("ON"))
		    {
			    UnityEngine.SceneManagement.SceneManager.LoadScene("Party");
		    }
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
		_room.ExitRoom(_player.Email);
	}

    public void OnClickLetsGo()
    {
	    string response = _room.StartTheParty(_player.Email);
	    feedbackMessage.text = response;
    }
}