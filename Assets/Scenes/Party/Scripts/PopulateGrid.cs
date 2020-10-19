using UnityEngine;
using UnityEngine.UI;

public class PopulateGrid : MonoBehaviour {
	public GameObject prefab;
	public int numberToCreate;

	public void Start() {
		this.Populate(this.LoadSprites());
	}

	private Sprite[] LoadSprites() {
		int[] cards = new Board().GetNumbers();
		Sprite[] sprites = new Sprite[this.numberToCreate];
		for (int i = 0; i < sprites.Length; i++) {
			sprites[i] =
				Resources.Load<Sprite>(
					"/home/w3edd/Documentos/Lotería/src/Loteria/Assets/Images/Cards/1.jpg");
		}
		return sprites;
	}

	public void Update() { }

	public void Populate(Sprite[] sprites) {
		GameObject newObject;
		for (int i = 0; i < this.numberToCreate; i++) {
			newObject = (GameObject) Instantiate(this.prefab, this.transform);
			newObject.GetComponent<Image>().sprite = sprites[i];
		}
	}
}