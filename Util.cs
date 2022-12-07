using System;
using System.Globalization;

namespace flightgear_interface;

public static class Util
{
	public static double MinMax(double min, double value, double max)
	{
		return Math.Max(min, Math.Min(max, value));
	}
}