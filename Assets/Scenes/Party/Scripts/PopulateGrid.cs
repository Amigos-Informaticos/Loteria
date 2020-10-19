using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopulateGrid : MonoBehaviour
{
	public GameObject prefab;
	public int numberToCreate;

	public void Start() {
		this.Populate();
	}

	public void Update() {
		
	}

	void Populate(){
		GameObject newObject;
		for (int i = 0; i < numberToCreate; i++) {
			newObject = (GameObject)Instantiate(prefab, transform);
			newObject.GetComponent<Image>().color = Random.ColorHSV();
		}
	}
}
