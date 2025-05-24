using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace VACalculatorApp
{
    public class Percentages : INotifyPropertyChanged
    {
        private int _value;
        private string _display = "0%";

        public int Value
        {
            get => _value;
            set
            {
                if (_value != value)
                {
                    _value = value;
                    Display = $"{_value}%";
                    OnPropertyChanged("Value");
                }
            }
        }

        public string Display
        {
            get => _display;
            set
            {
                if (_display != value)
                {
                    _display = value;
                    OnPropertyChanged("Display");
                }
            }
        }

        public override string ToString() => Display;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}