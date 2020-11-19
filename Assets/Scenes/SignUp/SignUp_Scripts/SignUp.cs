using System;
using System.Text;
using System.Text.RegularExpressions;
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
	public TextMeshProUGUI emailPlaceHolder;
	public Text feedbackMessage;

	private TCPSocket socket;

	private void Start()
    {
		
	}
	public void SignUpPlayer()
	{		
		Player player = InstancePlayer();
		string response = null;
		Debug.Log(player.Names);
		Debug.Log(player.NickName);
		Debug.Log(player.LastName);
		Debug.Log(player.Email);
		Debug.Log(player.Password);
		Debug.Log(player.Code);
		if (player.IsComplete())
        {
			if (String.Equals(this.txtPassword.text, this.txtPasswordConfirm.text))
			{
				response = player.SignUp();
				if (String.Equals(response, "OK"))
				{
					feedbackMessage.text = Localization.GetMessage("SignUp", "SignUpSuccess");
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
			else
			{
				feedbackMessage.text = Localization.GetMessage("SignUp", "IncompleteFields");
			}
		} 
		else
        {
			feedbackMessage.text = Localization.GetMessage("SigUp", "IncompleteFields");
        }		
		Debug.Log(response);
		
	}

	public void SendCodeToEmail()
	{
		Player player = InstancePlayer();			
		string response = player.SendCode();
        if (player.IsComplete())
        {
			if (String.Equals(response, "OK"))
			{
				this.feedbackMessage.text = "Código enviado con éxito";
			}
			else if (String.Equals(response, "WRONG EMAIL"))
			{
				this.feedbackMessage.text = "No se pudo enviar el correo, verifique su email";
			}
		} 
		else
        {
			this.feedbackMessage.text = "Campos incompletos";
        }
		this.socket.Close();
	}

	public void BackToMainMenu()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene(0);
	}

	public Player InstancePlayer()
    {
		Player player = new Player();		
		player.Email = Regex.Replace(txtEmail.text, @"[^\u0000-\u007F]+", string.Empty);
		player.NickName = Regex.Replace(txtNickname.text, @"[^\u0000-\u007F]+", string.Empty);
		player.Password = Regex.Replace(txtPassword.text, @"[^\u0000-\u007F]+", string.Empty);
		player.Names = Regex.Replace(txtName.text, @"[^\u0000-\u007F]+", string.Empty);
		player.LastName = Regex.Replace(txtLastame.text, @"[^\u0000-\u007F]+", string.Empty);
		player.Code = Regex.Replace(txtConfirmationCode.text, @"[^\u0000-\u007F]+", string.Empty);
		return player;
    }
}