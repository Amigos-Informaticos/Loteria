using System.Text.RegularExpressions;

public class Player
{
	private string names;
	private string lastName;
	private string email;
	private string password;
	private Command command;
	private TCPSocket tcpSocket;

	public string Names
	{
		get => this.names;
		set
		{
			if (IsName(value))
			{
				this.names = value;
			} else
			{
				this.names = null;
			}
		}
	}

	public string LastName
	{
		get => this.lastName;
		set
		{
			if (IsName(value))
			{
				this.lastName = value;
			} else
			{
				this.lastName = null;
			}
		}
	}

	public string NickName { get; set; }

	public string Email
	{
		get => this.email;
		set
		{
			if (IsEmail(value))
			{
				this.email = value;
			} else
			{
				this.email = null;
			}
		}
	}

	public string Password
	{
		get => this.password;
		set { this.password = Util.GetHashString(value); }
	}

	public string Code { get; set; }
	public int Score { get; set; }
	public Board Board { get; set; } = new Board();

	public Player()
	{
		TCPSocketConfiguration.BuildDefaultConfiguration(out this.tcpSocket);
	}

	public bool LogIn()
	{
		bool loggedIn = false;
		this.command = new Command("login");
		this.command.AddArgument("email", this.email);
		this.command.AddArgument("password", this.password);
		this.tcpSocket.AddCommand(this.command);
		this.tcpSocket.SendCommand();
		if (this.tcpSocket.GetResponse().Equals("OK"))
		{
			loggedIn = true;
		}
		return loggedIn;
	}

	public string SignUp()
	{
		string signedUp = "Error";
		if (this.IsComplete())
		{
			this.command = new Command("sign_up");
			this.command.AddArgument("email", this.email);
			this.command.AddArgument("nickname", this.NickName);
			this.command.AddArgument("password", this.password);
			this.command.AddArgument("name", this.names);
			this.command.AddArgument("lastname", this.lastName);
			this.command.AddArgument("code", this.Code);
			this.tcpSocket.AddCommand(this.command);
			this.tcpSocket.SendCommand();
			signedUp = this.tcpSocket.GetResponse();
		}
		return signedUp;
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

	public void MakeNewBoard()
	{
		this.Board = new Board();
	}

	public bool HaveWon()
	{
		bool won = true;
		for (int i = 0; i < 5; i++)
		{
			for (int j = 0; j < 5; j++)
			{
				if (this.Board.Pattern[i, j] == this.Board.Marks[i, j]) continue;
				won = false;
				break;
			}
			if (!won)
			{
				break;
			}
		}
		return won;
	}
}