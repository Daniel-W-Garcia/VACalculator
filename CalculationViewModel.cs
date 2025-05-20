using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace VACalculatorApp;

public class CalculationViewModel : INotifyPropertyChanged
{
    public ICommand RemovePercentCommand { get; set; }
    public ICommand AddPercentCommand { get; set; }
    public event PropertyChangedEventHandler? PropertyChanged;
    
    public ObservableCollection<DependentOption> ParentCount { get; } = new();
    public ObservableCollection<DependentOption> ChildUnder18Count { get; } = new();
    public ObservableCollection<DependentOption> ChildOver18InSchoolCount { get; } = new();
    
    
    private ObservableCollection<Percentages> _percents = new();

    public ObservableCollection<Percentages> Percents
    {
        get => _percents;
        set
        {
            _percents = value;
            OnPropertyChanged("Percents");
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
        }
    }

    private bool _isMarried;
    public bool IsMarried
    {
        get => _isMarried;
        set
        {
            _isMarried = value;
            OnPropertyChanged("IsMarried");
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
        
        
        AddPercentCommand = new Command(HandleAction);
        RemovePercentCommand = new Command<Percentages>(RemovePercentage);
        Percents = new ObservableCollection<Percentages>();

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


    public void HandleAction(object obj)
    {
        if (obj is string percentStr && int.TryParse(percentStr.Replace("%", ""), out int percentage))
        {
            Result = percentage;
        }
    }

    public void AddPercentage(int percentage)
    {
        var newPercentage = new Percentages();
        newPercentage.Value = percentage;
        Percents.Add(newPercentage);
        OnPropertyChanged(nameof(Percents));
    
    }

    public void RemovePercentage(Percentages percentToRemove)
    {
        Percents.Remove(percentToRemove);
        OnPropertyChanged(nameof(Percents));
    }

    public void ClearPercentages()
    {
        Percents.Clear();
        OnPropertyChanged(nameof(Percents));
    }
}