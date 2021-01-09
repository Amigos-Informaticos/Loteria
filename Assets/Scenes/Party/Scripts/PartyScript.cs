using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PartyScript : MonoBehaviour
{
    [SerializeField] private Image[] board = new Image[25];
    [SerializeField] private Image cardToShow;
    [SerializeField] private TextMeshProUGUI taChat;
    [SerializeField] private TextMeshProUGUI txtChat;
    private Player _player;
    private Room _room;
    private readonly int[] _cards = new int[54];
    private int _cardOnScreen;

    void Start()
    {
        _room = (Room) Memory.Load("room");
        _player = (Player) Memory.Load("player");
        _cardOnScreen = 0;
        for (int i = 0; i < 54; i++)
        {
            _cards[i] = i + 1;
        }
        this.GenerateBoard();
        IEnumerator coroutine = ChangeCard(0.5f);
        IEnumerator chatCoroutine = UpdateChat();
        StartCoroutine(coroutine);
        StartCoroutine(chatCoroutine);
    }

    private IEnumerator ChangeCard(float waitTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);
            ChangeSpriteOfCard(_cards[_cardOnScreen]);
            Debug.Log(_cardOnScreen);
            _cardOnScreen ++;
        }
    }
    
    private IEnumerator UpdateChat()
    {
        int counter = 0;
        while (true)
        {
            yield return new WaitForSeconds(3.0f);
            _room.GetMessages(_player.Email);
            while (counter < _room.Messages.Count)
            {
                this.taChat.GetComponent<TextMeshProUGUI>().text = this.taChat.text + "\n" + _room.Messages[counter].Key +
                                                                   ": " + _room.Messages[counter].Value;
                counter++;
            }
        }
    }

    public void ChangeSpriteOfCard(int index)
    {
        cardToShow.GetComponent<Image>().sprite = CreateSpriteOfACard(index);
    }

    public Sprite CreateSpriteOfACard(int idCard)
    {
        Texture2D texture = Resources.Load("Images/Cards/" + idCard) as Texture2D;
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        return sprite;
    }

    public void GenerateBoard()
    {
        int idBoardCard = 0;
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                board[idBoardCard].GetComponent<Image>().sprite = this.CreateSpriteOfACard(_player.Board.Cards[i,j]);
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
                                                               ": " + this.txtChat.text + "<color = #ff0000ff> ERROR </color>";
        }
        this.txtChat.text = "";
    }
}