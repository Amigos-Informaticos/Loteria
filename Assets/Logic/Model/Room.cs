using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using GitHub.Unity.Json;
using UnityEngine;

public class Room
{
	public Player Host { get; set; } = new Player();
    public List<PlayerStruct> Players { get; set; } = new List<PlayerStruct>();
    public List<KeyValuePair<string, string>> Messages { get; } = new List<KeyValuePair<string, string>>();
    public string GameMode { get; set; }
    public int Speed { get; set; }
    public int IdGameMode { get; set; }
    public int NumberPlayers { get; set; }
    public string IdRoom { get; set; }

    public struct PlayerStruct : IEquatable<PlayerStruct>
    {
	    public string NickName { get; set; }
	    public string Email { get; set; }
	    public string IsReady { get; set; }
	    
	    public bool Equals(PlayerStruct other)
	    {
		    return NickName == other.NickName && Email == other.Email && IsReady == other.IsReady;
	    }

		public override bool Equals(object obj)
		{
			return obj is PlayerStruct other && Equals(other);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				var hashCode = (NickName != null ? NickName.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (Email != null ? Email.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (IsReady != null ? IsReady.GetHashCode() : 0);
				return hashCode;
			}
		}
	}

	public string MakeRoom()
	{
		string response;
		if (this.IsComplete())
		{
			TCPSocketConfiguration.BuildDefaultConfiguration(out TCPSocket tcpSocket);
			Command makeRoom = new Command("make_room");
			makeRoom.AddArgument("creator_email", this.Host.Email);
			makeRoom.AddArgument("speed", this.Speed.ToString());
			makeRoom.AddArgument("players", this.NumberPlayers.ToString());
			makeRoom.AddArgument("game_mode", this.GameMode);
			tcpSocket.AddCommand(makeRoom);
			tcpSocket.SendCommand();
			response = tcpSocket.GetResponse(true, 1000);
			if (!response.Equals("ERROR. TIMEOUT"))
			{
				this.IdRoom = response;
				PlayerStruct host = new PlayerStruct();
				host.Email = Host.Email;
				this.Players.Add(host);
			}
			tcpSocket.Close();
		} else
		{
			response = "ERROR";
			this.IdRoom = response;
		}
		return response;
	}

    public string ExitRoom(string userEmail)
    {
        TCPSocketConfiguration.BuildDefaultConfiguration(out TCPSocket tcpSocket);
        string response;
        Command exitRoom = new Command("exit_room");
        exitRoom.AddArgument("user_email", userEmail);
        exitRoom.AddArgument("room_id", this.IdRoom);
        tcpSocket.AddCommand(exitRoom);
        tcpSocket.SendCommand();
        response = tcpSocket.GetResponse(true, 1000);
        if (response.Equals("OK"))
        {
            this.Players.Remove(FindPlayerInRoom(userEmail));
        }
        tcpSocket.Close();
        return response;
    }

    public string SendMessage(string message, Player player)
    {
	    TCPSocketConfiguration.BuildDefaultConfiguration(out TCPSocket tcpSocket);
	    Command sendMessage = new Command("send_message_to_room");
	    sendMessage.AddArgument("message", message);
	    sendMessage.AddArgument("nickname",  player.NickName);
	    sendMessage.AddArgument("user_email", player.Email);
	    sendMessage.AddArgument("room_id", this.IdRoom);
	    tcpSocket.AddCommand(sendMessage);
	    tcpSocket.SendCommand();
	    string response = tcpSocket.GetResponse(true, 1000);
	    if(response.Equals("OK"))
	    {
		    this.Messages.Add(new KeyValuePair<string, string>(player.NickName, message));
	    }
	    tcpSocket.Close();
	    return response;
    }
    public string GetMessages(string email)
    {
	    TCPSocketConfiguration.BuildDefaultConfiguration(out TCPSocket tcpSocket);
	    Command getMessages = new Command("get_messages");
	    Dictionary<string, Dictionary<string, string>> obtainedMessages = null;
	    getMessages.AddArgument("user_email", email);
	    getMessages.AddArgument("room_id", this.IdRoom);
	    tcpSocket.AddCommand(getMessages);
	    tcpSocket.SendCommand();
	    string response = tcpSocket.GetResponse(true, 1000);
	    try
	    {
		    obtainedMessages = SimpleJson.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(response);
	    }
	    catch (SerializationException)
	    {
		    obtainedMessages = null;
	    }

	    if (obtainedMessages != null)
	    {
		    for (int i = 0; i < obtainedMessages.Count; i++)
		    {
			    this.Messages.Add(new KeyValuePair<string, string>(obtainedMessages[i.ToString()]["nickname"],
				    obtainedMessages[i.ToString()]["message"]));
		    }
	    }
	    
	    tcpSocket.Close();
	    return response;
    }
    public List<string> GetGameModes()
	{
		TCPSocketConfiguration.BuildDefaultConfiguration(out TCPSocket tcpSocket);
		Dictionary<string, string> gameMode;
		List<string> gameModes = new List<string>();
		Command getGameModes = new Command("get_game_modes_by_user");
		getGameModes.AddArgument("user_email", this.Host.Email);
		tcpSocket.AddCommand(getGameModes);
		tcpSocket.SendCommand();
		string response = tcpSocket.GetResponse(true, 1000);
		try
		{
			gameMode = SimpleJson.DeserializeObject<Dictionary<string, string>>(response);
		}
		catch (SerializationException)
		{
			gameMode = null;
		}

		if (gameMode != null)
		{
			for (int i = 0; i < gameMode.Count; i++)
			{
				gameModes.Add(gameMode[i.ToString()]);
			}
		}
		tcpSocket.Close();
		return gameModes;
	}

