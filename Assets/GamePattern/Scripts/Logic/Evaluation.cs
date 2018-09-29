using UnityEngine;
using System.Collections;


/// <summary>
/// Basic evaluation.
/// </summary>
public static class Evaluation
{
    static int nagativeAttack = 150;
    //public static int mark = 63;
    //public static int[] BlackKnightSquareTable = new int[64]
    //        {
    //            8,  10,  8,  10,  8,   11,  8,   9,
    //            15, 20, 15,  20,  15,  20,  15,  19,
    //            19, 15, 22,  15,  23,  15,  21,  15,
    //            15, 32, 40,  32,  70,  32,  40,  20,
    //            32, 64, 32,  64,  32,  64,  32,  64,
    //            64, 32, 32,  130, 114,  120, 16,  32, 
    //            32, 60, 160, 40,  32,  60,  160, 60, 
    //            64, 32, 40,  150, 200, 32,  40,  32
    //        };
    public static int[] BlackKnightSquareTable = new int[64]
    {
        9,  8,  11,  8,  10,   8,  10,   8,
        19, 15, 20,  15,  20,  15,  20,  15,
        15, 21, 15,  23,  15,  22,  15,  19,
        20, 40, 32,  70,  32,  40,  32,  15,
        64, 32, 64,  32,  64,  32,  64,  32,
        32, 16, 120,  114, 130,  32, 32,  64, 
        60, 160, 60, 32,  40,  160,  60, 32, 
        32, 40, 32,  500, 300, 40,  32,  64
    };

    //public static int[] WhiteKnightSquareTable = new int[64]
    //        {
    //            32, 40,  32,  200, 150, 40,  32, 40,
    //            40, 160, 64,  32,  40,  160, 64, 32, 
    //            32, 16,  120, 114, 130, 32,  32, 40, 
    //            60, 32,  60,  32,  60,  32,  60, 32,
    //            20, 40,  32,  70,  32,  40,  32, 15,
    //            15, 21,  15,  23,  15,  22,  15, 19,
    //            19, 15,  20,  15,  20,  15,  20, 15,
    //            9,  8,   11,  8,   10,  8,   10, 8
    //        };
    public static int[] WhiteKnightSquareTable = new int[64]
    {
        40, 32,  40,  300, 500, 32,  40, 32,
        32, 64, 160,  40,  32,  64, 160, 40, 
        40, 32,  32, 130, 114, 120,  16, 32, 
        32, 60,  32,  60,  32,  60,  32, 60,
        15, 32,  40,  32,  70,  32,  40, 20,
        19, 15,  22,  15,  23,  15,  21, 15,
        15, 20,  15,  20,  15,  20,  15, 19,
        8,  10,   8,  10,  8,   11,   8, 9
    };

    private static int blackCount = 0;
    private static int whiteCount = 0;
    public static int Evaluate(Board board)
    {
        blackCount = whiteCount = 0;

        if (IsEndGame(board))
        {
            return EndGame(board);
        }

        PreventArea(board);
        AttackArea(board);

        int score = 0;
        score -= Calculate(Defs.BKnight, BlackKnightSquareTable, board);
        score += Calculate(Defs.WKnight, WhiteKnightSquareTable, board);

        score -= (board.scoreBlack - (Defs.MaxChess - whiteCount)) * Defs.ScoreValue;
        score += (board.scoreWhite - (Defs.MaxChess - blackCount)) * Defs.ScoreValue;
        return score;
    }

    /// <summary>
    /// Counts how many pieces are there and then that number is multiplied by piece score. Also simple piece table is used.
    /// </summary>
    private static int Calculate(int typeHorse, int[] Table, Board board)
    {

        int score = 0;

        int pieceCount = 0;

        ulong PieceBitboard = typeHorse == Defs.BKnight ? board.BKnight : board.WKnight;

        while (PieceBitboard != 0)
        {
            int index = Ops.PopFirstBit(ref PieceBitboard);
            if (typeHorse == Defs.BKnight)
            {
                score += Table[index]; //Current piece position gives pre defined amount of points
                blackCount++;
            }
            else
            {
                score += Table[index];
                whiteCount++;
            }

            pieceCount++;

        }

        score += Defs.ChessValue * pieceCount;

        //score += Defs.ScoreValue *  (typeHorse == Defs.BKnight ? board.scoreBlack : board.scoreWhite);

        return score;

    }

