namespace PruebaMySQL {
	public class Jugador {
		public int Puntuacion { get; set; }
		public Tablero Tablero { get; set; }

		public Jugador() {
			this.Tablero = new Tablero();
		}

		public bool HaveWon() {
			bool won = true;
			for (int i = 0; i < 5; i++) {
				for (int j = 0; j < 5; j++) {
					if (this.Tablero.Figura[i, j] != this.Tablero.Marcas[i, j]) {
						won = false;
						break;
					}
				}
				if (!won) {
					break;
				}
			}
			return won;
		}
	}
}