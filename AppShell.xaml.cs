namespace VACalculatorApp;

public partial class AppShell : Shell
{
    //TODO navigate back to main page
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute("knighttour", typeof(KnightTourPage));

    }
}