using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using flightgear_interface.Connection;

namespace flightgear_interface
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private IDebug debug;
		private FGData fgData;
		private FgClient client;
		
		private const bool showDebug = true;
		
		public MainWindow()
		{
			InitializeComponent();
			Style = (Style)FindResource(typeof(Window));

			debug = showDebug ? new DebugConsole() : new EmptyDebug();
			fgData = new FGData();
			client = new FgClient(fgData, debug);
		}

		private void slider_ValueChange(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			var slider = sender as Slider;
			fgData.Set(slider.Name, e.NewValue);
			client.Send();
		}
	}
}