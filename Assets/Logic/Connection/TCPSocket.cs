using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;

public class TCPSocket {
	public string Server { get; set; }
	public int Port { get; set; }
	public List<string> Messages { get; set; }
	private TcpClient _client;
	private NetworkStream _stream;
	private const int MAXTIMEOUT = 750;

	public TCPSocket(string server, int port) {
		this.Server = server;
		this.Port = port;
		this.Messages = new List<string>();
	}

	private void Prepare() {
		this._client ??= new TcpClient(this.Server, this.Port);
		if (!this._client.Connected) {
			this._client.Connect(this.Server, this.Port);
		}
		this._stream ??= this._client.GetStream();
	}

	public void Close() {
		this._stream.Close();
		this._client.Close();
	}

	public void AddMessage(string message) {
		this.Messages.Add(message);
	}

	public bool SendMessage(Command command) => this.SendMessage(command.GetJSON());

	public bool SendMessage(string message = null) {
		bool sent = false;
		if (this.Messages.Count <= 0 && message == null) return false;
		if (message != null) {
			if (this.Messages.Count > 0) {
				this.Messages.Insert(0, message);
			} else {
				this.Messages.Add(message);
			}
		}
		try {
			this.Prepare();
			byte[] data = Encoding.UTF8.GetBytes(this.Messages[0]);
			try {
				this._stream.Write(data, 0, data.Length);

				this.Messages.RemoveAt(0);
				sent = true;
			}
			catch (Exception e) {
				Console.Error.Write(e.Message);
			}
		}
		catch (Exception e) {
			Console.Error.Write(e.Message);
		}
		return sent;
	}

	public string Read() {
		byte[] received = new byte[1024];
		string response = "NO RESPONSE";
		int tamanio = 0;
		try {
			this.Prepare();
			this._stream.ReadTimeout = MAXTIMEOUT;
			tamanio = this._stream.Read(received, 0, received.Length);
			response = Encoding.UTF8.GetString(received, 0, tamanio);
		}
		catch (IOException exception) {
			response = "ERROR. TIMEOUT";
		}
		catch (Exception e) {
			Console.Error.Write(e.Message);
		}
		return response;
	}

	public string Chat(Command command) => this.Chat(command.GetJSON());

	public string Chat(string message = null) {
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
			this.Prepare();
			byte[] data = Encoding.UTF8.GetBytes(this.Messages[0]);
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
				Console.Error.Write(e.Message);
			}
		}
		catch (Exception e) {
			Console.Error.Write(e.Message);
		}
		return response;
	}
}