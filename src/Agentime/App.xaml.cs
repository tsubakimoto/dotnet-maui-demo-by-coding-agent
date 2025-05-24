namespace dev.tsubakimoto.Agentime;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		MainPage = new AppShell();
		
		// Set default theme to dark
		UserAppTheme = AppTheme.Dark;
	}
}
