using System;
using System.Globalization;
using System.Linq;

namespace flightgear_interface.Connection;

public class FGData
{
	public double Aileron { get; set; }
	public double Elevator { get; set; }
	public double Throttle { get; set; }
	public double Rudder { get; set; }

	private readonly Type _type;

	public FGData()
	{
		_type = typeof(FGData);
	}

	public FGData(string output) : this()
	{
		var tokens = output.Split(';').Select(double.Parse).ToArray();
		Aileron = tokens[0];
		Elevator = tokens[1];
		Throttle = tokens[2];
		Rudder = tokens[3];
	}

	public FGData(double aileron, double elevator, double throttle, double rudder)  : this()
	{
		Aileron = aileron;
		Elevator = elevator;
		Throttle = throttle;
		Rudder = rudder;
	}
	
	public void Set(string name, double value)
	{
		_type.GetProperty(name)!.SetValue(this, value);
	}

	public string GetCommand()
	{
		return $"{Aileron};{Elevator};{Throttle};{Rudder}\n";
	}
}