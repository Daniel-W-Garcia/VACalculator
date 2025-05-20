using Microsoft.Maui.Controls.Shapes;
using Syncfusion.Maui.Picker;
using Syncfusion.Maui.Toolkit.Popup;

namespace VACalculatorApp;

public partial class KnightTourPage
{
    private KnightsTourGame _game;
    private Button[,] _buttonGrid;
    private int _currentBoardSize = 8; //default size
    
    
    public KnightTourPage()
    {
        InitializeComponent();
        _game = new KnightsTourGame(_currentBoardSize);
        _buttonGrid = new Button[_currentBoardSize, _currentBoardSize];
        SetupChessboard();
    }
    private void OnBoardSizeButton_OnClicked(object sender, PickerSelectionChangedEventArgs pickerArgs)
    {
        if (ChessboardGrid == null) return; // Prevent event from running too early

        var gameConfiguration = BindingContext as GameBoardConfiguration;
        if (gameConfiguration?.BoardSizes == null || gameConfiguration.BoardSizes.Count == 0)
        {
            return; // Or fallback to a default size
        }

        var sizeOptions = gameConfiguration.BoardSizes;
        if (pickerArgs.NewValue < 0 || pickerArgs.NewValue >= sizeOptions.Count) //NewValue is a property of the PickerSelectionChangedEventArgs class from Sf
        {
            return;
        }

        string sizeString = sizeOptions[pickerArgs.NewValue]; //getting the string from the picker
        int newSize = ParseBoardSize(sizeString); //parsing the string to an int
        RecreateChessboard(newSize); //calling the function to recreate the chessboard with the new size
    }


    private void RecreateChessboard(int boardSize)
    {
        if (ChessboardGrid == null) return;
        _currentBoardSize = boardSize; // store current board size

        _game = new KnightsTourGame(boardSize);
        _buttonGrid = new Button[boardSize, boardSize];

        ChessboardGrid.RowDefinitions.Clear();
        for (int i = 0; i < boardSize; i++)
        {
            ChessboardGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
        }

        ChessboardGrid.ColumnDefinitions.Clear();
        for (int i = 0; i < boardSize; i++)
        {
            ChessboardGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        }
        SetupChessboard();
    }


    private int ParseBoardSize(string pickerValue) //getting input from the SfPicker and converting it to an int for the board size
    {
        if (pickerValue == null) return 8; // fallback to default size
        
        var boardSizeParts = pickerValue.Split('x', '×');
        
        if (boardSizeParts.Length == 2 && int.TryParse(boardSizeParts[0].Trim(), out int parsedSize))
        {
            return parsedSize; //only need 1 int since the board is square, so return the first one
        }
        return 8; //fall through to default size just in case
    }


    private void SetupChessboard()
    {
        if (ChessboardGrid == null) return;
        ChessboardGrid.Children.Clear();

        for (int row = 0; row < _currentBoardSize; row++)
        {
            for (int col = 0; col < _currentBoardSize; col++)
            {
                var cellButton = new Button
                {
                    BackgroundColor = (row + col) % 2 == 0 ? Color.FromArgb("#55d11f") : Colors.AliceBlue, //green and white board
                    CornerRadius = 0,
                    Margin = 1,
                    Padding = 0
                };
                int r = row, c = col;
                cellButton.Clicked += (sender, args) => CellButton_Clicked(r, c);
                ChessboardGrid.Add(cellButton, col, row);
                _buttonGrid[row, col] = cellButton;
            }
        }
    }

    private void CellButton_Clicked(int row, int col)
    {
        if (_game.Move(row, col))
        {
            // Update UI to show the knight's position
            UpdateBoard();
            ResultLabel.Text = $"Knight moved to Row {row+1}, Column {col+1}";
        }
        else
        {
            ResultLabel.Text = "Invalid move!";
        }

        if (_game.GetLegalMoves(row, col).Count == 0)
        {
            if (!_game.WinConditionMet)
            {
                ResultLabel.Text = "No legal moves. Game over!\nPress RESTART to try again";
            }
            
            else if (_game.WinConditionMet)
            {
                ResultLabelWin.Text = "Congratulations! You have completed the Knight's Tour!";
            }
        }
    }

