using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace VACalculatorApp;

public class CalculationViewModel : INotifyPropertyChanged
{
    public ICommand RemovePercentCommand { get; set; }
    public ICommand AddPercentCommand { get; set; }
    public ICommand ClearPercentagesCommand { get; set; }
    public ICommand ClearDependentsCommand { get; set; }
    public event PropertyChangedEventHandler? PropertyChanged;
    
    public ObservableCollection<DependentOption> ParentCount { get; } = new();
    public ObservableCollection<DependentOption> ChildUnder18Count { get; } = new();
    public ObservableCollection<DependentOption> ChildOver18InSchoolCount { get; } = new();
    
    private ObservableCollection<Percentages> _percents = new();
    public bool HasNoPercentages => Percents == null || Percents.Count == 0;


    public ObservableCollection<Percentages> Percents
    {
        get => _percents;
        set
        {
            _percents = value;
            OnPropertyChanged("Percents");
            UpdateCalculation();
        }
    }
    
    private DependentOption _selectedNumberOfParents;

    public DependentOption SelectedNumberOfParents
    {
        get => _selectedNumberOfParents;
        set
        {
            _selectedNumberOfParents = value;
            OnPropertyChanged("SelectedNumberOfParents");
            UpdateCalculation();
        }
    }
    private DependentOption _selectedNumberOfChildUnder18;
    public DependentOption SelectedNumberOfChildUnder18
    {
        get => _selectedNumberOfChildUnder18;
        set
        {
            _selectedNumberOfChildUnder18 = value;
            OnPropertyChanged("SelectedNumberOfChildUnder18");
            UpdateCalculation();
        }
    }
    
    private DependentOption _selectedNumberOfChildrenOver18InSchool;
    public DependentOption SelectedNumberOfChildrenOver18InSchool
    {
        get => _selectedNumberOfChildrenOver18InSchool;
        set
        {
            _selectedNumberOfChildrenOver18InSchool = value;
            OnPropertyChanged("SelectedNumberOfChildrenOver18InSchool");
            UpdateCalculation();
        }
    }

    private int? _combinedRating; //nullable IOT use HasValue in text output of CombinedRatingText
    public int? CombinedRating
    {
        get => _combinedRating;
        set
        {
            _combinedRating = value;
            OnPropertyChanged("CombinedRating");
            OnPropertyChanged("CombinedRatingText");
        }
    }
    public string CombinedRatingText => CombinedRating.HasValue ? $"{CombinedRating}%" : "--%";

    private float _compensation;
    public float Compensation
    {
        get => _compensation;
        set
        {
            _compensation = value;
            OnPropertyChanged("Compensation");
            OnPropertyChanged("CompensationText");
        }
    }
    public string CompensationText => Compensation >= 0 ? Compensation.ToString("C") : "Rate not available";

    private bool _isMarried;
    public bool IsMarried
    {
        get => _isMarried;
        set
        {
            _isMarried = value;
            OnPropertyChanged("IsMarried");
            UpdateCalculation();
        }
    }

    private int _result;
    public int Result
    {
        get => _result;
        set
        {
            _result = value; OnPropertyChanged("Result");
        }
    }

    public CalculationViewModel()
    {
        // Initialize parent options (0-2)
        for (int i = 0; i <= 2; i++)
        {
            ParentCount.Add(new DependentOption { Value = i, Display = i.ToString() });
        }
        
        // Initialize selection options for children (0-10)
        for (int i = 0; i <= 10; i++)
        {
            ChildUnder18Count.Add(new DependentOption { Value = i, Display = i.ToString() });
            ChildOver18InSchoolCount.Add(new DependentOption { Value = i, Display = i.ToString() });
        }
        
        // Set defaults
        SelectedNumberOfParents = ParentCount.FirstOrDefault();
        SelectedNumberOfChildUnder18 = ChildUnder18Count.FirstOrDefault();
        SelectedNumberOfChildrenOver18InSchool = ChildOver18InSchoolCount.FirstOrDefault();
        IsMarried = false;
        
        
        AddPercentCommand = new Command<string>(AddPercentageFromString);
        RemovePercentCommand = new Command<Percentages>(RemovePercentage);
        ClearPercentagesCommand = new Command(ClearPercentages);
        ClearDependentsCommand = new Command(ClearDependents);
        Percents = new ObservableCollection<Percentages>();

    }
    private void AddPercentageFromString(string percentStr)
    {
        if (int.TryParse(percentStr, out int percentage))
        {
            AddPercentage(percentage);
        }
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private void AddPercentage(int percentage)
    {
        var newPercentage = new Percentages();
        newPercentage.Value = percentage;
        Percents.Add(newPercentage);
        OnPropertyChanged(nameof(Percents));
        OnPropertyChanged(nameof(HasNoPercentages));
        UpdateCalculation();
    }

    public void RemovePercentage(Percentages percentToRemove)
    {
        Percents.Remove(percentToRemove);
        OnPropertyChanged(nameof(Percents));
        OnPropertyChanged(nameof(HasNoPercentages)); 
        UpdateCalculation(); 
    }

    private void ClearPercentages()
    {
        Percents.Clear();
        OnPropertyChanged(nameof(Percents));
        OnPropertyChanged(nameof(HasNoPercentages));
        UpdateCalculation();
    }

    private void ClearDependents()
    {
        IsMarried = false;
        SelectedNumberOfParents = ParentCount?.FirstOrDefault();
        SelectedNumberOfChildUnder18 = ChildUnder18Count?.FirstOrDefault();
        SelectedNumberOfChildrenOver18InSchool = ChildOver18InSchoolCount?.FirstOrDefault();
    }
    public void UpdateCalculation()
    {
        if (Percents.Count > 0)
        {
            List<int> percentages = Percents.Select(p => p.Value).ToList();
            int combinedRating = CalculateRate.CombineDisabilityRatings(percentages);

            int childrenUnder18 = SelectedNumberOfChildUnder18?.Value ?? 0;
            int childrenOver18 = SelectedNumberOfChildrenOver18InSchool?.Value ?? 0;

            int childrenBasic = childrenUnder18 > 0 || childrenOver18 > 0 ? 1 : 0;
            int additionalChildrenUnder18 = childrenBasic > 0 ? childrenUnder18 - 1 : 0;
            if (additionalChildrenUnder18 < 0) additionalChildrenUnder18 = 0;

            var veteran = new Veteran
            {
                DisabilityPercentage = combinedRating,
                IsMarried = IsMarried,
                ParentCount = SelectedNumberOfParents?.Value ?? 0,
                ChildrenUnder18Count = childrenBasic,
                ChildrenOver18InSchoolCount = childrenOver18,
                AdditionalChildrenUnder18Count = additionalChildrenUnder18,
                SpouseReceivingAidAndAttendance = false
            };

            CombinedRating = combinedRating;
            Compensation = CalculateRate.CalculateTotalCompensation(veteran);
        }
        else
        {
            CombinedRating = null;
            Compensation = 0;
        }
    }
}