using System;
using System.Collections.Generic;
using GitHub.Unity.Json;
using UnityEngine;
public class Room
{
	public Player Host { get; set; } = new Player();
    public List<PlayerStruct> Players { get; set; } = new List<PlayerStruct>();
    public int Rounds { get; set; }
    public string GameMode { get; set; }
    public int Speed { get; set; }
    public int NumberPlayers { get; set; }
    private Command command;
    private readonly TCPSocket tcpSocket;
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
    
    public Room()
    {
        TCPSocketConfiguration.BuildDefaultConfiguration(out this.tcpSocket);
    }
    
    public string MakeRoom()
    {
		Command makeRoom = new Command("make_room");
		makeRoom.AddArgument("creator_email",this.Host.Email);
        makeRoom.AddArgument("rounds", this.Rounds.ToString());
        makeRoom.AddArgument("speed", this.Speed.ToString());
        makeRoom.AddArgument("players", this.NumberPlayers.ToString());
        makeRoom.AddArgument("game_mode", this.GameMode);
		this.tcpSocket.AddCommand(makeRoom);
		this.tcpSocket.SendCommand();
		string response = tcpSocket.GetResponse(true, 1000);
        Debug.Log(response);
		if (!response.Equals("ERROR. TIMEOUT"))
		{
			this.IdRoom = response;
			PlayerStruct host = new PlayerStruct();
			host.Email = Host.Email;
            this.Players.Add(host);
		}
		tcpSocket.Close();
		return response;
    }

    public string ExitRoom(string userEmail)
    {
        string response = null;
        Command exitRoom = new Command("exit_room");
        exitRoom.AddArgument("user_email", userEmail);
        exitRoom.AddArgument("room_id", this.IdRoom);
        this.tcpSocket.AddCommand(exitRoom);
        this.tcpSocket.SendCommand();
        response = this.tcpSocket.GetResponse(true, 1000);
        Debug.Log(response);
        if (response.Equals("OK"))
        {
            this.Players.Remove(FindPlayerInRoom(userEmail));
        }
        tcpSocket.Close();
        return response;
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
    
    public void GetPlayersInRoom(string response = null)
    {
	    if (response == null)
	    {
		    response = GetUsersInRoom();

	    }

	    Dictionary<string, Dictionary<string, string>> playerList = SimpleJson.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(response);
	    
	    
	    Players = new List<PlayerStruct>();
	    for (int i = 0; i < playerList.Count; i++)
	    {
		    string key = i.ToString();
		    PlayerStruct player = new PlayerStruct();
		    player.Email = playerList[key]["email"];
		    player.NickName = playerList[key]["nickname"];
		    player.IsReady = playerList[key]["is_ready"];
		    Players.Add(player);
	    }
    }

    private string GetUsersInRoom()
    {
	    string response = null;
	    Command getUsersInRoom = new Command("get_users_in_room");
	    getUsersInRoom.AddArgument("room_id", IdRoom);
	    this.tcpSocket.AddCommand(getUsersInRoom);
	    this.tcpSocket.SendCommand();
	    response = this.tcpSocket.GetResponse(true, 1000);
	    return response;
    }
}