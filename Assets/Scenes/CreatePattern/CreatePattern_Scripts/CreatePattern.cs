using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreatePattern : MonoBehaviour
{
	[SerializeField] private readonly TextMeshProUGUI showPattern;
	[SerializeField] private readonly TextMeshProUGUI showPatternConverted;
	[SerializeField] private readonly Toggle[] toggles;
	[SerializeField] private readonly TextMeshProUGUI btnSubmit;
	[SerializeField] private readonly TextMeshProUGUI btnClear;
    [SerializeField] private readonly TextMeshProUGUI btnCancel;
	private List<bool> pattern = new List<bool>();
	private Board newPattern;
	void Start()
    {		
        try
        {
			this.btnSubmit.text = Localization.GetMessage("CreatePattern","Submit");
			this.btnClear.text = Localization.GetMessage("CreatePattern", "Clear");
			this.btnCancel.text = Localization.GetMessage("CreatePattern", "Cancel");
		}
        catch (KeyNotFoundException exception)
        {
			Debug.LogError(exception);
        }
		FillEmptyPattern();
		newPattern = new Board();
		foreach (Toggle toggle in toggles)
		{
			Toggle captured = toggle;
			toggle.onValueChanged.AddListener((value) => ToggleStateChanged(captured, value));			
		}
	}
	public void SavePattern()
    {
		newPattern.Pattern = ConvertToArray();
		PrintPattern();
		PrintArrayBi(newPattern.Pattern);
    }
	public void ClearPattern()
    {
		foreach(Toggle toggle in toggles)
        {
			toggle.GetComponent<Toggle>().isOn = false;
        }
    }
	public void Cancel()
    {
		UnityEngine.SceneManagement.SceneManager.LoadScene("SignUp");
	}
	private void ToggleStateChanged(Toggle toggle, bool state)
	{
		this.pattern.RemoveAt((Convert.ToInt32(toggle.GetComponent<Toggle>().name) - 1));
		this.pattern.Insert((Convert.ToInt32(toggle.GetComponent<Toggle>().name)-1), state);
	}
	
	private void FillEmptyPattern()
    {
		int count = 0;
		while (count < 25)
		{
			this.pattern.Add(false);
			count++;
		}
	}

	private void PrintPattern()
	{
		string pattern = null;
		int count = 0;
		foreach(bool cell in this.pattern)
        {
			pattern += Convert.ToInt32(cell);
			if(count == 4)
            {
				pattern += "\n";
				count = 0;
            }
			else
            {
				count++;
			}			
        }
		this.showPattern.text = pattern;
	}

	private bool[,] ConvertToArray()
    {
		int i = 0, j = 0;
		bool[,] converted = new bool[5, 5];
		foreach (bool cell in this.pattern)
		{
			converted[i, j] = cell;
			if (j == 4)
			{
				i++;
				j = 0;
			}
			else
			{
				j++;
			}			

		}
		return converted;
    }

	private void PrintArrayBi(bool[,] matrix)
    {
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < 5; i++)
		{
			for (int j = 0; j < 5; j++)
			{
				stringBuilder.Append(Convert.ToInt32(matrix[i, j]) + " ");
			}
			stringBuilder.Append("\n");
		}
		this.showPatternConverted.text = stringBuilder.ToString();
	}
}