    public PlayerStruct FindPlayerInRoom(string email)
    {
	    PlayerStruct player = new PlayerStruct();
	    for (int i = 0; i < Players.Count; i++)
	    {
		    if (Players[i].Email.Equals(email))
		    {
			    player = Players[i];
		    }
	    }
	    return player;
    }

    public string GetUsersInRoom()
    {
	    TCPSocketConfiguration.BuildDefaultConfiguration(out TCPSocket tcpSocket);
	    string response = null;
	    Command getUsersInRoom = new Command("get_users_in_room");
	    getUsersInRoom.AddArgument("room_id", IdRoom);
	    tcpSocket.AddCommand(getUsersInRoom);
	    tcpSocket.SendCommand();
	    response = tcpSocket.GetResponse(true, 1000);
	    tcpSocket.Close();
	    return response;
    }

	public bool GetPlayersInRoom()
	{
		bool done = true;
		string response = GetUsersInRoom();
		if (response.Equals("ERROR")||response.Equals("ROOM NOT FOUND")||response.Equals("ERROR. TIMEOUT"))
		{
			done = false;
		}
		else 
		{
			Dictionary<string, Dictionary<string, string>> playerList =
				SimpleJson.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(response);
			Players = new List<PlayerStruct>();
			for (int i = 0; i < playerList.Count; i++)
			{
				string key = i.ToString();
				PlayerStruct player = new PlayerStruct();
				player.Email = playerList[key]["email"];
				player.NickName = playerList[key]["nickname"];
				Players.Add(player);
			}	
		}
		return done;
	}

	public void SetRoomConfigByJson(string json)
	{
		try
		{
			Dictionary<string, string> roomConfig = SimpleJson.DeserializeObject<Dictionary<string, string>>(json);
			Speed = Convert.ToInt32(roomConfig["speed"]);
			IdGameMode = Convert.ToInt32(roomConfig["game_mode_id"]);
			GameMode = roomConfig["game_mode"];
			NumberPlayers = Convert.ToInt32(roomConfig["max_players"]);
		}
		catch (SerializationException)
		{
			//TODO: Mostrar un mensaje de error de esto en LetsPlay
		}
	}

	private bool IsComplete()
    {
		return this.Host != null && this.NumberPlayers != 0 && this.GameMode != null && this.Speed != 0;
    }

	public override string ToString()
	{
		return "[" + this.IdRoom + " - " + this.NumberPlayers + " - " +
		       this.Speed + " - " + this.GameMode + "]";
	}

	public string StartTheParty(string hostEmail)
	{
		TCPSocketConfiguration.BuildDefaultConfiguration(out TCPSocket tcpSocket);
		Command startParty = new Command("start_party");
		startParty.AddArgument("user_email",hostEmail);
		startParty.AddArgument("room_id",IdRoom);
		tcpSocket.AddCommand(startParty);
		tcpSocket.SendCommand();
		string response = tcpSocket.GetResponse(true, 1000);
		tcpSocket.Close();
		return response;
	}

	public string CheckPartyOn(string userEmail)
	{
		TCPSocketConfiguration.BuildDefaultConfiguration(out TCPSocket tcpSocket);
		Command isPartyOn = new Command("is_party_on");
		isPartyOn.AddArgument("user_email",userEmail);
		isPartyOn.AddArgument("room_id",IdRoom);
		tcpSocket.AddCommand(isPartyOn);
		tcpSocket.SendCommand();
		string response = tcpSocket.GetResponse();
		return response;
	}

	public string ThereIsAWinner()
	{
		TCPSocketConfiguration.BuildDefaultConfiguration(out TCPSocket tcpSocket);
		Command thereIsAWinner = new Command("there_is_a_winner");
		thereIsAWinner.AddArgument("room_id",IdRoom);
		tcpSocket.AddCommand(thereIsAWinner);
		tcpSocket.SendCommand();
		
		string response = tcpSocket.GetResponse(true, 1000);
		return response;
	}
}