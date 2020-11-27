using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyScript : MonoBehaviour
{
    [SerializeField] private Image[] board = new Image[25];
    [SerializeField] private Sprite cardChoosed;
    private Player player = new Player();
    
    // Start is called before the first frame update
    void Start()
    {
        this.generateBoard();
    }

    public Sprite CreateSprite(int idCard)
    {
        Texture2D texture = Resources.Load("Images/Cards/" + idCard) as Texture2D;
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        return sprite;
    }
    
    

    // Update is called once per frame
    void Update()
    {
        
    }
    
    
    public void generateBoard()
    {
        int idBoardCard = 0;
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                Debug.Log(idBoardCard);
                //Debug.Log(player.Board.Cards[i,j]);
                board[idBoardCard].GetComponent<Image>().sprite = this.CreateSprite(player.Board.Cards[i,j]);
                idBoardCard++;
                
            }
            
        }
    }
    
    
}
