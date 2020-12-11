using System.Collections.Generic;
using UnityEngine;
public class Room
{
	public Player Host { get; set; } = new Player();
    public List<string> Players { get; set; } = new List<string>();
    private Command command;
    private TCPSocket tcpSocket;
    public string IdRoom { get; set; }
    public Room()
    {
        TCPSocketConfiguration.BuildDefaultConfiguration(out this.tcpSocket);
    }
    public string MakeRoom()
    {
		Command makeRoom = new Command("make_room");
		makeRoom.AddArgument("creator_email",this.Host.Email);
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
}