using System.Collections.ObjectModel;

namespace VACalculatorApp;

public partial class MainPage : ContentPage
{
    private ObservableCollection<int> _selectedPercentages = new ObservableCollection<int>();
    private bool _isMarried = false;
    private int _parents = 0;
    private int _childrenUnder18 = 0;
    private int _childrenOver18 = 0;
    
    public MainPage()
    {
        InitializeComponent();
        
        // Set initial picker values
        ParentsPicker.SelectedIndex = 0;
        ChildrenUnder18Picker.SelectedIndex = 0;
        ChildrenOver18Picker.SelectedIndex = 0;
        
        // Set the collection view source
        SelectedPercentagesView.ItemsSource = _selectedPercentages;
        
        // Update the UI initially
        UpdateCalculation();
    }

    private void PercentageButton_Clicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is string percentageStr)
        {
            if (int.TryParse(percentageStr, out int percentage))
            {
                // Add the percentage to our list
                _selectedPercentages.Add(percentage);
                
                // Update the calculation
                UpdateCalculation();
            }
        }
    }

    private void RemovePercentage_Clicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is int percentage)
        {
            _selectedPercentages.Remove(percentage);
            UpdateCalculation();
        }
    }

    private void ClearPercentages_Clicked(object sender, EventArgs e)
    {
        _selectedPercentages.Clear();
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

    private void UpdateCalculation()
    {
        if (_selectedPercentages.Count > 0)
        {
            // Calculate combined rating
            List<int> percentages = _selectedPercentages.ToList();
            int combinedRating = CalculateRate.CombineDisabilityRatings(percentages);
            
            // Calculate total compensation - note we use _childrenUnder18 and _childrenOver18 separately
            // First determine the basic dependent situation (has children or not)
            int childrenBasic = _childrenUnder18 > 0 || _childrenOver18 > 0 ? 1 : 0;
            
            // Calculate additional children (beyond the first one)
            int additionalChildrenUnder18 = childrenBasic > 0 ? _childrenUnder18 - 1 : 0;
            if (additionalChildrenUnder18 < 0) additionalChildrenUnder18 = 0;
            
            float compensation = CalculateRate.CalculateTotalCompensation(
                combinedRating, 
                _isMarried, 
                _parents, 
                childrenBasic, 
                _childrenOver18,
                additionalChildrenUnder18);
            
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