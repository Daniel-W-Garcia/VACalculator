using Syncfusion.Maui.Toolkit.Chips;

namespace VACalculatorApp;

public partial class MainPage : ContentPage
{
    private CalculationViewModel _viewModel;
    public MainPage()
    {
        InitializeComponent();
        
        _viewModel = new CalculationViewModel();
        BindingContext = _viewModel;
    }
    
    private void SfChip_OnCloseButtonClicked(object? sender, EventArgs e)
    {
        if (sender is SfChip chip && chip.BindingContext is Percentages percent)
        {
            _viewModel.RemovePercentage(percent);
        }
    }

    private void InfoButton_OnClicked(object? sender, EventArgs e)
    {
        VaMathPopup.Show();
    }

    private async void GoToKnightTour_OnClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("knighttour");
    }

}