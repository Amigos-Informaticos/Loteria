public class Player {
	public int Score { get; set; }
	public Board Board { get; set; }

	public Player() {
		this.Score = 0;
		this.Board = new Board();
	}

	public void MakeNewBoard() {
		this.Board = new Board();
	}

	public bool HaveWon() {
		bool won = true;
		for (int i = 0; i < 5; i++) {
			for (int j = 0; j < 5; j++) {
				if (this.Board.Pattern[i, j] == this.Board.Marks[i, j]) continue;
				won = false;
				break;
			}
			if (!won) {
				break;
			}
		}
		return won;
	}
}