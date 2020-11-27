using TMPro;
using UnityEngine;

public class GlobalScores : MonoBehaviour
{
    [SerializeField] private GameObject entryContainer;
    [SerializeField] private GameObject entryTemplate;
    private void Awake()
    {
        entryTemplate.gameObject.SetActive(false);
        
        float templateHeight = 40f;
        for (int i = 0; i < 10; i++)
        {
            GameObject entryTransform = Instantiate(entryTemplate, entryContainer.transform, true);
            RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
            entryRectTransform.anchoredPosition = new Vector2(0,-templateHeight * i);
            entryTransform.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (i + 1).ToString();
            entryTransform.gameObject.SetActive(true);
        }
    }
}
