using System.Reflection;
using System.Text.Json;
using Syncfusion.Licensing;

namespace VACalculatorApp;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        string licenseKey = LoadLicenseKey();
        SyncfusionLicenseProvider.RegisterLicense(licenseKey);
        MainPage = new AppShell();
    }
    private string LoadLicenseKey()
    {
        try
        {
            var assembly = Assembly.GetExecutingAssembly();
            using var stream = assembly.GetManifestResourceStream("VACalculatorApp.secrets.json");
            using var reader = new StreamReader(stream);
            var json = reader.ReadToEnd();
        
            var options = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
            return options["SyncfusionLicense"];
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading license: {ex.Message}");
            return null;
        }
    }
}