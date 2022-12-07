using System.Windows;

namespace flightgear_interface.Components;

/// <summary>
/// Represents an object of arguments for the <c>JoystickMoved</c> event
/// </summary>
public class JoystickMovedEventArgs : RoutedEventArgs
{
	/// <summary>
	/// The new X value of the joystick
	/// </summary>
	public double NewX { get; init; }
	
	/// <summary>
	/// The new Y value of the joystick
	/// </summary>
	public double NewY { get; init; }
	
	
	public JoystickMovedEventArgs() { }
	
	public JoystickMovedEventArgs(RoutedEvent e) : base(e) { }
	
	public JoystickMovedEventArgs(RoutedEvent e, object source) : base(e, source) { }
} 