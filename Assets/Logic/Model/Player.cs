using Assets.Logic.Util;
using System;
using System.Text.RegularExpressions;

public class Player {
	private string names;
	private string lastName;
	private string email;
	private string password;
	public string Names
    {
        get
        {
			return this.names;
        }
        set
        {            
            if (Player.IsName(value))
            {
				this.names = value;
			}
            else
            {
				this.names = null;
            }
			
		}
    }

	public string LastName
    {
        get
        {
			return this.lastName;
        }
        set
        {
            if (Player.IsName(value))
            {
				this.lastName = value;
            }
            else
            {
				this.lastName = null;
            }
        }
    }

	public string Email
    {
        get
        {
			return this.email;
        }
		set
        {
			if (Player.IsEmail(value))
			{
				this.email = value;
			}
            else
            {
				this.email = null;
            }
        }
    }

	public string Password
    {
        get
        {
			return this.password;
        }
        set
        {
			this.password = Util.Encrypt(value);
        }
    }

    public int Score { get; set; }
	public Board Board { get; set; }

	public Player() {
		this.Score = 0;
		this.Board = new Board();
	}

	public static bool IsName(string names)
	{
		bool isName = false;
		Regex regex = new Regex("^[A-Z]+(([',. -][a-zA-Z ])?[a-zA-Z]*)*$");
		if (regex.IsMatch(names))
		{
			isName = true;
		}
		
		return isName;
	}

	public static bool IsEmail(string email)
    {
		bool isEmail = false;
		Regex regex = new Regex("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$");
        if (regex.IsMatch(email))
        {
			isEmail = true;
        }        
		return isEmail;
    }

	public bool IsComplete()
    {
		return this.email != null && this.names != null && this.lastName != null;
    }

	public void MakeNewBoard() {
		this.Board = new Board();
	}

	public bool HaveWon() {
		bool won = true;
		for (int i = 0; i < 5; i++) {
			for (int j = 0; j < 5; j++) {
				if (this.Board.Pattern[i, j] == this.Board.Marks[i, j]) continue;
				won = false;
				break;	
			}
			if (!won) {
				break;
			}
		}
		return won;
	}
}