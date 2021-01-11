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
	private readonly List<bool> _pattern = new List<bool>();
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
        catch (KeyNotFoundException keyNotFoundException)
        {
			Debug.LogError(keyNotFoundException);
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
		this._pattern.RemoveAt((Convert.ToInt32(toggle.GetComponent<Toggle>().name) - 1));
		this._pattern.Insert((Convert.ToInt32(toggle.GetComponent<Toggle>().name)-1), state);
	}	
	private void FillEmptyPattern()
    {
		int count = 0;
		while (count < 25)
		{
			this._pattern.Add(false);
			count++;
		}
	}

	private bool IsEmpty()
	{
		bool isEmpty = true;
		int count = 0;
		while (count < 25 && isEmpty)
		{
			isEmpty = !this._pattern[count];
			count++;
		}

		return isEmpty;
	}
	

	private void PrintPattern()
	{
		StringBuilder stringPattern = new StringBuilder();
		int count = 0;
		foreach(bool cell in this._pattern)
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
		foreach (bool cell in this._pattern)
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

	public void OnClickSavePattern()
	{
		string gameModeName = Regex.Replace(txtGameModeName.text, @"[^\u0000-\u007F]+", string.Empty);
		Player player = (Player) Memory.Load("player");
		player.Board.GameMode = Regex.Replace(this.txtGameModeName.text, @"[^\u0000-\u007F]+", string.Empty);
		player.Board.Pattern = ConvertToArray();
		newPattern.Pattern = ConvertToArray();
		if(!string.IsNullOrEmpty(gameModeName))
        {
	        this.imgGameMode.GetComponent<Image>().color = Util.GetHexColor("#ffffff");
	        if (!IsEmpty())
	        {
		        player.Board.SavePattern(((Player)Memory.Load("player")).Email);   
	        }
	        else
	        {
		        this.showPatternConverted.text = Localization.GetMessage("CreatePattern", "EmptyPattern");
	        }
		}
		else
		{
			this.imgGameMode.GetComponent<Image>().color = Util.GetHexColor("#ffbaba");
		}
		PrintPattern();
		//this.showPatternConverted.text = Util.PrintArrayBi(player.Board.Pattern);
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
