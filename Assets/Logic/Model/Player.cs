using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using GitHub.Unity.Json;
using UnityEngine;

public class Player
{
	private string _names;
	private string _lastName;
	private string _email;
	private string _password;
	private Command _command;

	public bool IsHost { get; set; }

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

	public struct Patterns
	{
		public List<bool[,]> Objective { get; set; }
	}

	public string LogIn()
	{
		string loggedIn;
		TCPSocket tcpSocket;
		TCPSocketConfiguration.BuildDefaultConfiguration(out tcpSocket);
		this._command = new Command("login");
		this._command.AddArgument("email", this._email);
		this._command.AddArgument("password", this._password);
		tcpSocket.AddCommand(this._command);
		tcpSocket.SendCommand();
		loggedIn = tcpSocket.GetResponse(true, 5000);
		tcpSocket.Close();
		return loggedIn;
	}

	public string SignUp()
	{
		TCPSocketConfiguration.BuildDefaultConfiguration(out TCPSocket tcpSocket);
		this._command = new Command("sign_up");
		this._command.AddArgument("email", this._email);
		this._command.AddArgument("nickname", this.NickName);
		this._command.AddArgument("password", this._password);
		this._command.AddArgument("name", this._names);
		this._command.AddArgument("lastname", this._lastName);
		this._command.AddArgument("code", this.Code);
		tcpSocket.AddCommand(this._command);
		tcpSocket.SendCommand();
		string signedUp = tcpSocket.GetResponse(true, 5000);
		tcpSocket.Close();
		return signedUp;
	}

	public string EnterToLobby(string code)
	{
		string message;
		TCPSocketConfiguration.BuildDefaultConfiguration(out TCPSocket tcpSocket);
		this._command = new Command("enter_room");
		this._command.AddArgument("room_id", code);
		this._command.AddArgument("user_email", this._email);
		tcpSocket.AddCommand(this._command);
		tcpSocket.SendCommand();
		message = tcpSocket.GetResponse(true, 3000);
		Debug.Log(message);
		tcpSocket.Close();
		return message;
	}

	public bool GetPlayerFromServer()
	{
		bool recoveredPlayer = false;
		TCPSocketConfiguration.BuildDefaultConfiguration(out TCPSocket tcpSocket);
		_command = new Command("get_user");
		_command.AddArgument("user_email", this.Email);
		tcpSocket.AddCommand(_command);
		tcpSocket.SendCommand();
		string response = tcpSocket.GetResponse(true, 1000);
		tcpSocket.Close();
		Debug.Log("GetPlayerFromServer() "+response);
		if (!response.Equals("ERROR. TIMEOUT"))
		{
			try
			{
				Dictionary<string, string> playerDictionary = SimpleJson.DeserializeObject<Dictionary<string, string>>(response);
				Email = playerDictionary["email"];
				Names = playerDictionary["name"];
				LastName = playerDictionary["lastname"];
				NickName = playerDictionary["nickname"];
				Score = Convert.ToInt32(playerDictionary["score"]);
				recoveredPlayer = true;
			}
			catch (SerializationException serializationException)
			{
				Debug.Log(serializationException);
				recoveredPlayer = false;
			}
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
		TCPSocketConfiguration.BuildDefaultConfiguration(out TCPSocket tcpSocket);
		this._command = new Command("send_code_to_email");
		_command.AddArgument("email", this._email);
		tcpSocket.AddCommand(_command);
		tcpSocket.SendCommand();
		string response = tcpSocket.GetResponse(true, 2000);
		tcpSocket.Close();
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
		int i = 0;
		while (i < 5 && won)
		{
			int j = 0;
			while (j < 5 && won)
			{
				if (this.Board.Pattern[i, j] && !this.Board.Marks[i, j])
				{
					won = false;
				}
				j++;
			}
			i++;
		}
		return won;
	}
	
	public bool HaveWon(Patterns patterns)
	{
		int a = 0;
		int differenceCounter = 0;
		while (a < patterns.Objective.Count)
		{
			bool isDifferent = false;
			int i = 0;
			while (i < 5 && !isDifferent)
			{
				int j = 0;
				while (j < 5 && !isDifferent)
				{
					if (!this.Board.Marks[i, j] && patterns.Objective[a][i, j])
					{
						differenceCounter++;
						isDifferent = true;
					}
					j++;
				}
				i++;
			}
			a++;
		}
		return differenceCounter < patterns.Objective.Count;
	}

	public string KickAPlayer(string playerToKick, string roomId)
	{
		TCPSocketConfiguration.BuildDefaultConfiguration(out TCPSocket tcpSocket);
		Command kickPlayer = new Command("kick_player");
		kickPlayer.AddArgument("user_email",Email);
		kickPlayer.AddArgument("room_id",roomId);
		kickPlayer.AddArgument("kicked_nickname",playerToKick);
		tcpSocket.AddCommand(kickPlayer);
		tcpSocket.SendCommand();
		string response = tcpSocket.GetResponse(true, 1000);
		tcpSocket.Close();
		Debug.Log("Kick response: " + response);
		return response;
	}

	public string IAmInRoom(string roomId)
	{
		TCPSocketConfiguration.BuildDefaultConfiguration(out TCPSocket tcpSocket);
		Command inRoom = new Command("in_room");
		inRoom.AddArgument("user_email",Email);
		inRoom.AddArgument("room_id",roomId);
		tcpSocket.AddCommand(inRoom);
		tcpSocket.SendCommand();
		string response = tcpSocket.GetResponse(true, 2000);
		tcpSocket.Close();
		Debug.Log(response);
		return response;
	}

	public bool NotifyWon(string roomId, int score)
	{
		bool won = false;
		TCPSocketConfiguration.BuildDefaultConfiguration(out TCPSocket tcpSocket);
		Command wonRound = new Command("won_round");
		wonRound.AddArgument("user_email",Email);
		wonRound.AddArgument("score", score.ToString());
		wonRound.AddArgument("room_id",roomId);
		wonRound.AddArgument("score",score.ToString());
		tcpSocket.AddCommand(wonRound);
		tcpSocket.SendCommand();
		string response = tcpSocket.GetResponse(true,1000);
		tcpSocket.Close();
		Debug.Log("Notify Won: " + response);
		if (response.Equals("OK"))
		{
			won = true;
		}
		return won;
	}

	public string SaveScore(int score)
	{
		TCPSocketConfiguration.BuildDefaultConfiguration(out TCPSocket tcpSocket);
		Command saveScore = new Command("save_score");
		saveScore.AddArgument("user_email",Email);
		saveScore.AddArgument("score",score.ToString());
		tcpSocket.AddCommand(saveScore);
		tcpSocket.SendCommand();
		string response = tcpSocket.GetResponse(true,2000);
		tcpSocket.Close();
		Debug.Log("Save score: " + response);
		return response;
	}

	public string ChangePassword(string newPassword)
	{
		TCPSocketConfiguration.BuildDefaultConfiguration(out TCPSocket tcpSocket);
		Command changePassword = new Command("change_password");
		changePassword.AddArgument("user_email",Email);
		
		string oldPassword = Password;
		changePassword.AddArgument("old_password",oldPassword);
		Password = newPassword;
		changePassword.AddArgument("new_password",Password);
		tcpSocket.AddCommand(changePassword);
		Debug.Log("old: "+ oldPassword+"new: "+Password);
		tcpSocket.SendCommand();
		string response = tcpSocket.GetResponse(true,1000);
		tcpSocket.Close();
		return response;
	}
}