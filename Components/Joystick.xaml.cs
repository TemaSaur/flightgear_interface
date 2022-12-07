using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace flightgear_interface.Components;

public delegate void JoystickMovedEventHandler(object sender, JoystickMovedEventArgs e);

/// <summary>
/// Represents a control that can be used to independently change two properties
/// at the same time by dragging the thumb along the horizontal and vertical axes.
/// </summary>
public partial class Joystick
{
	private const double ThumbSize = 16;
	
	#region DependencyProperties
	#region MinX DependencyProperty
	public static readonly DependencyProperty MinXProperty = DependencyProperty.Register(
		name: nameof(MinX),
		propertyType: typeof(double),
		ownerType: JoystickType,
		typeMetadata: new FrameworkPropertyMetadata(defaultValue: 0.0));
	
	/// <summary>
	/// Gets or sets the minimum value of the X (horizontal) axis
	/// </summary>
	public double MinX
	{
		get => (double)GetValue(MinXProperty);
		set => SetValue(MinXProperty, value);
	}
	#endregion
	
	#region MaxX DependencyProperty
	public static readonly DependencyProperty MaxXProperty = DependencyProperty.Register(
		name: nameof(MaxX),
		propertyType: typeof(double),
		ownerType: JoystickType,
		typeMetadata: new FrameworkPropertyMetadata(defaultValue: 1.0));
	
	/// <summary>
	/// Gets or sets the maximum value of the X (horizontal) axis
	/// </summary>
	public double MaxX
    {
        get => (double)GetValue(MaxXProperty);
        set => SetValue(MaxXProperty, value);
    }
	#endregion
	
	#region MinY DependencyProperty
	public static readonly DependencyProperty MinYProperty = DependencyProperty.Register(
		name: nameof(MinY),
		propertyType: typeof(double),
		ownerType: JoystickType,
		typeMetadata: new FrameworkPropertyMetadata(defaultValue: 0.0));
	
	/// <summary>
	/// Gets or sets the minimum value of the Y (vertical) axis
	/// </summary>
	public double MinY
	{
		get => (double)GetValue(MinYProperty);
		set => SetValue(MinYProperty, value);
	}
	#endregion

	#region MaxY DependencyProperty
	public static readonly DependencyProperty MaxYProperty = DependencyProperty.Register(
		name: nameof(MaxY),
		propertyType: typeof(double),
		ownerType: JoystickType,
		typeMetadata: new FrameworkPropertyMetadata(defaultValue: 1.0));
	
	/// <summary>
	/// Gets or sets the maximum value of the Y (vertical) axis
	/// </summary>
	public double MaxY
	{
		get => (double)GetValue(MaxYProperty);
		set => SetValue(MaxYProperty, value);
	}
	#endregion

	#region X DependencyProperty
	public static readonly DependencyProperty XProperty = DependencyProperty.Register(
		name: nameof(X),
		propertyType: typeof(double),
		ownerType: JoystickType,
		typeMetadata: new FrameworkPropertyMetadata(defaultValue: 0.0));
	
	/// <summary>
	/// Gets or sets the value of horizontal position of the control thumb relative to MinX and MaxX
	/// </summary>
	public double X
	{
		get => MinX + XRelative * (MaxX - MinX);
		set => Left = GetFromRelative(XRelative = (value - MinX) / (MaxX - MinX));
	}
	#endregion
	
	#region Y DependencyProperty
	public static readonly DependencyProperty YProperty = DependencyProperty.Register(
		name: nameof(Y),
		propertyType: typeof(double),
		ownerType: JoystickType,
		typeMetadata: new FrameworkPropertyMetadata(defaultValue: 0.0));
	
	/// <summary>
	/// Gets or sets the value of vertical position of the control thumb relative to MinY and MaxY
	/// </summary>
	public double Y
    {
        get => MinY + YRelative * (MaxY - MinY);
        set => Top = GetFromRelative(YRelative = (value - MinY) / (MaxY - MinY));
    }
	#endregion
	#endregion

	#region Properties
	private double _xRelative = 0.5;
	/// <summary>
	/// Gets the relative position of thumb along the x-axis. Takes values from 0 to 1
	/// </summary>
	/// <exception cref="ArgumentException">The passed value was less than 0 or more than 1</exception>
	private double XRelative
	{
		get => _xRelative;
		set => _xRelative = value is >= 0 and <= 1
			? value
			: throw new ArgumentException($"xRelative not within the range of [0, 1] ({value})");
	}
	
