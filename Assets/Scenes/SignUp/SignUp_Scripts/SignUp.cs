using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SignUp : MonoBehaviour
{
	public TextMeshProUGUI txtEmail;

	public TextMeshProUGUI txtNickname;

	public TextMeshProUGUI txtPassword;

	public TextMeshProUGUI txtPasswordConfirm;

	public TextMeshProUGUI txtConfirmationCode;

	public TextMeshProUGUI txtName;

	public TextMeshProUGUI txtLastame;
	
	public Text feedbackMessage;

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

		if (passwordText.Equals(passwordConfirmText))
		{
			TCPSocketConfiguration.BuildDefaultConfiguration(out this.socket);
			this.socket.AddCommand(command);
			this.socket.SendCommand();
			string response = this.socket.GetResponse(true);
			this.CloseSocket();
			if (response.Equals("OK"))
			{
				feedbackMessage.text = "Registro exitoso. Puedes volver al menú.";
			}else if (response.Equals("WRONG CODE"))
			{
				feedbackMessage.text = "El código de verificación no es correcto.";
			}
			else
			{
				feedbackMessage.text = "No fue posible registrar al jugador, inténtelo más tarde";
			}
		}
		else
		{
			feedbackMessage.text = "Verifica que las contraseñas coincidan.";
		}

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
		string response = this.socket.GetResponse(true);
		if (response.Equals("OK"))
		{
			this.feedbackMessage.text="Código enviado con éxito";
		}
		else if(response.Equals("WRONG EMAIL"))
		{
			this.feedbackMessage.text = "No se pudo enviar el correo, verifique su email";
		}
	}

	public void CloseSocket()
	{
		Command command = new Command("close");
		this.socket.AddCommand(command);
		this.socket.SendCommand();
		this.socket.Close();
	}

	public void BackToMainMenu()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene(0);
	}
}