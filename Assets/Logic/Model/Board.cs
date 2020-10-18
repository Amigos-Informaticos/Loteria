using System;

public class Board {
	public int[,] Cards { get; set; }
	public bool[,] Marks { get; set; }
	public bool[,] Pattern { get; set; }

	public Board() {
		this.Cards = new int[5, 5];
		this.Marks = new bool[5, 5];
		this.Pattern = new bool[5, 5];
		for (int i = 0; i < 5; i++) {
			for (int j = 0; j < 5; j++) {
				this.Marks[i, j] = false;
				this.Pattern[i, j] = false;
			}
		}
		this.GenerateRandom();
	}

	public void GenerateRandom() {
		Random random = new Random();
		int nextRandom = random.Next(54);
		for (int i = 0; i < 5; i++) {
			for (int j = 0; j < 5; j++) {
				while (this.Contains(nextRandom)) {
					nextRandom = random.Next(54);
				}
				this.Cards[i, j] = nextRandom;
			}
		}
	}

	public bool Contains(int value) {
		bool contains = false;
		int i = 0, j = 0;
		while (i < 5 && !contains) {
			while (j < 5 && !contains) {
				contains = this.Cards[i, j] == value;
				j++;
			}
			j = 0;
			i++;
		}
		return contains;
	}

	public void Mark(int carta) {
		if (!this.Contains(carta)) return;
		if (this.GetPos(carta) == null) return;
		int[] pos = this.GetPos(carta);
		this.Marks[pos[0], pos[1]] = true;
	}

	public int[] GetPos(int carta) {
		int[] position = null;
		if (!this.Contains(carta)) return null;
		int i = 0, j = 0;
		bool found = false;
		while (i < 5 && !found) {
			while (j < 5 && !found) {
				if (this.Cards[i, j] == carta) {
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
		return position;
	}
}