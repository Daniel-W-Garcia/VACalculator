namespace VACalculatorApp;

public class KnightTourSolver //this whole thing is copied from the internet https://www.geeksforgeeks.org/the-knights-tour-problem/
{
    static bool isSafe(int x, int y, int n, int[,] board) {
        return (x >= 0 && y >= 0 && x < n &&
                y < n && board[x, y] == -1);
    }

    static bool knightTourUtil(int x, int y, int step, int n,
        int[,] board,
        int[] dx, int[] dy) {

        // If all squares are visited
        if (step == n * n) {
            return true;
        }

        // Try all 8 possible knight moves
        for (int i = 0; i < 8; i++) {
            int nx = x + dx[i];
            int ny = y + dy[i];

            if (isSafe(nx, ny, n, board)) {
                board[nx, ny] = step;

                if (knightTourUtil(nx, ny, step + 1,
                        n, board, dx, dy)) {

                    return true;
                }

                // Backtrack
                board[nx, ny] = -1;
            }
        }

        return false;
    }

    static int[,] knightTour(int n) {

        int[,] board = new int[n, n];
        for (int i = 0; i < n; i++) {
            for (int j = 0; j < n; j++) {
                board[i, j] = -1;
            }
        }

        // 8 directions of knight moves
        int[] dx = {2, 1, -1, -2, -2, -1, 1, 2};
        int[] dy = {1, 2, 2, 1, -1, -2, -2, -1};

        // Start from top-left corner
        board[0, 0] = 0;

        if (knightTourUtil(0, 0, 1, n, board, dx, dy)) {
            return board;
        }

        return new int[,] { {-1} };
    }
    
    private static void PrintAnswer() 
    {
        int n = 5;

        int[,] result = knightTour(n);

        for (int i = 0; i < n; i++) 
        {
            for (int j = 0; j < n; j++) 
            {
                Console.Write(result[i, j] + " ");
            }
            Console.WriteLine();
        }
    }
}