using System.IO;
using UnityEngine;

public class Connection {
	private TCPSocket socket;

	public Connection() {
		this.LoadFromJSON();
	}

	public Connection(string serverIP, int port) {
		this.socket = new TCPSocket(serverIP, port);
	}

	public bool LoadFromJSON(string connectionConfigFile = "/Configuration/connection.json") {
		if (!File.Exists(connectionConfigFile)) return false;
		if (File.ReadAllBytes(connectionConfigFile).Length <= 0) return false;
		using (StreamReader reader = new StreamReader(connectionConfigFile)) {
			string configurations = reader.ReadToEnd();
			this.socket = JsonUtility.FromJson<TCPSocket>(configurations);
		}
		return true;
	}

	public void SaveToJSON(string connectionConfigFile = "/Configuration/connection.json") {
		string json = JsonUtility.ToJson(this.socket, true);
		StreamWriter writer = new StreamWriter(connectionConfigFile, false);
		writer.Write(json);
	}
}