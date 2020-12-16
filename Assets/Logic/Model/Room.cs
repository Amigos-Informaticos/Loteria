using System.Collections.Generic;
using GitHub.Unity.Json;
using UnityEngine;
public class Room
{
	public Player Host { get; set; } = new Player();
    public List<string> Players { get; set; } = new List<string>();
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
            this.Players.Add(this.Host.Email);
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
            this.Players.Remove(userEmail);
        }
        tcpSocket.Close();
        return response;
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

	    Dictionary<string,string> playerList = SimpleJson.DeserializeObject<Dictionary<string, string>>(response);
	    for (int i = 0; i < 4; i++)
	    {
		    PlayerStruct player = new PlayerStruct();
	    }
	    
    }
}