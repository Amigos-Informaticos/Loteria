using System.Text;
using TMPro;
using UnityEngine;

public class SignUp : MonoBehaviour
{
	public TextMeshProUGUI txtEmail;

	public TextMeshProUGUI txtNickname;

	public TextMeshProUGUI txtPassword;

	public TextMeshProUGUI txtPasswordConfirm;

	public TextMeshProUGUI txtConfirmationCode;

	public TextMeshProUGUI txtName;

	public TextMeshProUGUI txtLastame;

	private TCPSocket socket;

	public void SignUpPlayer()
	{
		string emailText = this.txtEmail.text;
		string nicknameText = this.txtNickname.text;
		string passwordText = this.txtPassword.text;
		string passwordConfirmText = this.txtPasswordConfirm.text;
		string nameText = this.txtName.text;
		string lastameText = this.txtLastame.text;
		string codeText = this.txtConfirmationCode.text;

		Command command = new Command("sign_up");
		command.AddArgument("email", emailText);
		command.AddArgument("nickname", nicknameText);
		command.AddArgument("password", passwordText);
		command.AddArgument("name", nameText);
		command.AddArgument("lastname", lastameText);
		command.AddArgument("code", codeText);

		TCPSocketConfiguration.BuildDefaultConfiguration(out this.socket);
		this.socket.AddCommand(command);
		this.socket.SendCommand();

		Debug.Log(emailText);
		Debug.Log(nicknameText);
		Debug.Log(passwordText);
		Debug.Log(passwordConfirmText);
		Debug.Log(nameText);
		Debug.Log(lastameText);
		Debug.Log(codeText);
	}

	public void SendCodeToEmail()
	{
		string email = this.txtEmail.text;
		byte[] bytes = Encoding.Default.GetBytes(email);
		email = Encoding.UTF8.GetString(bytes);
		Command command = new Command("send_code_to_email");
		command.AddArgument("email", email);
		TCPSocketConfiguration.BuildDefaultConfiguration(out this.socket);
		this.socket.AddCommand(command);
		this.socket.SendCommand();

		Debug.Log(this.socket.GetResponse(true));
		command = new Command("close");
		this.socket.AddCommand(command);
		this.socket.SendCommand();
		this.socket.Close();
	}

	public void BackToMainMenu()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene(0);
	}
}