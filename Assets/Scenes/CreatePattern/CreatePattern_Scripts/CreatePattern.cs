using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
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
	[SerializeField] private TextMeshProUGUI lblGameModeName;
	[SerializeField] private TextMeshProUGUI phGameModeName;
	[SerializeField] private TextMeshProUGUI txtGameModeName;
	[SerializeField] private Image imgGameMode;
	private readonly List<bool> pattern = new List<bool>();
	private readonly Board newPattern = new Board();
	

	void Start()
    {
        try
        {
			this.btnSave.text = Localization.GetMessage("CreatePattern","Save");
			this.btnClear.text = Localization.GetMessage("CreatePattern", "Clear");
			this.btnCancel.text = Localization.GetMessage("CreatePattern", "Cancel");
			this.lblGameModeName.text = Localization.GetMessage("CreatePattern", "GameMode");
			this.phGameModeName.text = Localization.GetMessage("CreatePattern","GameModePlaceHolder");
		}
        catch (KeyNotFoundException exception)
        {
			Debug.LogError(exception);
        }
		FillEmptyPattern();
		foreach (Toggle toggle in toggles)
		{
			Toggle captured = toggle;
			toggle.onValueChanged.AddListener((value) => ToggleStateChanged(captured, value));			
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

	public void OnClickSavePattern()
	{
		Player player = new Player();
		player.Board.GameMode = Regex.Replace(this.txtGameModeName.text, @"[^\u0000-\u007F]+", string.Empty);
		player.Board.Pattern = ConvertToArray();
		newPattern.Pattern = ConvertToArray();
		if(!string.IsNullOrEmpty(this.txtGameModeName.text))
        {
			player.Board.SavePattern(((Player)Memory.Load("player")).Email);
		}
		else
		{
			this.imgGameMode.GetComponent<Image>().color = Util.GetHexColor("#ffbaba");
		}
		PrintPattern();
		PrintArrayBi(player.Board.Pattern);
	}
	public void OnClickClearPattern()
	{
		foreach (Toggle toggle in toggles)
		{
			toggle.GetComponent<Toggle>().isOn = false;
		}
	}
	public void OnClickCancel()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene("CreateParty");
	}
}
