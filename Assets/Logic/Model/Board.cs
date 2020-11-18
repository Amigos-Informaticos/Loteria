using System;
using System.Collections.Generic;

public class Board
{
	public int[,] Cards { get; set; } = new int[5, 5];
	public bool[,] Marks { get; set; } = new bool[5, 5];
	public bool[,] Pattern { get; set; } = new bool[5, 5];

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

	public void GenerateRandom()
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
}