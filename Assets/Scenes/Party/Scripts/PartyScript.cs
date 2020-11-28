using UnityEngine;
using UnityEngine.UI;

public class PartyScript : MonoBehaviour
{
	[SerializeField] private Image[] board = new Image[25];
	private readonly Player _player = new Player();

	public void Start()
	{
		this.generateBoard();
	}

	public Sprite CreateSprite(int idCard)
	{
		string path = MainConfiguration.GetSetting("CardsDirectoryPath");
		Texture2D texture = Resources.Load(path + idCard) as Texture2D;
		Sprite sprite = Sprite.Create(
			texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
		return sprite;
	}

	public void generateBoard()
	{
		int idBoardCard = 0;
		for (int i = 0; i < 5; i++)
		{
			for (int j = 0; j < 5; j++)
			{
				this.board[idBoardCard].GetComponent<Image>().sprite =
					this.CreateSprite(this._player.Board.Cards[i, j]);
				idBoardCard++;
			}
		}
	}
}