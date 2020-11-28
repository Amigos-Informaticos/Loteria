using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyScript : MonoBehaviour
{
    [SerializeField] private Image[] board = new Image[25];
    [SerializeField] private Image cardToShow;
    private readonly Player player = new Player();
    private int[] cards = new int[54];
    
    private int seconds;
    
    int card = 1;

    private IEnumerator coroutine;
    
    void Start()
    {
        seconds = 0;
        this.GenerateBoard();
        coroutine = ChangeCard(0.5f);
        StartCoroutine(coroutine);
    }

    private IEnumerator ChangeCard(float waitTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);
            ChangeSpriteOfCard(card);
            //cardToShow.GetComponent<Image>().sprite = this.CreateSpriteOfACard(card);
            Debug.Log(card);
            card ++;
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
                board[idBoardCard].GetComponent<Image>().sprite = this.CreateSpriteOfACard(player.Board.Cards[i,j]);
                idBoardCard++;
            }
        }
    }
}
