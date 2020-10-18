using System;
public class Tablero {
	public int[,] Cartas { get; set; }
	public bool[,] Marcas { get; set; }
	public bool[,] Figura { get; set; }

	public Tablero() {
		this.Cartas = new int[5, 5];
		this.Marcas = new bool[5, 5];
		this.Figura = new bool[5, 5];
		for (var i = 0; i < 5; i++) {
			for (var j = 0; j < 5; j++) {
				this.Marcas[i, j] = false;
				this.Figura[i, j] = false;
			}
		}
		this.GenerateRandom();
	}

	public void GenerateRandom() {
		Random random = new Random();
		int nextRandom = random.Next(54);
		for (var i = 0; i < 5; i++) {
			for (var j = 0; j < 5; j++) {
				while (this.Contains(nextRandom)) {
					nextRandom = random.Next(54);
				}
				this.Cartas[i, j] = nextRandom;
			}
		}
	}

	public bool Contains(int value) {
		bool contains = false;
		int i = 0, j = 0;
		while (i < 5 && !contains) {
			while (j < 5 && !contains) {
				contains = this.Cartas[i, j] == value;
				j++;
			}
			j = 0;
			i++;
		}
		return contains;
	}

	public void Mark(int carta) {
		if (this.Contains(carta)) {
			if (this.GetPos(carta) != null) {
				int[] pos = this.GetPos(carta);
				this.Marcas[pos[0], pos[1]] = true;
			}
		}
	}

	public int[] GetPos(int carta) {
		int[] position = null;
		if (this.Contains(carta)) {
			int i = 0, j = 0;
			bool found = false;
			while (i < 5 && !found) {
				while (j < 5 && !found) {
					if (this.Cartas[i, j] == carta) {
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
}