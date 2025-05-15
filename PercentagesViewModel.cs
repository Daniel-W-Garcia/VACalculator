using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace VACalculatorApp;

public class PercentagesViewModel : INotifyPropertyChanged
{
    private ObservableCollection<Percents> _percents = new();
    private int _percent;
    private int _result;
    
    public ICommand AddPercentCommand { get; set; }
    public ICommand RemovePercentCommand { get; set; }


    

    public int Result
    {
        get => _result;
        set
        {
            _result = value; OnPropertyChanged("Result");
        }
    }


    public ObservableCollection<Percents> Percents
    {
        get => _percents;
        set
        {
            _percents = value; OnPropertyChanged("Percents");
        }
    }

    public PercentagesViewModel()
    {
        AddPercentCommand = new Command(HandleAction);
        RemovePercentCommand = new Command<Percents>(RemovePercentage);
        Percents = new ObservableCollection<Percents>();
    }
    
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    public void HandleAction(object obj)
    {
        Result = (obj as Percents)?.DisabilityPercentage ?? 0;
    }
    public void AddPercentage(int percentage)
    {
        Percents.Add(new Percents { DisabilityPercentage = percentage });
        OnPropertyChanged(nameof(Percents));
    }

    public void RemovePercentage(Percents percent)
    {
        Percents.Remove(percent);
        OnPropertyChanged(nameof(Percents));
    }

    public void ClearPercentages()
    {
        Percents.Clear();
        OnPropertyChanged(nameof(Percents));
    }

}

public class Percents
{
    public int DisabilityPercentage { get; set; }
    public string Display => $"{DisabilityPercentage}%";
}