    private static void PreventArea(Board board)
    {
        if (board.SideToPlay == 1)
        {
            //Debug.Log("Side: " + board.SideToPlay);
            // BLACK
            // chan o 1 diem
            BlackKnightSquareTable[10] = BlackKnightSquareTable[12] = 20;
            if (board.GetPieceAt(9) == Defs.WKnight)
                BlackKnightSquareTable[10] += 120;
            if (board.GetPieceAt(18) == Defs.WKnight)
                BlackKnightSquareTable[10] += 120;
            if (board.GetPieceAt(13) == Defs.WKnight)
                BlackKnightSquareTable[12] += 115;
            if (board.GetPieceAt(20) == Defs.WKnight)
                BlackKnightSquareTable[12] += 115;
            // chan o 2 diem
            BlackKnightSquareTable[11] = BlackKnightSquareTable[13] = 15;
            if (board.GetPieceAt(10) == Defs.WKnight)
                BlackKnightSquareTable[11] += 150;
            if (board.GetPieceAt(19) == Defs.WKnight)
                BlackKnightSquareTable[11] += 150;
            if (board.GetPieceAt(14) == Defs.WKnight)
                BlackKnightSquareTable[13] += 145;
            if (board.GetPieceAt(21) == Defs.WKnight)
                BlackKnightSquareTable[13] += 145;
        }
        else if (board.SideToPlay == 0)
        {
            //Debug.Log("Side: " + board.SideToPlay);
            // WHITE
            // chan o 1 diem
            WhiteKnightSquareTable[51] = WhiteKnightSquareTable[53] = 20;
            if (board.GetPieceAt(45) == Defs.BKnight)
                WhiteKnightSquareTable[53] += 120;
            if (board.GetPieceAt(54) == Defs.BKnight)
                WhiteKnightSquareTable[53] += 120;
            if (board.GetPieceAt(50) == Defs.BKnight)
                WhiteKnightSquareTable[51] += 115;
            if (board.GetPieceAt(43) == Defs.BKnight)
                WhiteKnightSquareTable[51] += 115;
            // chan o 2 diem
            WhiteKnightSquareTable[50] = WhiteKnightSquareTable[52] = 15;
            if (board.GetPieceAt(44) == Defs.BKnight)
                WhiteKnightSquareTable[52] += 150;
            if (board.GetPieceAt(53) == Defs.BKnight)
                WhiteKnightSquareTable[52] += 150;
            if (board.GetPieceAt(42) == Defs.BKnight)
                WhiteKnightSquareTable[50] += 145;
            if (board.GetPieceAt(49) == Defs.BKnight)
                WhiteKnightSquareTable[50] += 145;
        } 
    }

