namespace VACalculatorApp;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute("knighttour", typeof(KnightTourPage));

    }
}