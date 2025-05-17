using Syncfusion.Maui.Buttons;
using Syncfusion.Maui.Toolkit.Chips;

namespace VACalculatorApp;

public partial class MainPage : ContentPage
{
    private readonly PercentagesViewModel _viewModel;
    private bool _isMarried;
    private int _parents;
    private int _childrenUnder18;
    private int _childrenOver18;
    
    public MainPage()
    {
        InitializeComponent();
        _viewModel = BindingContext as PercentagesViewModel ?? new PercentagesViewModel();
        
        ParentsPicker.SelectedIndex = 0;
        ChildrenUnder18Picker.SelectedIndex = 0;
        ChildrenOver18Picker.SelectedIndex = 0;
        
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

    private void ClearPercentages_Clicked(object sender, EventArgs e)
    {
        _viewModel.ClearPercentages();
        EmptyPercentagesLabel.IsVisible = true;
        UpdateCalculation();
    }

    private void MarriedSwitch_Toggled(object sender, ToggledEventArgs toggledEventArgs)
    {
        _isMarried = toggledEventArgs.Value;
        UpdateCalculation();
    }

    private void DependentPicker_Changed(object sender, EventArgs e)
    {
        // Update values from all pickers
        if (ParentsPicker.SelectedIndex >= 0)
        {
            _parents = int.Parse(ParentsPicker.SelectedItem.ToString());
        }

        if (ChildrenUnder18Picker.SelectedIndex >= 0)
        {
            _childrenUnder18 = int.Parse(ChildrenUnder18Picker.SelectedItem.ToString());
        }

        if (ChildrenOver18Picker.SelectedIndex >= 0)
        {
            _childrenOver18 = int.Parse(ChildrenOver18Picker.SelectedItem.ToString());
        }
        
        UpdateCalculation();
    }
    
    private void ClearDependents_Clicked(object sender, EventArgs e)
    {
        // Reset dependent information
        MarriedSwitch.IsToggled = false;
        ParentsPicker.SelectedIndex = 0;
        ChildrenUnder18Picker.SelectedIndex = 0;
        ChildrenOver18Picker.SelectedIndex = 0;
    
        // Update internal values
        _isMarried = false;
        _parents = 0;
        _childrenUnder18 = 0;
        _childrenOver18 = 0;
    
        UpdateCalculation();
    }

    private void UpdateCalculation()
    {
        
        if (_viewModel.Percents.Count > 0)
        {
            // Calculate combined rating
            List<int> percentages = _viewModel.Percents.Select(percentItem => percentItem.DisabilityPercentage).ToList();
            int combinedRating = CalculateRate.CombineDisabilityRatings(percentages);

            int childrenBasic = _childrenUnder18 > 0 || _childrenOver18 > 0 ? 1 : 0;

            // Calculate additional children (beyond the first one)
            int additionalChildrenUnder18 = childrenBasic > 0 ? _childrenUnder18 - 1 : 0;
            if (additionalChildrenUnder18 < 0) additionalChildrenUnder18 = 0;

            
            var veteran = new Veteran
            {
                DisabilityPercentage = combinedRating,
                IsMarried = _isMarried,
                ParentCount = _parents,
                ChildrenUnder18Count = childrenBasic,
                ChildrenOver18InSchoolCount = _childrenOver18,
                AdditionalChildrenUnder18Count = additionalChildrenUnder18,
                SpouseReceivingAidAndAttendance = false 
            };

            float compensation = CalculateRate.CalculateTotalCompensation(veteran);

            // Update UI
            CombinedRatingLabel.Text = $"{combinedRating}%";

            if (compensation >= 0)
                CompensationLabel.Text = compensation.ToString("C");
            else
                CompensationLabel.Text = "Rate not available";
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
        if (sender is SfChip chip && chip.BindingContext is Percents percent)
        {
            _viewModel.RemovePercentage(percent);
            EmptyPercentagesLabel.IsVisible = _viewModel.Percents.Count == 0;
            UpdateCalculation();
        }
    }

    private void Infobutton_OnClicked(object? sender, EventArgs e)
    {
        VAMathPopup.Show();
    }

    private async void GoToKnightTour_OnClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("knighttour");
    }
}