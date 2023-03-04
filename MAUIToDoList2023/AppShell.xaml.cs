using MAUIToDoList2023.Views;

namespace MAUIToDoList2023;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		
		Routing.RegisterRoute(nameof(AddPage), typeof(AddPage));
	}
}
