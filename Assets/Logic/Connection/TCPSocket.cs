using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class TCPSocket {
	public string Server { get; set; }
	public int Port { get; set; }
	public List<Command> Messages { get; set; }
	private TcpClient _client;
	private NetworkStream _stream;
	private const int MAXTIMEOUT = 750;
	private Thread _thread;
	private string response;

	public TCPSocket(string server, int port) {
		this.Server = server;
		this.Port = port;
		this.Messages = new List<Command>();
	}

	private void Prepare() {
		if (this._client == null)
		{
			this._client = new TcpClient(this.Server, this.Port);
		}
		if (!this._client.Connected) {
			this._client.Connect(this.Server, this.Port);
		}
		if (this._stream == null)
		{
			this._stream = this._client.GetStream();
		}
	}

	public void Close() {
		this._stream.Close();
		this._client.Close();
	}

	public void AddCommand(Command command) {
		this.Messages.Add(command);
	}

	public void SendCommand() {
		this._thread = new Thread(this.SendAndGet);
		this._thread.Start();
	}

	private void SendAndGet() {
		Thread newThread = new Thread(this.Send);
		newThread.Start();
		newThread.Join();
		newThread = new Thread(this.Read);
		newThread.Start();
		newThread.Join();
	}

	public void Send() {
		if (this.Messages.Count == 0) {
			return;
		}
		try {
			this.Prepare();
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
		if (this._thread != null) {
			this._thread.Join();
			response = this.response;
		}
		return response;
	}

	public void Read() {
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
		this.response = response;
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
			this.Prepare();
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
				Console.Error.Write(e.Message);
			}
		}
		catch (Exception e) {
			Console.Error.Write(e.Message);
		}
		return response;
	}
}