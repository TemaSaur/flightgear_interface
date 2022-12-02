using System;

namespace flightgear_interface;

public static class Util
{
	public static double ParseDouble(this string textNumber)
	{
		textNumber = textNumber.Replace('.', ',');
		return double.Parse(textNumber);
	}
}