using System.Collections.ObjectModel;
using Syncfusion.Maui.Toolkit.Themes;

namespace VACalculatorApp;

public partial class MainPage : ContentPage
{
    private ObservableCollection<int> _selectedPercentages = new (); //super cool collection type that updates in real time when changes are made using an event
    private bool _isMarried;
    private int _parents;
    private int _childrenUnder18;
    private int _childrenOver18;
    
    public MainPage()
    {
        InitializeComponent();
        
        ParentsPicker.SelectedIndex = 0;
        ChildrenUnder18Picker.SelectedIndex = 0;
        ChildrenOver18Picker.SelectedIndex = 0;
        UpdateSelectedPercentagesDisplay();
        
        UpdateCalculation();
    }
    private async void NavigateToKnightTour_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//knighttour");
    }
    private void UpdateSelectedPercentagesDisplay()
    {
        SelectedPercentagesContainer.Children.Clear();
        EmptyPercentagesLabel.IsVisible = _selectedPercentages.Count == 0;
    
        foreach (var percentage in _selectedPercentages)
        {
            var percentFrame = new Frame
            {
                BackgroundColor = Color.FromArgb("#E6F2FF"),
                Padding = new Thickness(10, 5),
                CornerRadius = 5,
                BorderColor = Color.FromArgb("#0078D7"),
                Margin = new Thickness(0, 0, 8, 8)
            };
        
            var layout = new HorizontalStackLayout();
        
            var labelPercent = new Label
            {
                Text = $"{percentage}%",
                FontSize = 16,
                TextColor = Colors.Black,
                FontAttributes = FontAttributes.Bold,
                VerticalOptions = LayoutOptions.Center
            };
        
            var percentSelectedButton = new Button
            {
                Text = "×",
                FontSize = 14,
                FontAttributes = FontAttributes.Bold,
                BackgroundColor = Color.FromArgb("#FF5252"),
                TextColor = Colors.White,
                Margin = new Thickness(5, 0, 0, 0),
                HeightRequest = 20,
                WidthRequest = 20,
                CornerRadius = 10,
                Padding = new Thickness(0),  
                LineBreakMode = LineBreakMode.NoWrap,  
                VerticalOptions = LayoutOptions.Center, 
                HorizontalOptions = LayoutOptions.Center, 
                CommandParameter = percentage

            };
            percentSelectedButton.Clicked += RemovePercentage_Clicked;
        
            layout.Children.Add(labelPercent);
            layout.Children.Add(percentSelectedButton);
            percentFrame.Content = layout;
        
            SelectedPercentagesContainer.Children.Add(percentFrame);
        }
    }


    private void PercentageButton_Clicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is string percentageStr)
        {
            if (int.TryParse(percentageStr, out int percentage))
            {
                _selectedPercentages.Add(percentage);
                UpdateSelectedPercentagesDisplay();
                UpdateCalculation();
            }
        }
    }

    private void RemovePercentage_Clicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is int percentage)
        {
            _selectedPercentages.Remove(percentage);
            UpdateSelectedPercentagesDisplay();
            UpdateCalculation();
        }
    }

    private void ClearPercentages_Clicked(object sender, EventArgs e)
    {
        _selectedPercentages.Clear();
        UpdateSelectedPercentagesDisplay();
        UpdateCalculation();
    }

    private void MarriedSwitch_Toggled(object sender, ToggledEventArgs e)
    {
        _isMarried = e.Value;
        UpdateCalculation();
    }

    private void DependentPicker_Changed(object sender, EventArgs e)
    {
        // Update values from all pickers
        if (ParentsPicker.SelectedIndex >= 0)
            _parents = int.Parse(ParentsPicker.SelectedItem.ToString());
            
        if (ChildrenUnder18Picker.SelectedIndex >= 0)
            _childrenUnder18 = int.Parse(ChildrenUnder18Picker.SelectedItem.ToString());
            
        if (ChildrenOver18Picker.SelectedIndex >= 0)
            _childrenOver18 = int.Parse(ChildrenOver18Picker.SelectedItem.ToString());
        
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
        if (_selectedPercentages.Count > 0)
        {
            // Calculate combined rating
            List<int> percentages = _selectedPercentages.ToList();
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
}