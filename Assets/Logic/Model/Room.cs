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

    public struct PlayerStruct
    {
	    public string nickName;
	    public string email;
	    public string isReady;

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
			host.email = Host.Email;
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
		    if (Players[i].email.Equals(email))
		    {
			    player = Players[i];
		    }
	    }
	    return player;
    }
    
    

    //TODO Tengo que averiguar cómo deserializar la lista de jugadores
    public void GetPlayersInRoom()
    {
	    string response = null;
	    Command getUsersInRoom = new Command("get_users_in_room");
	    getUsersInRoom.AddArgument("room_id",IdRoom);
	    this.tcpSocket.AddCommand(getUsersInRoom);
	    this.tcpSocket.SendCommand();
	    response = this.tcpSocket.GetResponse(true, 1000);
	    Debug.Log(response);

	    Dictionary<string, Dictionary<string, string>> playerList = SimpleJson.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(response);
	    for (int i = 0; i < playerList.Count; i++)
	    {
		    string key = i.ToString();
		    PlayerStruct player = new PlayerStruct();
		    player.email = playerList[key]["email"];
		    player.nickName = playerList[key]["nickname"];
		    player.isReady = playerList[key]["is_ready"];
		    Players.Add(player);
	    }
    }
}