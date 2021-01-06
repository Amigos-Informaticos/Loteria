using System;
using GitHub.Unity.Json;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using UnityEngine;

public class Player
{
	private string _names;
	private string _lastName;
	private string _email;
	private string _password;
	private Command _command;
	private TCPSocket _tcpSocket;

	public bool IsHost { get; set; } = false;

	public string Names
	{
		get => this._names;
		set
		{
			if (IsName(value))
			{
				this._names = value;
			} else
			{
				this._names = null;
			}
		}
	}

	public string LastName
	{
		get => this._lastName;
		set
		{
			if (IsName(value))
			{
				this._lastName = value;
			} else
			{
				this._lastName = null;
			}
		}
	}

	public string NickName { get; set; }

	public string Email
	{
		get => this._email;
		set
		{
			if (IsEmail(value))
			{
				this._email = value;
			} else
			{
				this._email = null;
			}
		}
	}

	public string Password
	{
		get => this._password;
		set { this._password = Util.GetHashString(value); }
	}

	public string Code { get; set; }
	public int Score { get; set; }
	public Board Board { get; set; } = new Board();

	public string LogIn()
	{
		string loggedIn;
		TCPSocketConfiguration.BuildDefaultConfiguration(out this._tcpSocket);
		this._command = new Command("login");
		this._command.AddArgument("email", this._email);
		this._command.AddArgument("password", this._password);
		this._tcpSocket.AddCommand(this._command);
		this._tcpSocket.SendCommand();
		loggedIn = this._tcpSocket.GetResponse(true,5000);
		this._tcpSocket.Close();

		return loggedIn;
	}

	public string SignUp()
	{
		string signedUp = "ERROR";
		TCPSocketConfiguration.BuildDefaultConfiguration(out this._tcpSocket);
		this._command = new Command("sign_up");
		this._command.AddArgument("email", this._email);
		this._command.AddArgument("nickname", this.NickName);
		this._command.AddArgument("password", this._password);
		this._command.AddArgument("name", this._names);
		this._command.AddArgument("lastname", this._lastName);
		this._command.AddArgument("code", this.Code);
		this._tcpSocket.AddCommand(this._command);
		this._tcpSocket.SendCommand();
		signedUp = this._tcpSocket.GetResponse(true, 5000);
		this._tcpSocket.Close();
		return signedUp;
	}

	public string EnterToLobby(string code)
	{
		string message;
		TCPSocketConfiguration.BuildDefaultConfiguration(out this._tcpSocket);
		this._command = new Command("enter_room");
		this._command.AddArgument("room_id", code);
		this._command.AddArgument("user_email",this._email);
		this._tcpSocket.AddCommand(this._command);
		this._tcpSocket.SendCommand();
		message = this._tcpSocket.GetResponse(true,3000);
		Debug.Log(message);
		this._tcpSocket.Close();
		return message;
	}

	public bool GetPlayerFromServer()
	{
		bool recoveredPlayer = true;
		TCPSocketConfiguration.BuildDefaultConfiguration(out this._tcpSocket);
		this._command = new Command("get_user");
		this._command.AddArgument("user_email", this.Email);
		this._tcpSocket.AddCommand(_command);
		this._tcpSocket.SendCommand();
		string response = _tcpSocket.GetResponse();
		this._tcpSocket.Close();
		if (!response.Equals("ERROR"))
		{
			Debug.Log(response);
			Dictionary<string, string> playerDictionary = SimpleJson.DeserializeObject<Dictionary<string, string>>(response);
			Email = playerDictionary["email"];
			Names = playerDictionary["name"];
			LastName = playerDictionary["lastname"];
			NickName = playerDictionary["nickname"];
			Score = Convert.ToInt32(playerDictionary["score"]);	
		}
		else if (response.Equals("WRONG ARGUMENT"))
		{
			recoveredPlayer = false;
		}
		else
		{
			recoveredPlayer = false;
		}
		return recoveredPlayer;
	}
	public static Dictionary<string, Dictionary<string, string>> GetGlobalScore()
	{
		TCPSocketConfiguration.BuildDefaultConfiguration(out TCPSocket tcpSocket);
		Command getTopTen = new Command("get_top_ten");
		Dictionary<string, Dictionary<string, string>> scoreDictionary = null;
		tcpSocket.AddCommand(getTopTen);
		tcpSocket.SendCommand();
		string response = tcpSocket.GetResponse(true, 1000);
		Debug.Log(response);
		if (!response.Equals("ERROR. TIMEOUT"))
		{
			try
			{
				Debug.Log(response);
				scoreDictionary =
					SimpleJson.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(
						response);
			}
			catch (SerializationException)
			{
				Debug.Log("Invalid JSON");
				scoreDictionary = null;
			}
		}
		tcpSocket.Close();
		return scoreDictionary;
	}
	public string SendCode()
	{
		TCPSocketConfiguration.BuildDefaultConfiguration(out this._tcpSocket);
		this._command = new Command("send_code_to_email");
		_command.AddArgument("email", this._email);
		this._tcpSocket.AddCommand(_command);
		this._tcpSocket.SendCommand();
		string response = this._tcpSocket.GetResponse(true, 2000);
		this._tcpSocket.Close();
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
		return this.Email != null && this.NickName != null && this.Password != null &&
		       this.Names != null && this.LastName != null;
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