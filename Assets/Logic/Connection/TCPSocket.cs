using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

public class TCPSocket
{
	public string Server { get; set; }
	public int Port { get; set; }
	private List<Command> Messages { get; set; }
	private TcpClient _client;
	private NetworkStream _stream;
	private static readonly int MAXTIMEOUT = 500;
	private readonly List<string> _responses = new List<string>();

	public TCPSocket(string server, int port)
	{
		this.Server = server;
		this.Port = port;
		this.Messages = new List<Command>();
	}

	private bool IsPrepared()
	{
		bool prepared = false;
		if (this._client == null || !this._client.Connected)
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
			}
			catch (SocketException exception)
			{
				Debug.Log("SocketException: " + exception + "With code: " + exception.ErrorCode);
			}
			catch (ObjectDisposedException exception)
			{
				Debug.Log("ObjectDisposedException: " + exception);
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
				Debug.Log("SocketException: " + exception + "With code: " + exception.ErrorCode);
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

	public string GetResponse() => this.GetResponse(false, 500);

	public string GetResponse(bool wait, int timeOut)
	{
		string response = null;
		this.Read(wait, timeOut);
		if (this._responses.Count > 0)
		{
			response = this._responses[0];
			this._responses.RemoveAt(0);
		}
		return response;
	}

	public string GetSavedResponse()
	{
		string response = null;
		if (this._responses.Count > 0)
		{
			response = this._responses[0];
			this._responses.RemoveAt(0);
		}
		return response;
	}

	private void Read(bool wait, int timeOut)
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
		catch (IOException)
		{
			response = "ERROR. TIMEOUT";
		}
		catch (Exception e)
		{
			Debug.Log(e);
		}
		this._responses.Add(response);
	}
}