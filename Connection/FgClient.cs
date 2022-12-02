using System;
using System.Net.Sockets;
using System.Net;
using System.Text;

namespace flightgear_interface.Connection;

public class FgClient
{
	private const string Url = "127.0.0.1";
	private const int OutputPort = 4444;
	private const int InputPort = 4445;
	private IPEndPoint ip;
	private UdpClient outputSocket;
	private UdpClient inputSocket;
	private FGData fg;

	private IDebug debug;

	public FgClient(FGData fg, IDebug debug)
	{
		ip = new IPEndPoint(IPAddress.Parse(Url), OutputPort);
		outputSocket = new UdpClient(OutputPort);
		inputSocket = new UdpClient();
		this.fg = fg;
		this.debug = debug;
	}

	public string ReceiveData()
	{
		var data = outputSocket.Receive(ref ip);
		return Encoding.ASCII.GetString(data);
	}

	public void SendCommand(string command)
	{
		var data = Encoding.ASCII.GetBytes(command);
		inputSocket.Send(data, data.Length, Url, InputPort);
	}

	public void Send()
	{
		var command = fg.GetCommand();
		SendCommand(command);
	}
}