using System.Windows;
using System.Windows.Controls;
using flightgear_interface.Components;
using flightgear_interface.Connection;

namespace flightgear_interface
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow
	{
		#region Properties
		private FgClient Client { get; }
		#endregion

		#region Constructors
		public MainWindow()
		{
			InitializeComponent();
			Style = (Style)FindResource(typeof(Window));
			Client = new FgClient();
		}
		#endregion

		#region Event handlers
		private void slider_ValueChange(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			var sliderName = ((Slider)sender).Name!;
			var sliderValue = e.NewValue;

			TryUpdateClient(sliderName, sliderValue);

			TryUpdateJoystick(sliderName, sliderValue);
		}

		private void Joystick_JoystickMoved(object sender, JoystickMovedEventArgs e)
		{
			Aileron.Value = e.NewX;
			Elevator.Value = e.NewY;
		}

		private void Reset_Click(object sender, RoutedEventArgs e)
		{
			Elevator.Value = 0;
			Aileron.Value = 0;
			Throttle.Value = 0.2;
			Rudder.Value = 0;
		}
		#endregion
		
		#region Other methods
		private void TryUpdateClient(string sliderName, double sliderValue)
		{
			if ((FgClient?)Client is null) return;
			
			Client.Set(sliderName, sliderValue);
		}

		private void TryUpdateJoystick(string sliderName, double sliderValue)
		{
			if (Stick is null) return;

			if (sliderName == "Aileron")
				Stick.X = sliderValue;
			else if (sliderName == "Elevator")
				Stick.Y = sliderValue;
		}
		#endregion
	}
}