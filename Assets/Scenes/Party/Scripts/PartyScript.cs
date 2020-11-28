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
    
    
    void Start()
    {
        this.GenerateBoard();
        InvokeRepeating();
        
    }

    private void Update()
    {
        this.RunTheCards();
    }

    public Sprite CreateSprite(int idCard)
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
                board[idBoardCard].GetComponent<Image>().sprite = this.CreateSprite(player.Board.Cards[i,j]);
                idBoardCard++;
            }
        }
    }

    public void RunTheCards()
    {
        
        for (int i = 0; i < 54; i++)
        {
            StartCoroutine(ExecuteAfterTime(1f, i + 1));
        }
    }
    
    IEnumerator ExecuteAfterTime(float time, int index)
    {
        yield return new WaitForSeconds(time);
 
        cardToShow.GetComponent<Image>().sprite = this.CreateSprite(index);
    }
}
