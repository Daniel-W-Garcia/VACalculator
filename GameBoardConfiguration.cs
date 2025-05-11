using System.Collections.ObjectModel;

namespace VACalculatorApp;

public class GameBoardConfiguration
{
    public ObservableCollection<string> BoardSizes { get; set; } =
    [
        "4 x 4",
        "5 x 5",
        "6 x 6",
        "7 x 7",
        "8 x 8"
    ];
}