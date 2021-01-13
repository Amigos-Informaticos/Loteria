using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CreatePartyScript : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI txtPlayers;
	[SerializeField] private TextMeshProUGUI txtGameMode;
	[SerializeField] private TextMeshProUGUI txtSpeed;
	[SerializeField] private TextMeshProUGUI btnCreate;
	[SerializeField] private TextMeshProUGUI btnBack;
	[SerializeField] private TMP_Dropdown dpGameMode;
	[SerializeField] private TMP_Dropdown dpPlayers;
	[SerializeField] private TMP_Dropdown dpSpeed;
	[SerializeField] private TextMeshProUGUI txtFeedBackMessage;
	private List<TMP_Dropdown.OptionData> _gameModeOptions;
	private readonly Room _room = new Room();
	private int _gameModeSelectedIndex;
	private int _numberPlayers = 2;
	private int _speed = 3;
	private Player _player;

	void Start()
	{
		try
		{
			this.txtPlayers.text = Localization.GetMessage("CreateParty", "Players");
			this.txtGameMode.text = Localization.GetMessage("CreateParty", "GameMode");
			this.txtSpeed.text = Localization.GetMessage("CreateParty", "Speed");
			this.btnCreate.text = Localization.GetMessage("CreateParty", "Create");
			this.btnBack.text = Localization.GetMessage("CreateParty", "Back");
		}
		catch (KeyNotFoundException)
		{
			txtFeedBackMessage.text = "Translate Error";
		}

		this._gameModeOptions = dpGameMode.GetComponent<TMP_Dropdown>().options;
		_player = (Player) Memory.Load("player");
		InstanceRoom();
		FillGameModes();
	}

	private void FillGameModes()
	{
		try
		{
			List<string> gameModes = this._room.GetGameModes();
			if (gameModes != null)
			{
				foreach (string gameMode in gameModes)
				{
					this._gameModeOptions.Add(new TMP_Dropdown.OptionData(gameMode));
				}
			}
		}
		catch (NullReferenceException nullReferenceException)
		{
			Debug.Log("FillGameModes:" + nullReferenceException);
		}
	}

	public void OnValueChangedGameMode()
	{
		this._gameModeSelectedIndex = this.dpGameMode.value;
		Debug.Log(this._gameModeSelectedIndex);
		Debug.Log(_gameModeOptions[this._gameModeSelectedIndex].text);
	}

	public void OnValueChangedPlayers()
	{
		this._numberPlayers = this.dpPlayers.value + 2;
	}

	public void OnValueChangedSpeed()
	{
		this._speed = this.dpSpeed.value + 3;
	}

	public void OnClickBackToLetsPlay()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene("LetsPlay");
	}

	public void OnClickGoToLobby()
	{
		InstanceRoom();
		this._room.MakeRoom();
		if (EvaluateResponseMakeRoom())
		{
			Memory.Save("room", this._room);
			Player player = (Player) Memory.Load("player");
			player.IsHost = true;
			player.Board.GameMode = this._room.GameMode;
			List<bool[,]> listPatterns = player.Board.GetPattern();
			if (listPatterns != null)
			{
				if (listPatterns.Count > 1)
				{
					Player.Patterns patterns = new Player.Patterns();
					patterns.Objective = listPatterns;
					Memory.Save("patterns", patterns);
				}
				else
				{
					player.Board.Pattern = listPatterns[0];
				}

				Memory.Save("player", player);
				UnityEngine.SceneManagement.SceneManager.LoadScene("Lobby");
			}
			else
			{
				txtFeedBackMessage.text = Localization.GetMessage("CreateParty", "ErrorOnCreate");
			}
		}
	}

	public void OnClickNewGameMode()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene("CreatePattern");
	}

	private void InstanceRoom()
	{
		this._room.Host = this._player;
		this._room.Speed = this._speed;
		this._room.NumberPlayers = this._numberPlayers;
		this._room.GameMode = this._gameModeOptions[this._gameModeSelectedIndex].text;
	}

	private bool EvaluateResponseMakeRoom()
	{
		bool isMaked = true;
		switch (this._room.IdRoom)
		{
			case "ROOM ALREADY EXISTS":
				isMaked = false;
				this.txtFeedBackMessage.text =
					Localization.GetMessage("CreateParty", "RoomAlreadyExist");
				break;
			case "ERROR":
			case "ERROR. TIMEOUT":
				isMaked = false;
				this.txtFeedBackMessage.text =
					Localization.GetMessage("CreateParty", "WrongConnection");
				break;
			default:
				//Without actions
				break;
		}

		return isMaked;
	}
}