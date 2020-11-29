using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreatePattern : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI showPattern;
	[SerializeField] private TextMeshProUGUI showPatternConverted;
	[SerializeField] private Toggle[] toggles;
	[SerializeField] private TextMeshProUGUI btnSave;
	[SerializeField] private TextMeshProUGUI btnClear;
    [SerializeField] private TextMeshProUGUI btnCancel;
	private readonly List<bool> pattern = new List<bool>();
	private Board newPattern;
	void Start()
    {		
        try
        {
			this.btnSave.text = Localization.GetMessage("CreatePattern","Submit");
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
	public void OnClickSavePattern()
    {
		newPattern.Pattern = ConvertToArray();
		PrintPattern();
		PrintArrayBi(newPattern.Pattern);
    }
	public void OnClickClearPattern()
    {
		foreach(Toggle toggle in toggles)
        {
			toggle.GetComponent<Toggle>().isOn = false;
        }
    }
	public void OnClickCancel()
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
		StringBuilder stringPattern = new StringBuilder();
		int count = 0;
		foreach(bool cell in this.pattern)
        {
			stringPattern.Append(Convert.ToInt32(cell)+" ");
			if(count == 4)
            {
				stringPattern.Append("\n");
				count = 0;
            }
			else
            {
				count++;
			}			
        }
		this.showPattern.text = stringPattern.ToString();
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