    private void UpdateBoard()
    {
        int size = _currentBoardSize;

        RemoveOldDotsFromBoard();
        ResetButtonAppearance(size);
        UpdateVisitedCellAppearance(size);

        int kx = _game.CurrentX, ky = _game.CurrentY; // get current knight position
        UpdateKnightPosition(kx, ky);// update the knight's position on the board incase it was over written

        AddMoveIndicatorToBoard(kx, ky);
    }

    private void UpdateKnightPosition(int kx, int ky)
    {
        if (kx >= 0 && ky >= 0)
        {
            var current = _buttonGrid[kx, ky];
            current.BackgroundColor = Colors.DarkSlateGray;
            current.Text            = "♘";
            current.FontSize        = 24;
        }
    }

    private void UpdateVisitedCellAppearance(int size)
    {
        var visited = _game.GetBoard();
        for (int row = 0; row < size; row++)
        {
            for (int col = 0; col < size; col++)
            {
                if (visited[row, col])
                {
                    var cellButton = _buttonGrid[row, col];
                    cellButton.BackgroundColor = Colors.OrangeRed;
                    cellButton.Text = "♘";
                }
            }   
        }
    }

    private void ResetButtonAppearance(int size)
    {
        for (int rowIndex = 0; rowIndex < size; rowIndex++)
        {
            for (int colIndex = 0; colIndex < size; colIndex++)
            {
                var btn = _buttonGrid[rowIndex, colIndex];
                btn.Text        = "";
                btn.FontSize    = 14;
                btn.TextColor   = Colors.Black;
                btn.BackgroundColor = (rowIndex + colIndex) % 2 == 0 ? Color.FromArgb("#55d11f") : Colors.AliceBlue;
            }
        }
    }

    private void RemoveOldDotsFromBoard()
    {
        var oldDots = ChessboardGrid.Children
            .Where(v => v is Ellipse)
            .Cast<Ellipse>()
            .ToList();

        foreach (var dot in oldDots)
        {
            ChessboardGrid.Children.Remove(dot);
        }
    }

    private void AddMoveIndicatorToBoard(int kx, int ky)
    {
        foreach (var (legalMoveRow, legalMoveColumn) in _game.GetLegalMoves(kx, ky))
        {
            var moveIndicator = new Ellipse
            {
                Fill              = Colors.Gray.WithAlpha(0.5f).AsPaint(),
                WidthRequest      = 24,
                HeightRequest     = 24,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions   = LayoutOptions.Center
            };
            ChessboardGrid.Add(moveIndicator, legalMoveColumn, legalMoveRow);
        }
    }


    private void OnRestartClicked(object sender, EventArgs e)
    {
        _game.Reset();
        RemoveOldDotsFromBoard();
        ResetBoardAppearance();
    }

    private void ResetBoardAppearance()
    {
        for (int row = 0; row < _currentBoardSize; row++)
        for (int col = 0; col < _currentBoardSize; col++)
        {
            _buttonGrid[row, col].Text = "";
            _buttonGrid[row, col].BackgroundColor = (row + col) % 2 == 0 ? Color.FromArgb("#55d11f") : Colors.AliceBlue;
        }

        ResultLabel.Text = "Board reset. Select a starting position.";
    }

    private void BoardSizePickerButton_OnClicked(object? sender, EventArgs buttonEventArgs)
    {
        BoardSizePicker.IsOpen = true;
    }

    private void BoardSizePicker_OnCancelButtonClicked(object? sender, EventArgs footerEventArgs)
    {
        BoardSizePicker.IsOpen = false;
    }

    private async void NavigateToMainPage_OnClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("mainpage");
    }


    private void GameRules_OnClicked(object? sender, EventArgs e)
    {
        KnightsTourRulesPopup.Show();
    }
}