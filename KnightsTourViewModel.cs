using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace VACalculatorApp;

public class KnightsTourViewModel : INotifyPropertyChanged
{

    private KnightsTourGame _game;
    
    public event PropertyChangedEventHandler? PropertyChanged;
    public event EventHandler BoardUpdated;
    public event EventHandler RemoveOldDotsRequested;
    
    public ICommand RestartCommand { get;}
    public ICommand NavigateBackCommand { get;}

    private int _currentBoardSize;
    public int CurrentBoardSize
    {
        get => _currentBoardSize;
        set
        {
            if (SetField(ref _currentBoardSize, value))
            {
                RecreateGame();
            }
        }
    }
    private string _statusChange;

    public string StatusChange
    {
        get => _statusChange;
        set => SetField(ref _statusChange, value);
    }
    
    private string _winMessage;
    public string WinMessage
    {
        get => _winMessage;
        set => SetField(ref _winMessage, value);
    }
    
    private bool _gameWon;

    public bool GameWon
    {
        get => _gameWon;
        set => SetField(ref _gameWon, value);
    }
    
    public int CurrentX => _game?.CurrentX ?? -1;
    public int CurrentY => _game?.CurrentY ?? -1;
    public int MovesMade => _game?.MovesMade ?? -1;
    public bool WinConditionMet => _game?.WinConditionMet ?? false;

    public KnightsTourViewModel()
    {
        _currentBoardSize = 8;
        _game = new KnightsTourGame(_currentBoardSize);
        RestartCommand = new Command(RestartGame);
        NavigateBackCommand = new Command(async () => await NavigateBack());
    }

    public bool AttemptMove(int row, int col)
    {
        bool moveSuccessful = _game.Move(row, col);
        
        if (moveSuccessful)
        {
            StatusChange = $"Moved to Row, {row + 1}, Column {col + 1}";
            OnPropertyChanged(nameof(CurrentX));
            OnPropertyChanged(nameof(CurrentY));
            OnPropertyChanged(nameof(MovesMade));
            OnPropertyChanged(nameof(WinConditionMet));
            CheckGameEnd(row, col);
        }
        else
        {
            StatusChange = $"Invalid Move. Cannot move to {row}, {col}";
        }
        return moveSuccessful;
    }

    private void CheckGameEnd(int row, int col)
    {
        if (_game.GetLegalMoves(row, col).Count == 0)
        {
            if (!_game.WinConditionMet)
            {
                StatusChange = "No legal moves. Game Over!\nPress Restart to try again.";
                GameWon = false;
            }
            else
            {
                StatusChange = "CONGRATULATIONS! You have completed the Knight's Tour!\nPress Restart to try again.";
                GameWon = true;
            }
        }
    }

    public List<(int, int)> GetLegalMoves(int row, int col)
    {
        return _game.GetLegalMoves(row, col);
    }
    
    public bool[,] GetBoard() => _game.GetBoard();

    private void RestartGame()
    {
        _game.Reset();
        
        StatusChange = "Game restarted. Select your starting position.";
        WinMessage = "";
        GameWon = false;
        OnPropertyChanged(nameof(CurrentX));
        OnPropertyChanged(nameof(CurrentY));
        OnPropertyChanged(nameof(MovesMade));
        OnPropertyChanged(nameof(WinConditionMet));
        
        BoardUpdated?.Invoke(this, EventArgs.Empty);
        RemoveOldDotsRequested?.Invoke(this, EventArgs.Empty);
    }
    

    private void RecreateGame()
    {
        _game = new KnightsTourGame(_currentBoardSize);
        RestartGame();
    }

    private async Task NavigateBack()
    {
        await Shell.Current.GoToAsync("mainpage");
    }

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
}