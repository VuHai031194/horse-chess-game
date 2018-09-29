using UnityEngine;
using System;
public static class FEN{

    //Default FEN
    public static string Default = "nnn2nnn/8/8/8/8/8/8/NNN2NNN b 00";


    /// <summary>
    /// Construct board based on passed FeN.
    /// </summary>
	public static void BoardFromFen(this Board board, string FeN){

        int i = 0, j = 0;
        char letter;

        //Split FEN by spaces
        string[] FENData = FeN.Split(' ');

        while (i < 64 && j < FENData[0].Length)
        {

            letter = FENData[0][j];

            switch (letter)
            {
                case 'n': board.BKnight |= Ops.Pow2[i]; board.pieces[i] = Defs.BKnight; break;
                case 'N': board.WKnight |= Ops.Pow2[i]; board.pieces[i] = Defs.WKnight; break;
                case '/': i--; break;
                case '1': break;
                case '2': i++; break;
                case '3': i += 2; break;
                case '4': i += 3; break;
                case '5': i += 4; break;
                case '6': i += 5; break;
                case '7': i += 6; break;
                case '8': i += 7; break;
            }

            j++;
            i++;   
        }

        //Whose turn?
        if (FENData.Length > 1)
        {
            letter = FENData[1][0];
			if (letter == 'w') board.SideToPlay = 1;
			else if (letter == 'b') board.SideToPlay = 0;
        }

        var score = Int32.Parse(FENData[2]);
        board.scoreWhite = score % 10;
        board.scoreBlack = score / 10;
    }

    public static string FenFromBoard(this Board board) {

        string fen = "";
        char[] pieces = new char[3]{
            'x', 'N', 'n'
        };

        int count = 0;
        
        for (int i = 0; i < 64; i++) {
            
			int piece = board.GetPieceAt(i);

			if (piece == 0)
            {
                count++;
            }
            else
            {
                if (count > 0)
                {
                    fen += count;
                    count = 0;
                }

                fen += pieces[piece];
            }

            if ((i + 1) % 8 == 0 && i+1 != 64)
            {

                if (count > 0)
                {
                    fen += count;
                    count = 0;
                }

                fen += '/';
            }
        }

        //Side to play

        char[] side = new char[2] { 'b', 'w' };

        fen += " " + side[board.SideToPlay];

        fen += " " + board.scoreBlack + "" + board.scoreWhite;

        return fen;
    }
    

}

///
/// 5n2/8/8/4n1N1/4N3/8/nnN5/ w 44
/// Ko an quan lai di hien quan.
/// 5n2/8/8/8/4N1n1/5N2/nnN5/ w 44
/// Ko chan quan dan den thua toan cuoc
/// 8/1N5N/8/4n3/2n5/8/nnN5/ w 44
/// kO CHAN QUAN TAP 2. nGU HON TAP 1
/// 8/2nn4/8/2N5/8/8/nnN5/ w 54
/// Đi chưa được tốt lắm
/// 7n/8/3nN3/8/8/2N1n3/4N3/ w 53
/// Chua chon dc nuoc di tot nhat
/// 7n/8/5n2/1Nnn1N2/3N4/8/4Nn2/N w 12
/// Di ngu



