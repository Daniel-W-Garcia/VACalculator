public class KnightsTourGame
{
    private readonly int _size;
    private readonly bool[,] _visitedPositions;
    public int CurrentX { get; private set; } = -1;
    public int CurrentY { get; private set; } = -1;
    public int MovesMade { get; private set; } = 0; //TODO display a counter of moves. Perhaps count down from winning condition.
    
    public bool WinConditionMet { get; private set; }

    public KnightsTourGame(int size = 8)
    {
        _size = size;
        _visitedPositions = new bool[size, size];
    }

    public void Reset()//Z out the board
    {
        Array.Clear(_visitedPositions, 0, _visitedPositions.Length);

        CurrentX = CurrentY = -1;
        MovesMade = 0;

    }

    public bool IsPositionAvailable(int posX, int posY)
    {
        if (posX < 0 || posY < 0 || posX >= _size || posY >= _size)
        {
            return false;
        }
        return !_visitedPositions[posX, posY];
    }

    public List<(int, int)> GetLegalMoves(int currentX, int currentY)
    {
        var moveDirection = new (int directionX, int directionY)[] // tuple for 8 directions a knight can move. Offset from current position.
        {
            (2, 1), (1, 2), (-1, 2), (-2, 1),
            (-2, -1), (-1, -2), (1, -2), (2, -1)
        };
        
        var legalMoves = new List<(int, int)>();
        
        foreach (var direction in moveDirection)
        {
            int targetX = currentX + direction.directionX;
            int targetY = currentY + direction.directionY;
            
            if (IsPositionAvailable(targetX, targetY))
            {
                legalMoves.Add((targetX, targetY));
            }
        }
        return legalMoves;
    }

    public bool Move(int nextX, int nextY)
    {
        if (MovesMade == 0)
        {
            if (!IsPositionAvailable(nextX, nextY))
            {
                return false;
            }
        }
        else
        {
            // Subsequent moves: must be one of the knight’s legal moves
            var legal = GetLegalMoves(CurrentX, CurrentY);
            if (!legal.Contains((nextX, nextY)))
            {
                return false;
            }
        }
        // commit the move
        CurrentX = nextX;
        CurrentY = nextY;
        _visitedPositions[nextX, nextY] = true;
        MovesMade++;
        if (MovesMade == _size * _size)
        {
            WinConditionMet = true;
        }
        return true;
    }

    public bool[,] GetBoard() => (bool[,])_visitedPositions.Clone();
}