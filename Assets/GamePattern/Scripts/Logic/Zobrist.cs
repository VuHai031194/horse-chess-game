using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// Zobrist hashing.
/// </summary>
public static class Zobrist {

    public static long[][] Pieces; // Square | Pieces
    public static long[] SideToPlay; //Only for one side
    public static long[][] Scores; // Scores for side play

    private static System.Random rand = new System.Random(700); //Same generation everywhere, in case of openinig book
    /// <summary>
    /// Initializes the values.
    /// </summary>
    public static void Init() {

        Pieces = new long[64][];
        SideToPlay = new long[2];
        Scores = new long[2][];


        //Every square
        for (int i = 0; i < 64; i++) {

            Pieces[i] = new long[3];

            //For every piece
            for (int p = 0; p < Pieces[i].Length; p++)
            {
                Pieces[i][p] = NextInt64();
            }
        }

        for (int i = 0; i < 2; i++)
        {
            Scores[i] = new long[100];
            for (int p = 0; p < Scores[i].Length; p++)
            {
                Scores[i][p] = NextInt64();
            }
        }


        SideToPlay[0] = NextInt64();
        SideToPlay[1] = NextInt64();

    }

    /// <summary>
    /// Get almost unique hash position.
    /// </summary>
    public static long GetHashPosition(this Board board) {
        long pos = 0;

        //Get data from pieces array
        for (int i = 0; i < 64; i++) {
            pos ^= Pieces[i][board.pieces[i]];
        }

        //Side
        pos ^= SideToPlay[board.SideToPlay];

        pos ^= Scores[0][board.scoreBlack];
        pos ^= Scores[1][board.scoreWhite];

        return pos;
    }

    /// <summary>
    /// Random number generator.
    /// </summary>
    public static long NextInt64()
    {
        var buffer = new byte[sizeof(Int64)];
        rand.NextBytes(buffer);
        return Math.Abs(BitConverter.ToInt64(buffer, 0));
    }

}
