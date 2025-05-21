using Syncfusion.Maui.Buttons;
using Syncfusion.Maui.Toolkit.Chips;

namespace VACalculatorApp;

public partial class MainPage : ContentPage
{
    private CalculationViewModel _viewModel;
    private bool _isMarried;
    private int _parents;
    private int _childrenUnder18;
    private int _childrenOver18;
    
    public MainPage()
    {
        InitializeComponent();
        
        _viewModel = new CalculationViewModel();
        BindingContext = _viewModel;
    
    
        EmptyPercentagesLabel.IsVisible = true;
        UpdateCalculation();
    }

    private void PercentageButton_Clicked(object? sender, EventArgs e)
    {
        if (sender is SfButton percentageButton && percentageButton.CommandParameter is string percentageStr)
        {
            if (int.TryParse(percentageStr, out int percentage))
            {
                _viewModel.AddPercentage(percentage);
                EmptyPercentagesLabel.IsVisible = _viewModel.Percents.Count == 0;
                UpdateCalculation();
            }
        }
    }
    private void MarriedSwitch_OnStateChanged(object? sender, SwitchStateChangedEventArgs e)
    {
        UpdateCalculation();
    }

    private void ClearPercentages_Clicked(object sender, EventArgs e)
    {
        _viewModel.ClearPercentages();
        EmptyPercentagesLabel.IsVisible = true;
        UpdateCalculation();
    }

    private void DependentPicker_Changed(object sender, EventArgs e)
    {
        UpdateCalculation();
    }
    
    private void ClearDependents_Clicked(object sender, EventArgs e)
    {
        // Reset dependent information
        _viewModel.IsMarried = false;
        
        _viewModel.SelectedNumberOfParents = _viewModel.ParentCount?.FirstOrDefault(); //App was crashing for null ref exceptions before adding a null check here
        _viewModel.SelectedNumberOfChildUnder18 = _viewModel.ChildUnder18Count?.FirstOrDefault(); //Assigning a value to allow viewmodel to instantiate with a default value
        _viewModel.SelectedNumberOfChildrenOver18InSchool = _viewModel.ChildOver18InSchoolCount?.FirstOrDefault(); 
    
        UpdateCalculation();
    }

    private void UpdateCalculation()
    {
        
        if (_viewModel.Percents.Count > 0)
        {
            // Calculate combined rating
            List<int> percentages = _viewModel.Percents.Select(p => p.Value).ToList();
            int combinedRating = CalculateRate.CombineDisabilityRatings(percentages);

            int chilrenUnder18 = _viewModel.SelectedNumberOfChildUnder18?.Value ?? 0;
            int childrenOver18 = _viewModel.SelectedNumberOfChildrenOver18InSchool?.Value ?? 0;


            int childrenBasic = chilrenUnder18 > 0 || childrenOver18 > 0 ? 1 : 0;

            // Calculate additional children (beyond the first one)
            int additionalChildrenUnder18 = childrenBasic > 0 ? chilrenUnder18 - 1 : 0;
            if (additionalChildrenUnder18 < 0) additionalChildrenUnder18 = 0;

            
            var veteran = new Veteran
            {
                DisabilityPercentage = combinedRating,
                IsMarried = _viewModel.IsMarried,
                ParentCount = _viewModel.SelectedNumberOfParents?.Value ?? 0,
                ChildrenUnder18Count = childrenBasic,
                ChildrenOver18InSchoolCount = childrenOver18,
                AdditionalChildrenUnder18Count = additionalChildrenUnder18,
                SpouseReceivingAidAndAttendance = false 
            };

            float compensation = CalculateRate.CalculateTotalCompensation(veteran);

            // Update UI
            CombinedRatingLabel.Text = $"{combinedRating}%";

            if (compensation >= 0)
            {
                CompensationLabel.Text = compensation.ToString("C");
            }
            else
            {
                CompensationLabel.Text = "Rate not available";
            }
        }
        else
        {
            // No percentages selected
            CombinedRatingLabel.Text = "--%";
            CompensationLabel.Text = "$0.00";
        }
    }

    private void SfChip_OnCloseButtonClicked(object? sender, EventArgs e)
    {
        if (sender is SfChip chip && chip.BindingContext is Percentages percent)
        {
            _viewModel.RemovePercentage(percent);
            EmptyPercentagesLabel.IsVisible = _viewModel.Percents.Count == 0;
            UpdateCalculation();
        }
    }

    private void Infobutton_OnClicked(object? sender, EventArgs e)
    {
        VaMathPopup.Show();
    }

    private async void GoToKnightTour_OnClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("knighttour");
    }

}