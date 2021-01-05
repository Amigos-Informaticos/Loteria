﻿using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CreatePartyScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtPlayers;
    [SerializeField] private TextMeshProUGUI txtRounds;
    [SerializeField] private TextMeshProUGUI txtGameMode;
    [SerializeField] private TextMeshProUGUI txtSpeed;
    [SerializeField] private TextMeshProUGUI btnCreate;
    [SerializeField] private TextMeshProUGUI btnBack;
    [SerializeField] private TMP_Dropdown dpGameMode;
    [SerializeField] private TMP_Dropdown dpPlayers;
    [SerializeField] private TMP_Dropdown dpRounds;
    [SerializeField] private TMP_Dropdown dpSpeed;
    [SerializeField] private TextMeshProUGUI txtFeedBackMessage;
    private List<TMP_Dropdown.OptionData> gameModeOptions;
    private readonly Room room = new Room();   
    private int gameModeSelectedIndex;
    private int numberPlayers = 2;
    private int numberRounds = 1;
    private int speed = 3;
    void Start()
    {
        this.txtPlayers.text = Localization.GetMessage("CreateParty", "Players");
        this.txtRounds.text = Localization.GetMessage("CreateParty", "Rounds");
        this.txtGameMode.text = Localization.GetMessage("CreateParty", "GameMode");
        this.txtSpeed.text = Localization.GetMessage("CreateParty", "Speed");
        this.btnCreate.text = Localization.GetMessage("CreateParty", "Create");
        this.btnBack.text = Localization.GetMessage("CreateParty", "Back");
        this.gameModeOptions = dpGameMode.GetComponent<TMP_Dropdown>().options;
        InstanceRoom();
        FillGameModes();        
    }

    private void FillGameModes()
    {
        try
        {
            List<string> gameModes = this.room.GetGameModes();
            if (gameModes != null)
            {
                foreach (string gameMode in gameModes)
                {
                    this.gameModeOptions.Add(new TMP_Dropdown.OptionData(gameMode));
                }
            }
        }
        catch (NullReferenceException)
        {
            Debug.Log("Sin modos de juego");
        }        
    }

    public void OnValueChangedGameMode()
    {
        this.gameModeSelectedIndex = this.dpGameMode.value;
        Debug.Log(this.gameModeSelectedIndex);
        Debug.Log(gameModeOptions[this.gameModeSelectedIndex].text);
    }
    public void OnValueChangedPlayers()
    {
        this.numberPlayers = this.dpPlayers.value + 2;
    }
    public void OnValueChangedRounds()
    {
        this.numberRounds = this.dpRounds.value + 1;
    }
    public void OnValueChangedSpeed()
    {
        this.speed = this.dpSpeed.value + 3;
    }
    public void OnClickBackToLetsPlay()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("LetsPlay");
    }    
    public void OnClickGoToLobby()
    {        
        InstanceRoom();
        Debug.Log(this.room.ToString());
        this.room.MakeRoom();
        if (EvaluateResponseMakeRoom())
        {
            this.room.GetPlayersInRoom();
            Memory.Save("room", this.room);
            UnityEngine.SceneManagement.SceneManager.LoadScene("Lobby");
        }
    }
    public void OnClickNewGameMode()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("CreatePattern");
    }
    private void InstanceRoom()
    {
        this.room.Host = (Player) Memory.Load("player");       
        this.room.Rounds = this.numberRounds;
        this.room.Speed = this.speed;
        this.room.NumberPlayers = this.numberPlayers;
        this.room.GameMode = this.gameModeOptions[this.gameModeSelectedIndex].text;
    }
    private bool EvaluateResponseMakeRoom()
    {
        bool isMaked = true;
        switch (this.room.IdRoom)
        {
            case "ROOM ALREADY EXISTS":
                isMaked = false;
                this.txtFeedBackMessage.text = Localization.GetMessage("CreateParty", "RoomAlreadyExist");
                break;
            case "ERROR":
            case "ERROR. TIMEOUT":
                isMaked = false;
                this.txtFeedBackMessage.text = Localization.GetMessage("CreateParty", "WrongConnection");
                break;
            default:                
                break;
        }
        return isMaked;
    }
}
