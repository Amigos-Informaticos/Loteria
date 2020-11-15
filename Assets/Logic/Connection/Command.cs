using System.Collections.Generic;

public class Command {
	public string Method { get; set; }
	public List<string> Arguments { get; set; }

	public Command(string method) {
		this.Method = method;
		this.Arguments = new List<string>();
	}

	public void AddArgument(string name, string value) {
		this.Arguments.Add(name);
		this.Arguments.Add(value);
	}

	public string GetJSON() {
		string json = "{\"Method\":\"";
		json += this.Method;
		json += "\",\"Arguments\":{";
		for (int i = 0; i < this.Arguments.Count;) {
			json += "\"";
			json += this.Arguments[i];
			json += "\":\"";
			i++;
			json += this.Arguments[i];
			json += "\"";
			if (i < this.Arguments.Count - 1) {
				json += ",";
			}
			i++;
		}
		json += "}}";
		return json;
	}
}