using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Windows;

namespace flightgear_interface.Connection;

public class FgClient
{
	private const string Url = "127.0.0.1";
	private const int OutputPort = 4444;
	private const int InputPort = 4445;
	private UdpClient OutputSocket { get; }
	private UdpClient InputSocket { get; }
	private FGData FgData { get; }
	private IPEndPoint _ip;

	private IDebug Debug { get; }

	public FgClient()
	{
		_ip = new IPEndPoint(IPAddress.Parse(Url), OutputPort);
		OutputSocket = new UdpClient(OutputPort);
		InputSocket = new UdpClient();
		FgData = new FGData();
		Debug ??= (IDebug)Application.Current.Properties["Debug"]!;
	}

	public string ReceiveData()
	{
		var data = OutputSocket.Receive(ref _ip);
		return Encoding.ASCII.GetString(data);
	}

	private void SendCommand(string command)
	{
		var data = Encoding.ASCII.GetBytes(command);
		InputSocket.Send(data, data.Length, Url, InputPort);
	}

	private void Send()
	{
		var command = FgData.GetCommand();
		SendCommand(command);
	}

	public void Set(string name, double value)
	{
		FgData.Set(name, value);
		Send();
	}
}