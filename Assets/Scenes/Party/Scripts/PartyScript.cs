using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyScript : MonoBehaviour
{
    [SerializeField] private Image[] board = new Image[25];
    [SerializeField] private Image cardToShow;
    private readonly Player _player = new Player();
    private readonly int[] _cards = new int[54];
    private int _cardOnScreen;

    void Start()
    {
        _cardOnScreen = 0;
        for (int i = 0; i < 54; i++)
        {
            _cards[i] = i + 1;
        }
        this.GenerateBoard();
        IEnumerator coroutine = ChangeCard(0.5f);
        StartCoroutine(coroutine);
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
}