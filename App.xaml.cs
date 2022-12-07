using System.Globalization;
using System.Windows;

namespace flightgear_interface
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		private const bool ShowDebug = false;
		
		public App()
		{
			CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
			
			Properties["Debug"] = ShowDebug ? new DebugConsole() : new EmptyDebug();
		}
	}
}