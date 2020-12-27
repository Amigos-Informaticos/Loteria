using System.Collections.Generic;
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
    void Start()
    {
        this.txtHighScores.text = Localization.GetMessage("GlobalScore", "HighScores");
        this.txtPlace.text = Localization.GetMessage("GlobalScore", "Place");
        this.txtScore.text = Localization.GetMessage("GlobalScore", "Score");
        this.txtPlayer.text = Localization.GetMessage("GlobalScore", "Player");
    }
    private void Awake()
    {
        Dictionary<string, Dictionary<string, string>> globalScore = Player.GetGlobalScoreWithoutServer();
        entryTemplate.gameObject.SetActive(false);
        float templateHeight = 40f;
        for (int i = 0; i < globalScore.Count; i++)
        {
            GameObject entryTransform = Instantiate(entryTemplate, entryContainer.transform, true);
            RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
            entryRectTransform.anchoredPosition = new Vector2(0,-templateHeight * i);
            entryTransform.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (i + 1).ToString();
            entryTransform.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = globalScore[i.ToString()]["name"];
            entryTransform.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = globalScore[i.ToString()]["points"];
            entryTransform.gameObject.SetActive(true);
        }
    }
}
