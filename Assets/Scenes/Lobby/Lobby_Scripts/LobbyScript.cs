using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LobbyScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtPlayerOne;
    [SerializeField] private TextMeshProUGUI txtPlayerTwo;
    [SerializeField] private TextMeshProUGUI txtPlayerThree;
    [SerializeField] private TextMeshProUGUI txtPlayerFour;
    [SerializeField] private TextMeshProUGUI btnLetsGo;
    [SerializeField] private TextMeshProUGUI btnBack;
    void Start()
    {
        this.txtPlayerOne.text = Localization.GetMessage("Lobby","PlayerOne");
        this.txtPlayerTwo.text = Localization.GetMessage("Lobby","PlayerTwo");
        this.txtPlayerThree.text = Localization.GetMessage("Lobby","PlayerThree");
        this.txtPlayerFour.text = Localization.GetMessage("Lobby","PlayerFour");
        this.btnLetsGo.text = Localization.GetMessage("Lobby","LetsGo");
        this.btnBack.text = Localization.GetMessage("Lobby","Back");
    }
}
