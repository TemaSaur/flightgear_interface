namespace flightgear_interface;


public static class Debug
{
	public static bool ShowDebug
	{
		set => DebugInterface = value ? new DebugConsole() : new EmptyDebug();
	}

	private static IDebug? DebugInterface { get; set; }

	public static void WriteLine(string any)
	{
		DebugInterface!.WriteLine(any);
	}
}