	private double _yRelative = 0.5;

	/// <summary>
	/// Gets or sets the relative position of thumb along the y-axis.
	/// Takes values from 0 to 1
	/// </summary>
	/// <exception cref="ArgumentException">The passed value was less than 0 or more than 1</exception>
	private double YRelative
	{
		get => _yRelative;
		set => _yRelative = value is >= 0 and <= 1
			? value
			: throw new ArgumentException($"yRelative not within the range of [0, 1] ({value})");
	}

	/// <summary>
	/// Gets or sets the size of the control body
	/// </summary>
	private double FrameSize
	{
		get => Frame.Width;
		set => Frame.Width = Frame.Height = value;
	}

	/// <summary>
	/// Gets or sets the size of the area available for moving of the thumb
	/// </summary>
	private double MovementAreaSize => FrameSize - ThumbSize;

	private double _left;
	/// <summary>
	/// Gets or sets the absolute horizontal position of the thumb
	/// </summary>
	private double Left
	{
		get => _left;
		set => Canvas.SetLeft(Stick, _left = value);
	}

	private double _top;
	
	/// <summary>
	/// Gets or sets the absolute horizontal position of the thumb
	/// </summary>
	private double Top
	{
		get => _top;
		set => Canvas.SetTop(Stick, _top = value);
	}
	
	private static Type JoystickType => typeof(Joystick);
	#endregion

	#region Constructors
	public Joystick()
	{
		InitializeComponent();
		Stick.Height = ThumbSize;
		Stick.Width = ThumbSize;
	}
	#endregion

	#region Custom Events
	private static readonly RoutedEvent JoystickMovedEvent = EventManager.RegisterRoutedEvent(
		name: "JoystickMoved",
		routingStrategy: RoutingStrategy.Bubble,
		handlerType: typeof(JoystickMovedEventHandler),
		ownerType: JoystickType);
	
	/// <summary>
	/// Occurs when the thumb of the joystick is moved
	/// </summary>
	public event JoystickMovedEventHandler JoystickMoved
	{
		add => AddHandler(JoystickMovedEvent, value);
		remove => RemoveHandler(JoystickMovedEvent, value);
	}
	#endregion
	
	#region Event Handlers
	/// <summary>
	/// Sets the new coordinates of the thumb based on the movement, raises the <c>JoystickMoved</c> event 
	/// </summary>
	private void Thumb_DragDelta(object sender, DragDeltaEventArgs e)
	{
		Left = GetValidCoordinate(Left + e.HorizontalChange);
		Top = GetValidCoordinate(Top + e.VerticalChange);

		XRelative = Left / MovementAreaSize;
		YRelative = Top / MovementAreaSize;

		RaiseEvent(new JoystickMovedEventArgs(JoystickMovedEvent, this) { NewX = X, NewY = Y });
	}
	
	/// <summary>
	/// Sets new size and updates coordinates based on the initial relative coordinates
	/// </summary>
	private void Control_SizeChanged(object sender, SizeChangedEventArgs e)
	{
		FrameSize = GetMinSize(e.NewSize);
		UpdateCoordinates();
	}
	#endregion
	
	#region Other methods
	/// <summary>
	/// Finds the corresponding valid coordinate within the acceptable movement area
	/// </summary>
	/// <param name="value">The initial coordinate</param>
	/// <returns>A valid coordinate within the movement area</returns>
	private double GetValidCoordinate(double value)
	{
		return Util.MinMax(0, value, MovementAreaSize);
	}

	/// <summary>
	/// Updates the absolute thumb coordinates from the relative ones
	/// </summary>
	private void UpdateCoordinates()
	{
		Left = GetFromRelative(XRelative);
		Top = GetFromRelative(YRelative);
	}
	
	/// <summary>
	/// Finds the minimum side length from size object
	/// </summary>
	/// <param name="size">The size of an object</param>
	/// <returns>The minimum side length</returns>
	private static double GetMinSize(Size size)
	{
		var width = size.Width;
		var height = size.Height;

		return Math.Min(width, height);
	}

	/// <summary>
	/// Finds the absolute coordinate value from the relative position
	/// </summary>
	/// <param name="coordinate">The relative position coordinate</param>
	/// <returns>The absolute position coordinate</returns>
	private double GetFromRelative(double coordinate)
	{
		return MovementAreaSize * coordinate;
	}
	#endregion
}
