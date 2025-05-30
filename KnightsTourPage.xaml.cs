using Microsoft.Maui.Controls.Shapes;
using Syncfusion.Maui.Picker;

namespace VACalculatorApp;

public partial class KnightTourPage
{
    private KnightsTourViewModel _tourViewModel;
    private Button[,] _buttonGrid;
    private bool isResizing = false;
    
    
    public KnightTourPage()
    {
        _tourViewModel = new KnightsTourViewModel();
        BindingContext = _tourViewModel;
        InitializeComponent();
        
        _tourViewModel.BoardUpdated +=(s , e) =>
        {
            if(!isResizing) UpdateBoard();  //subscribe to the event to update the board when it changes (when a move is made) only when board is not being resized
        };
        _tourViewModel.RemoveOldDotsRequested += (s, e) => RemoveOldDotsFromBoard();
        var boardSizeOptions = GameBoardConfiguration.BoardSizes;
        BoardSizePicker.Columns[0].ItemsSource = boardSizeOptions;
        
        _buttonGrid = new Button[_tourViewModel.CurrentBoardSize, _tourViewModel.CurrentBoardSize]; //2D matrix for the chessboard
        SetupChessboard();
    }
    private void OnBoardSizeButton_OnClicked(object sender, PickerSelectionChangedEventArgs pickerArgs)
    {
        if (ChessboardGrid == null) return; // Prevent event from running too early

        var sizeOptions = GameBoardConfiguration.BoardSizes;
        
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
        isResizing = true;
        _tourViewModel.CurrentBoardSize = boardSize;
        
        _buttonGrid = new Button[boardSize,boardSize]; // store current board size
        
        ChessboardGrid.RowDefinitions.Clear();
        ChessboardGrid.ColumnDefinitions.Clear();

        for (int i = 0; i < boardSize; i++) // add rows and columns to the grid (boardSize x boardSize)
        {
            ChessboardGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            ChessboardGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        }

        SetupChessboard();
        isResizing = false;
    }
    
    private void SetupChessboard()
    {
        if (ChessboardGrid == null) return;
        ChessboardGrid.Children.Clear();

        int boardSize = _tourViewModel.CurrentBoardSize;
        for (int row = 0; row < boardSize; row++)
        {
            for (int col = 0; col < boardSize; col++)
            {
                var cellButton = new Button
                {
                    BackgroundColor = (row + col) % 2 == 0 ? Color.FromArgb("#55d11f") : Colors.AliceBlue,
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
        if (_tourViewModel.AttemptMove(row, col))
        {
            UpdateBoard();
        }
    }
    
    private void UpdateBoard()
    {
        int size = _tourViewModel.CurrentBoardSize;

        RemoveOldDotsFromBoard();
        ResetButtonAppearance(size);
        UpdateVisitedCellAppearance(size);

        int kx = _tourViewModel.CurrentX, ky = _tourViewModel.CurrentY; // get current knight position
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
        var visited = _tourViewModel.GetBoard();
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
        foreach (var (legalMoveRow, legalMoveColumn) in _tourViewModel.GetLegalMoves(kx, ky))
        {
            var moveIndicator = new Ellipse
            {
                Fill              = Colors.Gray.WithAlpha(0.5f).AsPaint(),
                WidthRequest      = 24,
                HeightRequest     = 24,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions   = LayoutOptions.Center,
                InputTransparent  = true
            };
            ChessboardGrid.Add(moveIndicator, legalMoveColumn, legalMoveRow);
        }
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

    private void BoardSizePickerButton_OnClicked(object? sender, EventArgs e) => BoardSizePicker.IsOpen = true;
    
    private void BoardSizePicker_OnCancelButtonClicked(object? sender, EventArgs e) => BoardSizePicker.IsOpen = false;
    private void GameRules_OnClicked(object? sender, EventArgs e) => KnightsTourRulesPopup.Show();
}