using GitHub.Unity.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using UnityEngine;

public class Board
{
	public int[,] Cards { get; set; } = new int[5, 5];
	public bool[,] Marks { get; set; } = new bool[5, 5];
	public bool[,] Pattern { get; set; } = new bool[5, 5];
	private Command command;
	private readonly TCPSocket tcpSocket;

	public Board()
	{
		TCPSocketConfiguration.BuildDefaultConfiguration(out this.tcpSocket);
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

	public void GenerateRandom()
	{
        System.	Random random = new System.Random();
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
		Dictionary<string, string> sortedDeck = null;
		Command getSortedDeck = new Command("get_sorted_deck");
		getSortedDeck.AddArgument("player_email", email);
		getSortedDeck.AddArgument("room_id", idRoom);
		this.tcpSocket.AddCommand(getSortedDeck);
		this.tcpSocket.SendCommand();
		string response = tcpSocket.GetResponse(true, 1000);
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
		this.tcpSocket.Close();
		return sortedDeck;
    }
	public string SavePattern()
    {
		string response = null;
		Command savePattern = new Command("save_pattern");
		savePattern.AddArgument("game_mode_name","Custom");
		savePattern.AddArgument("pattern", this.GetStringPattern());
		this.tcpSocket.AddCommand(savePattern);
		response = this.tcpSocket.GetResponse(true, 1000);
		this.tcpSocket.Close();
		return response;
    }
	public int[] GetPos(int carta)
	{
		int[] position = null;
		if (this.Contains(carta))
		{
			int i = 0, j = 0;
			bool found = false;
			while (i < 5 && !found)
			{
				while (j < 5 && !found)
				{
					if (this.Cards[i, j] == carta)
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