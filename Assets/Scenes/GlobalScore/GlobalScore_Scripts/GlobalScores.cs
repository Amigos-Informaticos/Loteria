using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GlobalScores : MonoBehaviour
{
    public GameObject entryContainer;
    public GameObject entryTemplate;
    
    private void Awake()
    {
        //entryContainer = transform.Find("HighScoreEntryContainer");
        //entryTemplate = entryContainer.Find("HighScoreEntryTemplate");
        
        entryTemplate.gameObject.SetActive(false);
        
        float templateHeight = 40f;
        for (int i = 0; i < 10; i++)
        {
            GameObject entryTransform = Instantiate(entryTemplate, entryContainer.transform, true);
            RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
            entryRectTransform.anchoredPosition = new Vector2(0,-templateHeight * i);
            //entryTransform.Find("Place").GetComponent<TextMeshProUGUI>().text = (i + 1).ToString();
            entryTransform.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (i + 1).ToString();
            entryTransform.gameObject.SetActive(true);
        }
    }
}
