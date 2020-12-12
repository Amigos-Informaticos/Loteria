using System.Collections.Generic;
using System.Text;

public class Command
{
	private string Method { get; set; }
	private List<string> Arguments { get; set; }

	public Command(string method)
	{
		this.Method = method;
		this.Arguments = new List<string>();
	}

	public void AddArgument(string name, string value)
	{
		this.Arguments.Add(name);
		this.Arguments.Add(value);
	}

	public string GetJSON()
	{
		StringBuilder builder = new StringBuilder();
		builder.Append("{\"Method\":\"");
		builder.Append(this.Method);
		builder.Append("\",\"Arguments\":{");
		for (int i = 0; i < this.Arguments.Count;)
		{
			builder.Append("\"");
			builder.Append(this.Arguments[i]);
			builder.Append("\":\"");
			i++;
			builder.Append(this.Arguments[i]);
			builder.Append("\"");
			if (i < this.Arguments.Count - 1)
			{
				builder.Append(",");
			}
			i++;
		}
		builder.Append("}}");
		return builder.ToString();
	}
}