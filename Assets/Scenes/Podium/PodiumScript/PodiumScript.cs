using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PodiumScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtLoteria;
    [SerializeField] private TextMeshProUGUI txtWinner;
    [SerializeField] private TextMeshProUGUI txtScore;
    [SerializeField] private TextMeshProUGUI btnExit;
    private Player _player;
    private int _score;
    private string _winner;
    
    void Start()
    {
        _player = (Player) Memory.Load("player");
        _winner = (string) Memory.Load("winner");
        _score = (int) Memory.Load("score");
        ConfigurePodium();
    }

    public void ConfigurePodium()
    {
        if (_winner.Equals(_player.NickName))
        {
            txtLoteria.text = Localization.GetMessage("Podium", "Win");
            txtWinner.text = Localization.GetMessage("Podium", "You are the winner");
        }
        else if (_winner.Equals("NO WINNER"))
        {
            txtLoteria.text = Localization.GetMessage("Podium", "Lose");
            txtLoteria.text = Localization.GetMessage("Podium", "No winner");
        }
        else
        {
            txtLoteria.text = Localization.GetMessage("Podium", "Lose");
            txtWinner.text = Localization.GetMessage("Podium", "The winner is") + _winner;
        }
        txtScore.text = Localization.GetMessage("Podium", "Your score") + _score;
        btnExit.text = Localization.GetMessage("Podium", "Exit");
    }

    public void OnClickExit()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("LetsPlay");
    }
}