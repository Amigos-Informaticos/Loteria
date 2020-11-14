using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class TCPSocket {
	public string Server { get; set; }
	public int Port { get; set; }
	public List<Command> Messages { get; set; }
	private TcpClient _client;
	private NetworkStream _stream;
	public static readonly int MAXTIMEOUT = 500;
	private volatile string _response;

	public TCPSocket(string server, int port) {
		this.Server = server;
		this.Port = port;
		this.Messages = new List<Command>();
	}

	private bool IsPrepared() {
		bool prepared = false;
		if (this._client == null) {
			this._client = new TcpClient();
			try {
				this._client.Connect(this.Server, this.Port);
				prepared = true;
			}
			catch (Exception e) {
				Debug.Log(e);
			}
		}
		if (!this._client.Connected) {
			this._client.Connect(this.Server, this.Port);
		}
		if (this._stream == null) {
			this._stream = this._client.GetStream();
		}
		return prepared;
	}

	public void Close() {
		this._stream.Close();
		this._client.Close();
	}

	public void AddCommand(Command command) {
		this.Messages.Add(command);
	}

	public void SendCommand() {
		this.SendAndGet();
	}

	private void SendAndGet() {
		this.Send();
		this.Read();
	}

	public void Send() {
		if (this.Messages.Count == 0) {
			return;
		}
		try {
			this.IsPrepared();
			byte[] data = Encoding.UTF8.GetBytes(this.Messages[0].GetJSON());
			try {
				this._stream.Write(data, 0, data.Length);

				this.Messages.RemoveAt(0);
			}
			catch (Exception e) {
				Console.Error.Write(e.Message);
			}
		}
		catch (Exception e) {
			Console.Error.Write(e.Message);
		}
	}

	public string GetResponse() {
		string response = null;
		response = this._response;
		return response;
	}

	public string Read(bool wait = false) {
		byte[] received = new byte[1024];
		string response = "NO RESPONSE";
		int tamanio = 0;
		try {
			this.IsPrepared();
			if (!wait) {
				this._stream.ReadTimeout = MAXTIMEOUT;
			} else {
				this._stream.ReadTimeout = 600000;
			}
			tamanio = this._stream.Read(received, 0, received.Length);
			response = Encoding.UTF8.GetString(received, 0, tamanio);
		}
		catch (IOException exception) {
			response = "ERROR. TIMEOUT";
		}
		catch (Exception e) {
			Debug.Log(e);
		}
		this._response = response;
		return response;
	}

	public string Chat(Command message = null) {
		string response = "NO RESPONSE";
		if (this.Messages.Count <= 0 && message == null) return null;
		if (message != null) {
			if (this.Messages.Count > 0) {
				this.Messages.Insert(0, message);
			} else {
				this.Messages.Add(message);
			}
		}
		try {
			if (this.IsPrepared()) {
				byte[] data = Encoding.UTF8.GetBytes(this.Messages[0].GetJSON());
				try {
					this._stream.Write(data, 0, data.Length);
					this.Messages.RemoveAt(0);
					data = new byte[1024];
					this._stream.ReadTimeout = MAXTIMEOUT;
					int tamanio = this._stream.Read(data, 0, data.Length);
					response = Encoding.UTF8.GetString(data, 0, tamanio);
				}
				catch (IOException) {
					response = "ERROR. TIMEOUT";
				}
				catch (Exception e) {
					Debug.Log(e);
				}
			}
		}
		catch (Exception e) {
			Debug.Log(e);
		}
		return response;
	}
}