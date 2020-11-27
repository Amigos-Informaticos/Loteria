using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyScript : MonoBehaviour
{
    [SerializeField] private Image[] board = new Image[25];
    [SerializeField] private Sprite cardChoosed;
    private Player player;
    
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
        for (int i = 0; i < 25; i++)
        {
            board[i].GetComponent<Image>().sprite = this.CreateSprite(i + 1);
        }
    }
    
    
}
