using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

public class TCPSocket
{
	private string Server { get; set; }
	private int Port { get; set; }
	private List<Command> Messages { get; set; }
	private TcpClient _client;
	private NetworkStream _stream;
	public static readonly int MAXTIMEOUT = 500;
	private volatile string _response;

	public TCPSocket(string server, int port)
	{
		this.Server = server;
		this.Port = port;
		this.Messages = new List<Command>();
	}

	private bool IsPrepared()
	{
		bool prepared = false;
		if (this._client == null)
		{
			this._client = new TcpClient();
			try
			{
				this._client.Connect(this.Server, this.Port);
				prepared = true;
			}
			catch (ArgumentNullException exception)
			{
				Debug.Log("ArgumentNullException: " + exception);
				prepared = false;
			}
			catch (SocketException exception)
            {
				Debug.Log("SocketException: " + exception);
				prepared = false;
            }
			catch (ObjectDisposedException exception)
            {
				Debug.Log("ObjectDisposedException: " + exception);
				prepared = false;
            }
		}
		if (!this._client.Connected)
		{
			try
			{
				this._client.Connect(this.Server, this.Port);
			}
			catch (ArgumentNullException exception)
			{
				Debug.Log("ArgumentNullException: " + exception);
			}
			catch (SocketException exception)
			{
				Debug.Log("SocketException: " + exception);
			}
			catch (ObjectDisposedException exception)
			{
				Debug.Log("ObjectDisposedException: " + exception);
			}
		}
		if (this._stream == null)
		{
			this._stream = this._client.GetStream();
		}
		return prepared;
	}

	public void Close()
	{
		if (this.IsPrepared())
		{
			this.AddCommand(new Command("close"));
			this.SendCommand();
		}
		this._stream.Close();
		this._client.Close();
	}

	public void AddCommand(Command command) => this.Messages.Add(command);

	public void SendCommand() => this.Send();

	public void Send()
	{
		if (this.Messages.Count == 0)
		{
			return;
		}
		try
		{
			this.IsPrepared();
			string message = Regex.Replace(this.Messages[0].GetJSON(), @"[^\u0000-\u007F]+",
				string.Empty);
			byte[] data = Encoding.ASCII.GetBytes(message);
			try
			{
				this._stream.Write(data, 0, data.Length);
				this.Messages.RemoveAt(0);
			}
			catch (Exception e)
			{
				Console.Error.Write(e.Message);
			}
		}
		catch (Exception e)
		{
			Console.Error.Write(e.Message);
		}
	}

	public string GetResponse(bool wait = false, int timeOut = 500) => this.Read(wait, timeOut);

	public string Read(bool wait = false, int timeOut = 500)
	{
		byte[] received = new byte[1024];
		string response = "NO RESPONSE";
		try
		{
			this.IsPrepared();
			this._stream.ReadTimeout = wait ? timeOut : MAXTIMEOUT;
			int size = this._stream.Read(received, 0, received.Length);
			response = Encoding.ASCII.GetString(received, 0, size);
		}
		catch (IOException exception)
		{
			response = "ERROR. TIMEOUT";
		}
		catch (Exception e)
		{
			Debug.Log(e);
		}
		this._response = response;
		return response;
	}

	public string Chat(Command message = null)
	{
		string response = "NO RESPONSE";
		if (this.Messages.Count <= 0 && message == null) return null;
		if (message != null)
		{
			if (this.Messages.Count > 0)
			{
				this.Messages.Insert(0, message);
			} else
			{
				this.Messages.Add(message);
			}
		}
		try
		{
			if (this.IsPrepared())
			{
				byte[] data = Encoding.UTF8.GetBytes(this.Messages[0].GetJSON());
				try
				{
					this._stream.Write(data, 0, data.Length);
					this.Messages.RemoveAt(0);
					data = new byte[1024];
					this._stream.ReadTimeout = MAXTIMEOUT;
					int tamanio = this._stream.Read(data, 0, data.Length);
					response = Encoding.ASCII.GetString(data, 0, tamanio);
				}
				catch (IOException)
				{
					response = "ERROR. TIMEOUT";
				}
				catch (Exception e)
				{
					Debug.Log(e);
				}
			}
		}
		catch (Exception e)
		{
			Debug.Log(e);
		}
		return response;
	}
}