    public static void AttackArea(Board board)
    {
        if (board.SideToPlay == 0)
        {
        
            //Debug.Log("Side: " + board.SideToPlay);
            // BLACK
            // tan cong o 1 diem
            BlackKnightSquareTable[43] = 114;
            BlackKnightSquareTable[45] = 32;
            BlackKnightSquareTable[50] = 60;
            BlackKnightSquareTable[54] = 60;
            if (board.pieces[51] == Defs.WKnight)
            {
                BlackKnightSquareTable[43] -= 60;
                BlackKnightSquareTable[50] -= 60;
            }
            else if (board.pieces[51] == Defs.BKnight)
            {
                BlackKnightSquareTable[43] -= 30;
                BlackKnightSquareTable[50] -= 30;
            }

            if (board.pieces[53] == Defs.WKnight)
            {
                BlackKnightSquareTable[45] -= 60;
                BlackKnightSquareTable[54] -= 60;
            }
            else if (board.pieces[53] == Defs.BKnight)
            {
                BlackKnightSquareTable[45] -= 30;
                BlackKnightSquareTable[45] -= 30;
            }
            // tan cong o 2 diem
            BlackKnightSquareTable[42] = 120;
            BlackKnightSquareTable[44] = 130;
            BlackKnightSquareTable[49] = 160;
            BlackKnightSquareTable[53] = 160;
            if (board.pieces[50] == Defs.WKnight)
            {
                BlackKnightSquareTable[42] -= 60;
                BlackKnightSquareTable[49] -= 60;
            }
            else if (board.pieces[50] == Defs.BKnight)
            {
                BlackKnightSquareTable[42] -= 30;
                BlackKnightSquareTable[49] -= 30;
            }

            if (board.pieces[52] == Defs.WKnight)
            {
                BlackKnightSquareTable[44] -= 60;
                BlackKnightSquareTable[53] -= 60;
            }
            else if (board.pieces[52] == Defs.BKnight)
            {
                BlackKnightSquareTable[44] -= 30;
                BlackKnightSquareTable[53] -= 30;
            }
        }
        else if (board.SideToPlay == 1)
        {
        //Debug.Log("Side: " + board.SideToPlay);
        // WHITE
        // tan cong o 1 diem
            WhiteKnightSquareTable[9] = 64;
            WhiteKnightSquareTable[13] = 64;
            WhiteKnightSquareTable[18] = 32;
            WhiteKnightSquareTable[20] = 114;
            if (board.pieces[10] == Defs.BKnight)
            {
                WhiteKnightSquareTable[9] -= nagativeAttack;
                WhiteKnightSquareTable[18] -= nagativeAttack;
            }
            else if (board.pieces[10] == Defs.WKnight)
            {
                WhiteKnightSquareTable[9] -= nagativeAttack/2;
                WhiteKnightSquareTable[18] -= nagativeAttack/2;
            }

            if (board.pieces[12] == Defs.BKnight)
            {
                WhiteKnightSquareTable[13] -= nagativeAttack;
                WhiteKnightSquareTable[20] -= nagativeAttack;
            }
            else if (board.pieces[12] == Defs.WKnight)
            {
                WhiteKnightSquareTable[13] -= nagativeAttack/2;
                WhiteKnightSquareTable[20] -= nagativeAttack/2;
            }

        // tan cong o 2 diem
            WhiteKnightSquareTable[10] = 160;
            WhiteKnightSquareTable[14] = 160;
            WhiteKnightSquareTable[19] = 130;
            WhiteKnightSquareTable[21] = 120;

            if (board.pieces[11] == Defs.BKnight)
            {
                WhiteKnightSquareTable[10] -= nagativeAttack;
                WhiteKnightSquareTable[19] -= nagativeAttack;
            }
            else if (board.pieces[11] == Defs.WKnight)
            {
                WhiteKnightSquareTable[10] -= nagativeAttack/2;
                WhiteKnightSquareTable[19] -= nagativeAttack/2;
            }

            if (board.pieces[13] == Defs.BKnight)
            {
                WhiteKnightSquareTable[14] -= nagativeAttack;
                WhiteKnightSquareTable[21] -= nagativeAttack;
            }
            else if (board.pieces[13] == Defs.WKnight)
            {
                WhiteKnightSquareTable[14] -= nagativeAttack / 2;
                WhiteKnightSquareTable[21] -= nagativeAttack / 2;
            }
        } 
    }

    private static int EndGame(Board board)
    {
        if (board.scoreBlack >= Defs.ScoreWin)
            return -5767;
        if (board.scoreWhite >= Defs.ScoreWin)
            return 5767;
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

// nnn2nnn/8/8/8/8/8/8/NNN2NNN b 00

// 8/4nN2/1n6/8/8/3N1n2/8/5N b 33 ko an o 2 diem

// n5n1/n7/6n1/3N4/1N6/8/5n2/N b 35

// 6n1/3nn3/4N3/5N2/6N1/3n1N2/8/ w 22

