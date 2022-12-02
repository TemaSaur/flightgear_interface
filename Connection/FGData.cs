using System;
using System.Globalization;
using System.Linq;

namespace flightgear_interface.Connection;

public class FGData
{
	public double Aileron { get; set; }
	public double Elevator { get; set; }
	public double Throttle { get; set; }

	private readonly Type _type;
	private readonly CultureInfo _cultureInfo = new("en-US");

	public FGData()
	{
		_type = typeof(FGData);
	}

	public FGData(string output) : this()
	{
		var tokens = output.Split(';').Select(s => s.ParseDouble()).ToArray();
		Aileron = tokens[0];
		Elevator = tokens[1];
		Throttle = tokens[2];
	}

	public FGData(double aileron, double elevator, double throttle)  : this()
	{
		Aileron = aileron;
		Elevator = elevator;
		Throttle = throttle;
	}
	
	public void Set(string name, double value)
	{
		_type.GetProperty(name)!.SetValue(this, value);
	}

	public string GetCommand()
	{
		return $"{Aileron.ToString(_cultureInfo)};{Elevator.ToString(_cultureInfo)};{Throttle.ToString(_cultureInfo)}\n";
	}
}