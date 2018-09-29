/// <summary>
/// Basic evaluation.
/// </summary>
public static class Evaluation
{
    public static int[] BlackKnightSquareTable = new int[64]
    {
        1, 1, 1, 1, 1, 1, 1, 1,
        1, 1, 1, 1, 1, 1, 1, 1,
        1, 1, 1, 1, 1, 1, 1, 1,
        1, 1, 1, 1, 1, 1, 1, 1,
        1, 1, 1, 1, 1, 1, 1, 1,
        1, 1, 1, 1, 1, 1, 1, 1,
        1, 1, 1, 1, 1, 1, 1, 1,
        1, 1, 1, 1, 1, 1, 1, 1
    };

    public static int[] WhiteKnightSquareTable = new int[64]
    {
        1, 1, 1, 1, 1, 1, 1, 1,
        1, 1, 1, 1, 1, 1, 1, 1,
        1, 1, 1, 1, 1, 1, 1, 1,
        1, 1, 1, 1, 1, 1, 1, 1,
        1, 1, 1, 1, 1, 1, 1, 1,
        1, 1, 1, 1, 1, 1, 1, 1,
        1, 1, 1, 1, 1, 1, 1, 1,
        1, 1, 1, 1, 1, 1, 1, 1
    };

    public static int Evaluate(Board board)
    {
        if (IsEndGame(board))
        {
            return EndGame(board);
        }

        int score = 0;
        score -= Calculate(Defs.BKnight, BlackKnightSquareTable, board);
        score += Calculate(Defs.WKnight, WhiteKnightSquareTable, board);
        return score;
    }

    /// <summary>
    /// Counts how many pieces are there and then that number is multiplied by piece score. Also simple piece table is used.
    /// </summary>
    private static int Calculate(int typeHorse, int[] Table, Board board)
    {

        int score = 0;

        ulong PieceBitboard = ((typeHorse == Defs.BKnight) ? board.BKnight : board.WKnight);

        while (PieceBitboard != 0)
        {
            int index = Ops.PopFirstBit(ref PieceBitboard);
            if (typeHorse == Defs.BKnight)
            {
                score += Table[index]; //Current piece position gives pre defined amount of points
            }
            else
            {
                score += Table[index];
            }
        }
        return score;
    }
    
    private static int EndGame(Board board)
    {
        if (board.scoreBlack >= Defs.ScoreWin)
            return -1;
        if (board.scoreWhite >= Defs.ScoreWin)
            return 1;
        return 0;
    }

    private static bool IsEndGame(Board board)
    {
        if (board.BKnight == 0 || board.WKnight == 0)
            return true;
        if (board.scoreWhite >= Defs.ScoreWin || board.scoreBlack >= Defs.ScoreWin)
            return true;
        return false;
    }
}

