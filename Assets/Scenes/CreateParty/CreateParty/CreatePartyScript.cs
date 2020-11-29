using System.Collections;
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
    
    void Start()
    {
        this.txtPlayers.text = Localization.GetMessage("CreateParty","Players");
        this.txtRounds.text = Localization.GetMessage("CreateParty","Rounds");
        this.txtGameMode.text = Localization.GetMessage("CreateParty","GameMode");
        this.txtSpeed.text = Localization.GetMessage("CreateParty","Speed");
        this.btnCreate.text = Localization.GetMessage("CreateParty","Create");
        this.btnBack.text = Localization.GetMessage("CreateParty","Back");
    }

    public void BackToLetsPlay()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("LetsPlay");
    }
}
