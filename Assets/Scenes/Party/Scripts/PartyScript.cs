using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PartyScript : MonoBehaviour
{
    [SerializeField] private Image[] board = new Image[25];
    [SerializeField] private Toggle[] toggles = new Toggle[25];
    [SerializeField] private Image cardToShow;
    [SerializeField] private TextMeshProUGUI taChat;
    [SerializeField] private TextMeshProUGUI txtChat;
    [SerializeField] private TextMeshProUGUI[] txtPlayers = new TextMeshProUGUI[4];
    [SerializeField] private TextMeshProUGUI txtFeedBackMessage;
    [SerializeField] private TextMeshProUGUI txtScore;
    [SerializeField] private TextMeshProUGUI txtScoreWord;
    [SerializeField] private TextMeshProUGUI txtBack;
    [SerializeField] private TextMeshProUGUI txtWriteYourMessage;
    private Player _player;
    private Room _room;
    private int[] _cards = new int[54];
    private int _cardOnScreen;
    private int _currentCard;
    private bool _won;
    private int _score;
    private string _winner;

    void Start()
    {
        try
        {
            txtScoreWord.text = Localization.GetMessage("Party", "Score");
            txtBack.text = Localization.GetMessage("Party", "Back");
            txtWriteYourMessage.text = Localization.GetMessage("Party", "Write your message");
        }
        catch (KeyNotFoundException fileNotFoundException)
        {
            Debug.Log(fileNotFoundException);
        }
        
        _room = (Room) Memory.Load("room");
        _player = (Player) Memory.Load("player");
        _score = 0;
        _cardOnScreen = 0;
        _won = false;
        _cards = Board.GetSortedDeck(_room.IdRoom, _player.Email);
        GenerateBoard();
        this.txtFeedBackMessage.text = Util.PrintArrayBi(_player.Board.Pattern);
        
        foreach (Toggle toggle in toggles)
        {
            Toggle captured = toggle;
            toggle.onValueChanged.AddListener((value) => ToggleStateChanged(captured, value));
        }
        IEnumerator coroutine = ChangeCard(_room.Speed);
        IEnumerator chatCoroutine = UpdateChat();
        IEnumerator waitingForPlayers = WaitingForPlayers();
        IEnumerator checkIfKicked = CheckIfKicked();
        IEnumerator checkIfWinner = CheckIfWinner();
        StartCoroutine(coroutine);
        StartCoroutine(chatCoroutine);
        StartCoroutine(waitingForPlayers);
        StartCoroutine(checkIfKicked);
        StartCoroutine(checkIfWinner);
    }
    private void ToggleStateChanged(Toggle toggle, bool state)
    {
        if (Convert.ToInt32(toggle.GetComponentInChildren<Image>().name) == _currentCard)
        {
            toggle.isOn = true;
            _player.Board.Mark(_currentCard);
            _score += 10;
            txtScore.text = _score.ToString();

            if (_player.Board.IsEmpty())
            {
                Player.Patterns patterns = (Player.Patterns) Memory.Load("patterns");
                if (_player.HaveWon(patterns))
                {
                    Debug.Log("YA GANASTE!!!!");
                    if (_player.NotifyWon(_room.IdRoom, _score))
                    {
                        _score *= 3;
                    }
                }
            }
            else
            {
                if (_player.HaveWon())
                {
                    Debug.Log("YA GANASTE!!!!");
                    if (_player.NotifyWon(_room.IdRoom, _score))
                    {
                        _score *= 3;
                    }
                }
            }
        }
        else
        {
            toggle.isOn = false;
        }
    }
    private IEnumerator ChangeCard(float waitTime)
    {
        while (_cardOnScreen < 54)
        {
            yield return new WaitForSeconds(waitTime);
            ChangeSpriteOfCard(_cards[_cardOnScreen]);
            Debug.Log(_cardOnScreen);
            _cardOnScreen++;
        }
        GoToPodium();
    }

    public IEnumerator WaitingForPlayers()
    {
        while (true)
        {
            yield return new WaitForSeconds(8.0f);
            if (_room.GetPlayersInRoom())
            {
                StartPlayerList();
                SetPlayerList();
            }
            else
            {
                OnClickBack();
            }
        }
    }

    public IEnumerator CheckIfWinner()
    {
        while (true)
        {
            yield return new WaitForSeconds(2.0f);
            _winner = _room.ThereIsAWinner();
            if (!_winner.Equals("NO WINNER")&&!_winner.Equals("ERROR")&&!_winner.Equals("ERROR. TIMEOUT")&&!_winner.Equals("ROOM NOT FOUND"))
            {
                StopAllCoroutines();
                GoToPodium();
            }
        }
    }

    public IEnumerator CheckIfKicked()
    {
        while (true)
        {
            yield return new WaitForSeconds(2.0f);
            if (!_player.IAmInRoom(_room.IdRoom).Equals("OK"))
            {
                GetKicked();               
            }
        }
    }
    
    public void StartPlayerList()
    {
        for (int i = 0; i < _room.NumberPlayers; i++)
        {
            txtPlayers[i].text = Localization.GetMessage("Party", "NoPlayer");
        }
    }

    void SetPlayerList()
    {
        for (int i = 0; i < _room.Players.Count; i++)
        {
            txtPlayers[i].text = _room.Players[i].NickName;
        }
    }

    private IEnumerator UpdateChat()
    {
        int counter = 0;
        while (true)
        {
            yield return new WaitForSeconds(1.0f);
            _room.GetMessages(_player.Email);
            while (counter < _room.Messages.Count)
            {
                this.taChat.GetComponent<TextMeshProUGUI>().text =
                    this.taChat.text + "\n" + _room.Messages[counter].Key +
                    ": " + _room.Messages[counter].Value;
                counter++;
            }
        }
    }

    private void GetKicked()
    {
        ExitRoom();
        UnityEngine.SceneManagement.SceneManager.LoadScene("LetsPlay");
    }

    public void ChangeSpriteOfCard(int index)
    {
        cardToShow.GetComponent<Image>().sprite = CreateSpriteOfACard(index);
        cardToShow.GetComponent<Image>().name = index.ToString();
        Debug.Log("changedSprite:"+cardToShow.GetComponent<Image>().name);
        _currentCard = index;
    }

    public Sprite CreateSpriteOfACard(int idCard)
    {
        Texture2D texture = Resources.Load("Images/Cards/" + idCard) as Texture2D;
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), 
            new Vector2(0.5f, 0.5f));
        return sprite;
    }
    public void GenerateBoard()
    {
        int idBoardCard = 0;
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                board[idBoardCard].GetComponent<Image>().sprite = this.CreateSpriteOfACard(_player.Board.Cards[i, j]);
                board[idBoardCard].GetComponent<Image>().name = _player.Board.Cards[i, j].ToString();
                idBoardCard++;
            }
        }
    }
    public void OnClickSendMessage()
    {
        Room room = (Room) Memory.Load("room");
        string response = room.SendMessage(this.txtChat.text, (Player) Memory.Load("player"));
        if (response.Equals("ERROR") || response.Equals("ERROR. TIMEOUT"))
        {
            this.taChat.GetComponent<TextMeshProUGUI>().text = this.taChat.text + "\n" + _player.NickName +
                                                               ": " + this.txtChat.text +
                                                               "<color = #ff0000ff> ERROR </color>";
        }
        this.txtChat.text = "";
    }

    public void ExitRoom()
    {
        _player.Board = new Board();
        Memory.Save("player",_player);
        _room.ExitRoom(_player.Email);
        Room room = new Room();
        Memory.Save("room",room);
    }

    public void OnClickBack()
    {
        ExitRoom();
        UnityEngine.SceneManagement.SceneManager.LoadScene("LetsPlay");
    }

    public void OnClickKickPlayer(int index)
    {
        string playerToKick = txtPlayers[index].text;
        if (!playerToKick.Equals(Localization.GetMessage("Party", "NoPlayer")))
        {
            _player.KickAPlayer(playerToKick, _room.IdRoom);
        }
    }

    public void GoToPodium()
    {
        _player.SaveScore(_score);
        Memory.Save("winner",_winner);
        Memory.Save("score",_score);
        ExitRoom();
        UnityEngine.SceneManagement.SceneManager.LoadScene("Podium");
    }
}