using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using GitHub.Unity.Json;
using UnityEngine;
using Random = System.Random;

public class Board
{
	public int[,] Cards { get; set; } = new int[5, 5];
	public bool[,] Marks { get; set; } = new bool[5, 5];
	public bool[,] Pattern { get; set; } = new bool[5, 5];
	public string GameMode { get; set; } = null;
	private Command _command;
	private readonly TCPSocket _tcpSocket;

	public Board()
	{
		TCPSocketConfiguration.BuildDefaultConfiguration(out this._tcpSocket);
		for (int i = 0; i < 5; i++)
		{
			for (int j = 0; j < 5; j++)
			{
				this.Marks[i, j] = false;
				this.Pattern[i, j] = false;
			}
		}
		this.GenerateRandom();
	}

	private void GenerateRandom()
	{
		Random random = new Random();
		int nextRandom = random.Next(54);
		for (int i = 0; i < 5; i++)
		{
			for (int j = 0; j < 5; j++)
			{
				while (this.Contains(nextRandom))
				{
					nextRandom = random.Next(54);
				}
				this.Cards[i, j] = nextRandom;
			}
		}
	}

	public bool Contains(int value)
	{
		bool contains = false;
		int i = 0, j = 0;
		while (i < 5 && !contains)
		{
			while (j < 5 && !contains)
			{
				contains = this.Cards[i, j] == value;
				j++;
			}
			j = 0;
			i++;
		}
		return contains;
	}

	public void Mark(int card)
	{
		if (this.Contains(card) && this.GetPos(card) != null)
		{
			int[] pos = this.GetPos(card);
			this.Marks[pos[0], pos[1]] = true;
		}
	}

	public Dictionary<string, string> GetSortedDeck(string idRoom, string email)
	{
		TCPSocketConfiguration.BuildDefaultConfiguration(out TCPSocket tcpSocket);
		Dictionary<string, string> sortedDeck = null;
		Command getSortedDeck = new Command("get_sorted_deck");
		getSortedDeck.AddArgument("player_email", email);
		getSortedDeck.AddArgument("room_id", idRoom);
		tcpSocket.AddCommand(getSortedDeck);
		tcpSocket.SendCommand();
		string response = tcpSocket.GetResponse(true, 1000);
		Debug.Log(response);
		if (!response.Equals("ERROR. TIMEOUT"))
		{
			try
			{
				Debug.Log(response);
				sortedDeck = SimpleJson.DeserializeObject<Dictionary<string, string>>(response);
			}
			catch (SerializationException)
			{
				Debug.Log("Invalid JSON");
				sortedDeck = null;
			}
		}
		tcpSocket.Close();
		return sortedDeck;
	}

	public string SavePattern(string userEmail)
	{
		string response = null;
		Command savePattern = new Command("save_pattern");
		savePattern.AddArgument("user_email", userEmail);
		savePattern.AddArgument("game_mode_name", this.GameMode);
		savePattern.AddArgument("pattern", this.GetStringPattern());
		this._tcpSocket.AddCommand(savePattern);
		this._tcpSocket.SendCommand();
		response = this._tcpSocket.GetResponse(true, 1000);
		this._tcpSocket.Close();
		return response;
	}

	public int[] GetPos(int card)
	{
		int[] position = null;
		if (this.Contains(card))
		{
			int i = 0, j = 0;
			bool found = false;
			while (i < 5 && !found)
			{
				while (j < 5 && !found)
				{
					if (this.Cards[i, j] == card)
					{
						position = new int[2];
						position[0] = i;
						position[1] = j;
						found = true;
					}
					j++;
				}
				j = 0;
				i++;
			}
		}
		return position;
	}

	public int[] GetNumbers()
	{
		List<int> numbers = new List<int>();
		for (int i = 0; i < 5; i++)
		{
			for (int j = 0; j < 5; j++)
			{
				numbers.Add(this.Cards[i, j]);
			}
		}
		return numbers.ToArray();
	}

	public string GetStringPattern()
	{
		StringBuilder stringPattern = new StringBuilder();
		for (int i = 0; i < 5; i++)
		{
			for (int j = 0; j < 5; j++)
			{
				stringPattern.Append(Convert.ToInt32(this.Pattern[i, j]));
			}
		}
		return stringPattern.ToString();
	}
}