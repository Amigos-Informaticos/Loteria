using System;
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
		InstancePlayer(out Player player);
		string response = null;
		string passwordConfirmText = this.txtPasswordConfirm.text;
		string codeText = this.txtConfirmationCode.text;

		if (String.Equals(this.txtPassword.text, this.txtPasswordConfirm))
		{
			response = player.SignUp();
			if (String.Equals(response, "OK"))
			{
				feedbackMessage.text = "Registro exitoso. Puedes volver al menú.";
			}
			else if (String.Equals(response, "WRONG CODE"))
			{
				feedbackMessage.text = "El código de verificación no es correcto.";
			}
			else
			{
				feedbackMessage.text = "No fue posible registrar al jugador, inténtelo más tarde";
			}
		}
		Debug.Log(response);
		Debug.Log(passwordConfirmText);
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

	public void InstancePlayer(out Player player)
    {		
		player = new Player
		{
			Email = txtEmail.text,
			NickName = txtNickname.text,
			Password = txtPassword.text,
			Names = txtName.text,
			LastName = txtLastame.text,
			Code = txtConfirmationCode.text
		};
    }
}