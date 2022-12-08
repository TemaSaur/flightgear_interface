using System.Globalization;
using System.Windows;

namespace flightgear_interface
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App
	{
		public App()
		{
			CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
			Debug.ShowDebug = true;
		}
	}
}