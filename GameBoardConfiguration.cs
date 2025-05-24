using System.Collections.ObjectModel;

namespace VACalculatorApp;

public static class GameBoardConfiguration
{
    public static ObservableCollection<string> BoardSizes { get; } = new ObservableCollection<string>
    {
        "5 x 5", "6 x 6", "7 x 7", "8 x 8"
    };
}
