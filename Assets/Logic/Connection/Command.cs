using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Command
{
	public string Method;
	public Dictionary<string, string> Arguments;

	public Command(string method) {
		this.Method = method;
		this.Arguments = new Dictionary<string, string>();
	}

	public void AddArgument(string name, string value) {
		this.Arguments.Add(name, value);
	}

	public string GetJSON() {
		return JsonUtility.ToJson(this).ToString();
	}

	public void FromResponse(string json) {
		JsonUtility.FromJson<Command>(json);
	}
}