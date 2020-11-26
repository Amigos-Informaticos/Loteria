using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class PopulateGrid : MonoBehaviour
{
	public GameObject prefab;
	public int numberToCreate;

	public void Start() 
	{
		this.Populate(this.LoadSprites());		
	}

	public void Update() { }	

    private Sprite[] LoadSprites() 
	{
		const string path = "Assets/Images/Cards/1.jpg";
		Debug.Log(File.Exists(path));
		int[] cards = new Board().GetNumbers();
		Sprite[] textures = new Sprite[this.numberToCreate];
		for (int i = 0; i < textures.Length; i++) {
			textures[i] = Resources.Load<Sprite>(path);
		}
		return textures;
	}
	
	public void Populate(Sprite[] textures) 
	{
		GameObject newObject;
		for (int i = 0; i < this.numberToCreate; i++) 
		{
			newObject = (GameObject) Instantiate(this.prefab, this.transform);
			newObject.GetComponent<Image>().sprite =
				Resources.Load("Assets/Images/Cards/1.jpg") as Sprite;
		}
	}	
}