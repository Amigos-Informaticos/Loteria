using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using TMPro;
using UnityEngine;

public class LobbyScript : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI txtCode;
	[SerializeField] private TextMeshProUGUI[] txtPlayers = new TextMeshProUGUI[4];
	[SerializeField] private TextMeshProUGUI txtLetsGo;
	[SerializeField] private TextMeshProUGUI feedbackMessage;
	[SerializeField] private GameObject btnLetsGo;
	[SerializeField] private TextMeshProUGUI btnBack;
	private Room _room;
	private Player _player;

	void Start()
	{
		_room = (Room) Memory.Load("room");
		_player = (Player) Memory.Load("player");
		txtCode.text = _room.IdRoom;
		try
		{
			txtLetsGo.text = Localization.GetMessage("Lobby", "LetsGo");
			btnBack.text = Localization.GetMessage("Lobby", "Back");
			StartPlayerList();
		}
		catch (SerializationException)
		{
			this.feedbackMessage.text = "Translate error";
		}

		ConfigureWindow();
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
				try
				{
					feedbackMessage.text = Localization.GetMessage("Lobby", "Net error");
				}
				catch (SerializationException)
				{
					feedbackMessage.text = "Translate error";
				}
			}
		}
	}

	public IEnumerator WaitingForStart()
	{
		while (true)
		{
			yield return new WaitForSeconds(2.0f);
			string response = _room.CheckPartyOn(_player.Email);
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
		if (_player.IsHost)
		{
			_player.IsHost = false;
			Memory.Save("player", _player);
		}

		UnityEngine.SceneManagement.SceneManager.LoadScene("LetsPlay");
		_room.ExitRoom(_player.Email);
		Room room = new Room();
		Memory.Save("room", room);
	}

	public void OnClickLetsGo()
	{
		string response = _room.StartTheParty(_player.Email);
		try
		{
			feedbackMessage.text = Localization.GetMessage("Lobby",response);
		}
		catch (KeyNotFoundException)
		{
			feedbackMessage.text = "Translate error";
		}
	}
}