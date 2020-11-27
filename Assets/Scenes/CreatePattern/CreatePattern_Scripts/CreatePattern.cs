using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreatePattern : MonoBehaviour
{
	public TextMeshProUGUI showPattern;
	public TextMeshProUGUI showPatternConverted;
	public Toggle[] toggles = null;
	private List<bool> pattern = new List<bool>();
	public Button btn1;
	private Board newPattern;
	void Start()
    {
		FillEmptyPattern();
		newPattern = new Board();
		btn1 = GetComponent<Button>();		  		        
		foreach (Toggle toggle in toggles)
		{
			Toggle captured = toggle;
			toggle.onValueChanged.AddListener((value) => ToggleStateChanged(captured, value));			
		}
	}

    void Update() 
	{
		
	}
	public void OnClick()
    {
		newPattern.Pattern = ConvertToArray();
		PrintPattern();
		PrintArrayBi(newPattern.Pattern);
    }
	public void CleanPattern()
    {
		foreach(Toggle toggle in toggles)
        {
			toggle.GetComponent<Toggle>().isOn = false;
        }
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
		string print = null;
		for (int i = 0; i < 5; i++)
		{
			for (int j = 0; j < 5; j++)
			{
				print += Convert.ToInt32(matrix[i, j]) + " ";
			}
			print += "\n";
		}
		this.showPatternConverted.text = print;
	}
}
