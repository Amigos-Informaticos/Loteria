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
    void Start()
    {
        this.txtPlayerOne = Localization.GetMessage();
    }
}
