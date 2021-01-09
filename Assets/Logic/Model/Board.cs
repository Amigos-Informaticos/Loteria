using System;
using System.Collections.Generic;
using System.Linq;
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

	public Board()
	{
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
	public static int[] GetSortedDeck(string idRoom, string email)
    {
		TCPSocketConfiguration.BuildDefaultConfiguration(out TCPSocket tcpSocket);
		Dictionary<string, string> sortedDeck = null;
		Command getSortedDeck = new Command("get_sorted_deck");
		getSortedDeck.AddArgument("player_email", email);
		getSortedDeck.AddArgument("room_id", idRoom);
		tcpSocket.AddCommand(getSortedDeck);
		tcpSocket.SendCommand();
		string response = tcpSocket.GetResponse(true, 1000);
		tcpSocket.Close();
		Debug.Log("GetSortedDeck response:"+response);
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
		int[] converted = new int[54];
		if (sortedDeck != null)
		{
			int i = 0;
			while (i < 54)
			{
				converted[i] = Convert.ToInt32(sortedDeck[i.ToString()]);
				i++;
			}
		}
		return converted;
    }

	public string SavePattern(string userEmail)
	{
		TCPSocketConfiguration.BuildDefaultConfiguration(out TCPSocket tcpSocket);
		string response;
		Command savePattern = new Command("save_pattern");
		savePattern.AddArgument("user_email", userEmail);
		savePattern.AddArgument("game_mode_name", this.GameMode);
		savePattern.AddArgument("pattern", this.GetStringPattern());
		tcpSocket.AddCommand(savePattern);
		tcpSocket.SendCommand();
		response = tcpSocket.GetResponse(true, 1000);
		tcpSocket.Close();
		return response;
	}

	public bool[,] GetPatternByGameMode()
	{
		TCPSocketConfiguration.BuildDefaultConfiguration(out TCPSocket tcpSocket);
		Command getPattern = new Command("get_patterns");
		getPattern.AddArgument("game_mode_name", this.GameMode);
		tcpSocket.AddCommand(getPattern);
		tcpSocket.SendCommand();
		string response = tcpSocket.GetResponse(true, 1000);
		tcpSocket.Close();
		Dictionary<string, Dictionary<string, string>> patternDictionary = null;
		try
		{
			Debug.Log("GePattern response:"+response);
			patternDictionary =
				SimpleJson.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(
					response);
		}
		catch (SerializationException serializationException)
		{
			Debug.Log(serializationException);
		}
		bool[,] converted = new bool[5,5];
		int i = 0, j = 0;
		if (patternDictionary != null)
		{
			foreach (var cell3 in patternDictionary["0"]["pattern"])
			{
				converted[i, j] = cell3 == '1';
				if (j == 4)
				{
					i++;
					j = 0;
				}
				else
				{
					j++;
				}
			}
		}
		return converted;
	}
	
	public List<bool[,]> GetPatternsByGameMode()
	{
		TCPSocketConfiguration.BuildDefaultConfiguration(out TCPSocket tcpSocket);
		Command getPattern = new Command("get_patterns");
		getPattern.AddArgument("game_mode_name", this.GameMode);
		tcpSocket.AddCommand(getPattern);
		tcpSocket.SendCommand();
		string response = tcpSocket.GetResponse(true, 1000);
		tcpSocket.Close();
		Dictionary<string, Dictionary<string, string>> patternDictionary = null;
		try
		{
			Debug.Log("GePattern response:"+response);
			patternDictionary =
				SimpleJson.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(
					response);
		}
		catch (SerializationException serializationException)
		{
			Debug.Log(serializationException);
		}
		List<bool[,]> patterns = new List<bool[,]>();
		int i = 0, j = 0;
		if (patternDictionary != null)
		{
			foreach (var cell in patternDictionary)
			{
				foreach (var cell2 in cell.Value)
				{
					patterns.Add(ToArrayBi(cell2.Value));
				}
			}
		}
		return patterns;
	}

	private bool[,] ToArrayBi(string pattern)
	{
		int index = 0;
		bool[,] converted = new bool[5,5]; 
		for (int i = 0; i < 5; i++)
		{
			for (int j = 0; j < 5; j++)
			{
				converted[i, j] = pattern[index] == '1';
				index++;
			}
		}
		return converted;
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