using System.Collections.ObjectModel;

namespace VACalculatorApp;

public class GameBoardConfiguration
{
    private ObservableCollection<string> _boardSizes = new();
    public ObservableCollection<string> BoardSizes
    {
        get => _boardSizes;
        set => _boardSizes = value;
    }
    
    public GameBoardConfiguration()
    {
        BoardSizes = new ObservableCollection<string>
        {
            "4 x 4", "5 x 5", "6 x 6", "7 x 7", "8 x 8"
        };
    }
}