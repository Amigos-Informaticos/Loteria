using System.Collections.Generic;
using System.Runtime.Serialization;
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
		try
		{
			this.txtJoinGame.text = Localization.GetMessage("LetsPlay", "Join Game");
			this.btnJoinGame.text = Localization.GetMessage("LetsPlay", "Join");
			this.btnCreateParty.text = Localization.GetMessage("LetsPlay", "Create Party");
			this.btnBack.text = Localization.GetMessage("LetsPlay", "Back");
			this.phCode.text = Localization.GetMessage("LetsPlay", "Code");
		}
		catch (SerializationException)
		{
			this.txtFeedbackMessage.text = "Translate error";
		}
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
		string response = ((Player) Memory.Load("player")).EnterToLobby(code);
		if (response.Equals("WRONG ID") || response.Equals("WRONG ARGUMENTS") ||
		    response.Equals("ERROR") || response.Equals("ROOM FULL") ||
		    response.Equals("ALREADY JOINED") || response.Equals("ERROR. TIMEOUT"))
		{
			try
			{
				this.txtFeedbackMessage.text = Localization.GetMessage("LetsPlay", response);
			}
			catch (SerializationException)
			{
				this.txtFeedbackMessage.text = "Translate error";
			}
		}
		else
		{
			if (SaveRoomInMemory(code, response))
			{
				SavePlayerInMemory((Player) Memory.Load("player"));
				UnityEngine.SceneManagement.SceneManager.LoadScene("Lobby");
			}
			else
			{
				txtFeedbackMessage.text = Localization.GetMessage("LetsPlay", "ERROR");
			}
		}
	}

	private bool SaveRoomInMemory(string code, string json)
	{
		bool saved;
		Room room = new Room();
		room.IdRoom = code;
		if (room.SetRoomConfigByJson(json))
		{
			room.GetPlayersInRoom();
			Memory.Save("room", room);
			saved = true;
		}
		return saved;
	}

	private void SavePlayerInMemory(Player player)
	{
		player.Board.GameMode = ((Room) Memory.Load("room")).GameMode;
		List<bool[,]> listPatterns = player.Board.GetPattern();
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
	}
}