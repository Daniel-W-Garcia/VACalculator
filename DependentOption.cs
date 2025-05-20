namespace VACalculatorApp;

public class DependentOption : BindableObject
{
    private int _value;

    public int Value
    {
        get => _value; 
        set
        {
            _value = value;
            OnPropertyChanged();
        }
    }
    
    private string _display;
    public string Display 
    { 
        get => _display;
        set
        {
            _display = value;
            OnPropertyChanged();
        }
    }
    
    public override string ToString() => Display;
}