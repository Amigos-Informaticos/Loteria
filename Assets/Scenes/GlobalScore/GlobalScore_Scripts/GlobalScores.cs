using System.Collections.Generic;
using System.Runtime.Serialization;
using TMPro;
using UnityEngine;

public class GlobalScores : MonoBehaviour
{
	[SerializeField] private GameObject entryContainer;
	[SerializeField] private GameObject entryTemplate;
	[SerializeField] private TextMeshProUGUI txtHighScores;
	[SerializeField] private TextMeshProUGUI txtPlace;
	[SerializeField] private TextMeshProUGUI txtScore;
	[SerializeField] private TextMeshProUGUI txtPlayer;
	[SerializeField] private TextMeshProUGUI txtBack;

	void Start()
	{
		try
		{
			this.txtHighScores.text = Localization.GetMessage("GlobalScore", "HighScores");
			this.txtPlace.text = Localization.GetMessage("GlobalScore", "Place");
			this.txtScore.text = Localization.GetMessage("GlobalScore", "Score");
			this.txtPlayer.text = Localization.GetMessage("GlobalScore", "Player");
			this.txtBack.text = Localization.GetMessage("GlobalScore", "Back");
		}
		catch (SerializationException)
		{
			this.txtHighScores.text = "Translate error";
		}

		this.FillGlobalScore();
	}

	private void FillGlobalScore()
	{
		Dictionary<string, Dictionary<string, string>> globalScore = Player.GetGlobalScore();
		if (globalScore != null)
		{
			entryTemplate.gameObject.SetActive(false);
			float templateHeight = 40f;
			for (int i = 0; i < globalScore.Count; i++)
			{
				GameObject entryTransform =
					Instantiate(entryTemplate, entryContainer.transform, true);
				RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
				entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * i);
				entryTransform.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text =
					(i + 1).ToString();
				entryTransform.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text =
					globalScore[i.ToString()]["points"];
				entryTransform.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text =
					globalScore[i.ToString()]["name"];
				entryTransform.gameObject.SetActive(true);
			}
		}
	}

	public void BackToMenu()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene("SignedIn");
	}
}