namespace VACalculatorApp
{
    public partial class KnightTourPage : ContentPage
    {
        private KnightsTourGame _game;
        private Button[,] _cellButtons;

        public KnightTourPage()
        {
            InitializeComponent();
            _game = new KnightsTourGame();
            _cellButtons = new Button[8, 8]; // TODO 8x8 chessboard for now, but may add option later for user input to choose size
            SetupChessboard();
        }

        private void SetupChessboard()
        {
            // Clear existing children
            ChessboardGrid.Children.Clear();
            
            // Create cell buttons for the chessboard
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    var cellButton = new Button
                    {
                        BackgroundColor = (row + col) % 2 == 0 ? Color.FromArgb("#55d11f") : Colors.AliceBlue,
                        CornerRadius = 0,
                        Margin = 1,
                        Padding = 0
                    };
                    
                    // Store row/col for later use in event handler
                    int r = row;
                    int c = col;
                    
                    cellButton.Clicked += (sender, args) => CellButton_Clicked(r, c);
                    
                    ChessboardGrid.Add(cellButton, col, row);
                    _cellButtons[row, col] = cellButton;
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
                
                if (_game.IsCompleted())//TODO need to add win and lose conditions
                {
                    ResultLabel.Text = "Tour completed!";
                }
            }
            else
            {
                ResultLabel.Text = "Invalid move!";
            }
        }

        private void UpdateBoard()
        {
            // 1) Reset every cell to its “base” chess‐board color and clear text
            /*for (int boardRow = 0; boardRow < 8; boardRow++)
            {
                for (int currentColumn = 0; currentColumn < 8; currentColumn++)
                {
                    _cellButtons[boardRow, currentColumn].Text = "";
                    _cellButtons[boardRow, currentColumn].BackgroundColor = (boardRow + currentColumn) % 2 == 0 ? Color.FromArgb("#55d11f") : Colors.AliceBlue;
                }
            }*/

            // 2) Mark all previously visited cells
            var visited = _game.GetBoard();
            for (int rowIndex = 0; rowIndex < 8; rowIndex++)
            {
                for (int colIndex = 0; colIndex < 8; colIndex++)
                {
                    if (visited[rowIndex, colIndex])
                    {
                        _cellButtons[rowIndex, colIndex].Text = "♘";
                        _cellButtons[rowIndex, colIndex].BackgroundColor = Colors.DarkSlateGray;
                    }
                }
            }

            // 3) Place the knight
            var kx = _game.CurrentX;
            var ky = _game.CurrentY;
            if (kx >= 0 && ky >= 0)
            {
                var btn = _cellButtons[kx, ky];
                btn.Text = "♘";
                btn.FontSize = 24;
                btn.TextColor = Colors.Black;
            }

            // 4) Highlight all legal moves
            var moves = _game.GetLegalMoves(kx, ky);
            foreach (var (r, c) in moves)
            {
                _cellButtons[r, c].BackgroundColor = Colors.LightGoldenrodYellow;
            }
        }

        private void OnRestartClicked(object sender, EventArgs e)
        {
            _game.Reset();
            
            // Reset button visuals
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    _cellButtons[row, col].Text = "";
                    _cellButtons[row, col].BackgroundColor = (row + col) % 2 == 0 ? Color.FromArgb("#55d11f") : Colors.AliceBlue;
                }
            }
            ResultLabel.Text = "Board reset. Select a starting position.";
        }
    }
}