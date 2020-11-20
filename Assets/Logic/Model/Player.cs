using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using GitHub.Unity.Json;
using UnityEngine;
using UnityEngine.UI;

public class Player
{	
	private string names;
	private string lastName;
	private string email;
	private string password;
	private Command command;
	private TCPSocket tcpSocket;

	public string Names
	{
		get => this.names;
		set
		{
			if (IsName(value))
			{
				this.names = value;
			} else
			{
				this.names = null;
			}
		}
	}

	public string LastName
	{
		get => this.lastName;
		set
		{
			if (IsName(value))
			{
				this.lastName = value;
			} else
			{
				this.lastName = null;
			}
		}
	}

	public string NickName { get; set; }

	public string Email
	{
		get => this.email;
		set
		{
			if (IsEmail(value))
			{
				this.email = value;
			} else
			{
				this.email = null;
			}
		}
	}

	public string Password
	{
		get => this.password;
		set { this.password = Util.GetHashString(value); }
	}

	public string Code { get; set; }
	public int Score { get; set; }
	public Board Board { get; set; } = new Board();

	public Player()
	{
		TCPSocketConfiguration.BuildDefaultConfiguration(out this.tcpSocket);
	}

	public string LogIn()
	{
		string loggedIn;
		this.command = new Command("login");
		this.command.AddArgument("email", this.email);
		this.command.AddArgument("password", this.password);
		this.tcpSocket.AddCommand(this.command);
		this.tcpSocket.SendCommand();		
		loggedIn = this.tcpSocket.GetResponse(true, 1000);
		this.tcpSocket.Close();
		return loggedIn;
	}

	public string SignUp()
	{
		string signedUp = "Error";
		this.command = new Command("sign_up");
		this.command.AddArgument("email", this.email);
		this.command.AddArgument("nickname", this.NickName);
		this.command.AddArgument("password", this.password);
		this.command.AddArgument("name", this.names);
		this.command.AddArgument("lastname", this.lastName);
		this.command.AddArgument("code", this.Code);
		this.tcpSocket.AddCommand(this.command);
		this.tcpSocket.SendCommand();
		signedUp = this.tcpSocket.GetResponse(true, 1000);
		this.tcpSocket.Close();		
		return signedUp;
	}

	public static Dictionary<int, Dictionary<string, string>> GetGlobalScore()
	{
		Command getTopTen = new Command("get_top_ten");
		TCPSocketConfiguration.BuildDefaultConfiguration(out TCPSocket tcpSocket);
		Dictionary<int, Dictionary<string, string>> scoreDictionary = null;
		tcpSocket.AddCommand(getTopTen);
		string response = tcpSocket.GetResponse(true, 2000);
		Debug.Log(response);
		if(!response.Equals("ERROR. TIMEOUT"))
        {
			scoreDictionary = SimpleJson.DeserializeObject<Dictionary<int, Dictionary<string, string>>>(response);
		}			
		tcpSocket.Close();
		return scoreDictionary;
    }

	public string SendCode()
    {		
		this.command = new Command("send_code_to_email");
		command.AddArgument("email", this.email);		
		this.tcpSocket.AddCommand(command);
		this.tcpSocket.SendCommand();
		string response = this.tcpSocket.GetResponse(true, 2000);
		this.tcpSocket.Close();
		return response;
	}

	public static bool IsName(string names)
	{
		bool isName = false;
		Regex regex = new Regex("^[A-Z]+(([',. -][a-zA-Z ])?[a-zA-Z]*)*$");
		if (regex.IsMatch(names))
		{
			isName = true;
		}

		return isName;
	}

	public static bool IsEmail(string email)
	{
		bool isEmail = false;
		Regex regex = new Regex("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$");
		if (regex.IsMatch(email))
		{
			isEmail = true;
		}
		return isEmail;
	}

	public bool IsComplete()
    {
		return this.Email != null && this.NickName != null && this.Password != null && this.Names != null && this.LastName != null;
    }

	public void MakeNewBoard()
	{
		this.Board = new Board();
	}

	public bool HaveWon()
	{
		bool won = true;
		for (int i = 0; i < 5; i++)
		{
			for (int j = 0; j < 5; j++)
			{
				if (this.Board.Pattern[i, j] == this.Board.Marks[i, j]) continue;
				won = false;
				break;
			}
			if (!won)
			{
				break;
			}
		}
		return won;
	}
}