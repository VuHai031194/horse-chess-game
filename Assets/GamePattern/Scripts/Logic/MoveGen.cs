using System;
using UnityEngine;

/// <summary>
/// All about move generation.
/// </summary>
public static class MoveGen {


    #region Table


    /// <summary>
    /// Knight occupancy bitboard on every square.
    /// </summary>
    public static ulong[] KnightAttacksDatabase = {
		0x20400, 0x50800, 0xa1100, 0x142200, 0x284400, 0x508800, 0xa01000, 0x402000,
		0x2040004, 0x5080008, 0xa110011, 0x14220022, 0x28440044, 0x50880088, 0xa0100010, 0x40200020,
		0x204000402, 0x508000805, 0xa1100110a, 0x1422002214, 0x2844004428, 0x5088008850, 0xa0100010a0, 0x4020002040,
		0x20400040200, 0x50800080500, 0xa1100110a00, 0x142200221400, 0x284400442800, 0x508800885000, 0xa0100010a000, 0x402000204000,
		0x2040004020000, 0x5080008050000,  0xa1100110a0000, 0x14220022140000, 0x28440044280000, 0x50880088500000, 0xa0100010a00000, 0x40200020400000,
		0x204000402000000, 0x508000805000000, 0xa1100110a000000, 0x1422002214000000, 0x2844004428000000, 0x5088008850000000, 0xa0100010a0000000, 0x4020002040000000,
		0x400040200000000, 0x800080500000000, 0x1100110a00000000, 0x2200221400000000, 0x4400442800000000, 0x8800885000000000, 0x100010a000000000, 0x2000204000000000,
		0x4020000000000, 0x8050000000000, 0x110a0000000000, 0x22140000000000, 0x44280000000000, 0x88500000000000, 0x10a00000000000, 0x20400000000000

	};
    #endregion
    
    /// <summary>
    /// Knight prevent bitboard on every square.
    /// </summary>

    /// <summary>
    /// Generates all moves
    /// </summary>
    /// <param name="board">Board reference</param>
    /// <param name="moves">Already initialized moves array( new SMove[Defs.MaxMoves] )</param>
    /// <param name="StartAt">Array index offset</param>
    /// <param name="depth">Used in search, should not be changed otherwise( used for move killers)</param>
    /// <returns>Returns end index relative to the start</returns>
	public static int GenerateMoves(Board board, SMove[] moves, int StartAt = 0, int depth = 0) {
        var r = new System.Random(DateTime.Now.Second);

		int counter = StartAt;
        int[] pieces = board.pieces;
        
		if (board.SideToPlay == 1) //WHITE
        {

            #region White knights move generation

            ulong Knights = board.WKnight;
            while (Knights != 0)
            {

                int From = Ops.FirstBit(Knights);
           
                Knights ^= Ops.Pow2[From];

                ulong Moves = KnightAttacksDatabase[From] & ~board.WKnight; //Can't step on a friendly pieces
    
                while (Moves != 0)
                {
                    int To = Ops.PopFirstBit(ref Moves);

                    int Barrier = GetBarrierPos(From, To);

                    if (board.GetPieceAt(Barrier) != 0)
                        continue;
                    
                    int capturePiece = pieces[To];

                    //Add move
                    int move = (From | (To << 6) | (Defs.WKnight << 12) | (capturePiece << 16));
                    moves[counter].move = move;
                    if (capturePiece == 0)
                    {
                        moves[counter].score = 100000 + (Evaluation.WhiteKnightSquareTable[To] - Evaluation.WhiteKnightSquareTable[From]);
                    }
                    else {

                        if (board.SearchKillers[0][depth] == move)
                        {
                            moves[counter].score = 900000;
                        }
                        else if (board.SearchKillers[1][depth] == move)
                        {
                            moves[counter].score = 800000;
                        }
                        else
                        {
                            //moves[counter].score = board.SearchHistory[board.pieces[From]][To];
                            moves[counter].score = 700000;
                        }  
                    }

                    if (To == 4) moves[counter].score += 1200000;
                    else if (To == 3) moves[counter].score += 500000;
     
                    moves[counter].score += r.Next(0, 10);
                    counter++;
                }
            }
            #endregion
        }
        else { //BLACK

            #region Black knights move generation

            ulong Knights = board.BKnight;

            while (Knights != 0)
            {

                int From = Ops.FirstBit(Knights);
                Knights ^= Ops.Pow2[From];
                ulong Moves = KnightAttacksDatabase[From] & ~board.BKnight; //Can't step on a friendly pieces
                
                while (Moves != 0)
                {
                    int To = Ops.PopFirstBit(ref Moves);

                    int Barrier = GetBarrierPos(From, To);
                 
                    if (board.GetPieceAt(Barrier) != 0)
                        continue;

                    int capturePiece = pieces[To];

                    //Add move
                    int move = (From | (To << 6) | (Defs.BKnight << 12) | (capturePiece << 16));
                    moves[counter].move = move;
                    if (capturePiece == 0)
                    {
                        moves[counter].score = 100000 + (Evaluation.BlackKnightSquareTable[To] - Evaluation.BlackKnightSquareTable[From]);
                    }
                    else
                    {

                        if (board.SearchKillers[0][depth] == move)
                        {
                            moves[counter].score = 900000;
                        }
                        else if (board.SearchKillers[1][depth] == move)
                        {
                            moves[counter].score = 800000;
                        }
                        else
                        {
                            //moves[counter].score = board.SearchHistory[board.pieces[From]][To];
                            moves[counter].score = 700000;
                        }
                    }

                    if (To == 59) moves[counter].score += 1200000;
                    else if (To == 60) moves[counter].score += 500000;
                    moves[counter].score += r.Next(0, 10);
                    counter++;

                }

            }

            #endregion
        }

        //Returns end index relative to the start
		return counter- StartAt;
	}

    public static int GenerateCapturingMoves(Board board, SMove[] moves, int StartAt = 0)
    {

        int counter = StartAt;

        int[] pieces = board.pieces;

        if (board.SideToPlay == 1) //WHITE
        {

            #region White knights move generation

            ulong Knights = board.WKnight;

            while (Knights != 0)
            {

                int From = Ops.FirstBit(Knights);
                Knights ^= Ops.Pow2[From];

                ulong Moves = KnightAttacksDatabase[From] & ~board.WKnight; //Can't step on a friendly pieces
                
                while (Moves != 0)
                {
                    int To = Ops.PopFirstBit(ref Moves);

                    if (To == 59 || To == 60)
                        continue;

                    int Barrier = GetBarrierPos(From, To);

                    if (board.GetPieceAt(Barrier) != 0)
                        continue;

                    int capturePiece = pieces[To];
                   
                    //Add move
                    if (capturePiece != 0 || To == 3 || To == 4)
                    {
                        moves[counter].move = (From | (To << 6) | (Defs.WKnight << 12) | (capturePiece << 16));
                        moves[counter].score = 0;
                        if (capturePiece != 0)
                            moves[counter].score = 700000;

                        if (To == 3) moves[counter].score += 500000;
                        else if (To == 4) moves[counter].score += 1200000;

                        counter++;
                    }
                }

            }

            #endregion

          
        }
        else
        { //BLACK

            #region Black knights move generation

            ulong Knights = board.BKnight;

            while (Knights != 0)
            {

                int From = Ops.FirstBit(Knights);
                Knights ^= Ops.Pow2[From];
                ulong Moves = KnightAttacksDatabase[From] & ~board.BKnight; //Can't step on a friendly pieces
                
                while (Moves != 0)
                {
                    int To = Ops.PopFirstBit(ref Moves);

                    if (To == 3 || To == 4)
                        continue;

                    int Barrier = GetBarrierPos(From, To);

                    if (board.GetPieceAt(Barrier) != 0)
                        continue;

                    int capturePiece = pieces[To];

                    //Add move
                    if (capturePiece != 0 || To == 59 || To == 60)
                    {
                        moves[counter].move = (From | (To << 6) | (Defs.BKnight << 12) | (capturePiece << 16));
                        moves[counter].score = 0;
                        if (capturePiece != 0)
                            moves[counter].score = 700000;

                        if (To == 60) moves[counter].score += 500000;
                        else if (To == 59) moves[counter].score += 1200000;
            
                        counter++;
                    }
                    
                }
            }
            #endregion
        }
        return counter - StartAt;
    }


    public static int GetBarrierPos(int From, int To)
    {
        switch (From - To)
        {
            case 17:
                return From - 8;
            case 15:
                return From - 8;
            case 10:
                return From - 1;
            case 6:
                return From + 1;
            case -6:
                return From - 1;
            case -10:
                return From + 1;
            case -15:
                return From + 8;
            case -17:
                return From + 8;
            //default:;
                break;
        }

        return -1;
    }

    //static int[] direct = new int[]{-1, 0, 0, 1, 1, 0, 0, -1};
    //static int[] xxx = new int[] { -8, 1, 8, -1 };

    //public static void GenPreventTable2()
    //{
    //    int From = 0;
    //    string str = "";
    //    for (int i = 0; i < 64; i++)
    //    {
    //        From = i;
    //        ulong Moves = KnightAttacksDatabase[From];
            
    //        ulong save = 0;
    //        int countBit = 0;
    //        while (Moves != 0)
    //        {
    //            int To = Ops.PopFirstBit(ref Moves);
    //            //Debug.Log("From: " + From  + " To: " + To);
    //            switch (From - To)
    //            {
    //                case 17:
    //                    save |= (ulong)(((ulong)From - 8) << (6*countBit));
    //                    //Debug.Log(save);
    //                    break;
    //                case 15:
    //                    save |= (ulong)(((ulong)From - 8) << (6 * countBit));
    //                    //Debug.Log(save);
    //                    break;
    //                case 10:
    //                    save |= (ulong)(((ulong)From - 1) << (6 * countBit));
    //                    //Debug.Log(save);
    //                    break;
    //                case 6:
    //                    save |= (ulong)(((ulong)From + 1) << (6 * countBit));
    //                    //Debug.Log(save);
    //                    break;
    //                case -6:
    //                    save |= (ulong)(((ulong)From - 1) << (6 * countBit));
    //                    //Debug.Log(save);
    //                    break;
    //                case -10:
    //                    save |= (ulong)(((ulong)From + 1) << (6 * countBit));
                       
    //                    break;
    //                case -15:
    //                    save |= (ulong)(((ulong)From + 8) << (6 * countBit));
    //                    //Debug.Log(save);
    //                    break;
    //                case -17:
    //                    save |= (ulong)(((ulong)From + 8) << (6 * countBit));
    //                    //Debug.Log(save);
    //                    break;
    //                default: Debug.Log("Some thing wrong");
    //                    break;
    //            }
    //            countBit++;
    //        }

    //        str += string.Format("0x{0:X}", save) + ", ";
    //        if ((63 - i) % 8 == 0)
    //            str += "\n";
    //        Debug.Log("Num: " + i);
    //        while (save != 0)
    //        {
    //            var pre = save & 0x3f;
    //            save = save >> 6;
    //            Debug.Log(pre);
    //        }
            
    //    }

    //    Debug.Log(str);
    //}

    // test 
    //public static void GenPreventTable()
    //{
    //    int[] str = new int[64];

    //    string preventTable = "";
        
    //    for (int i = 0; i < 8; i++)
    //    {
    //        for (int j = 0; j < 8; j++)
    //        {
    //            for (int k = 0; k < 64; k++)
    //            {
    //                str[k] = 0;
                    
    //            }
    //            for (int x = 0; x < 4; x++)
    //            {
    //                if (i + direct[x * 2] >= 0 && i + direct[x * 2] < 8 && j + direct[x * 2 + 1] >= 0 && j + direct[x * 2 + 1] < 8)
    //                {
    //                    str[i*8 + j + xxx[x]] = 1;
    //                }
    //            }
    //            string strx = "";
               
    //            for (int k = 63; k >= 0; k--)
    //            {
                    
    //                strx += str[k].ToString();
    //                if (k % 4 == 0)
    //                    strx += "";
    //            }
                
    //            //Debug.Log("i: " + i + " j: " + j + " " + BinToHex(strx));
    //            //preventTable += "0x" + BinToHex(strx) + ", ";
    //            //if (j == 7)
    //            //    preventTable += "\n";
    //            Debug.Log("i: " + i + " j: " + j + " " +  strx);
    //        }
    //    }

    //    Debug.Log(preventTable);
    //}

    //static string BinToHex(string bin)
    //{
    //    StringBuilder binary = new StringBuilder(bin);
    //    bool isNegative = false;
    //    if (binary[0] == '-')
    //    {
    //        isNegative = true;
    //        binary.Remove(0, 1);
    //    }

    //    for (int i = 0, length = binary.Length; i < (4 - length % 4) % 4; i++) //padding leading zeros
    //    {
    //        binary.Insert(0, '0');
    //    }

    //    StringBuilder hexadecimal = new StringBuilder();
    //    StringBuilder word = new StringBuilder("0000");
    //    for (int i = 0; i < binary.Length; i += 4)
    //    {
    //        for (int j = i; j < i + 4; j++)
    //        {
    //            word[j % 4] = binary[j];
    //        }

    //        switch (word.ToString())
    //        {
    //            case "0000": hexadecimal.Append('0'); break;
    //            case "0001": hexadecimal.Append('1'); break;
    //            case "0010": hexadecimal.Append('2'); break;
    //            case "0011": hexadecimal.Append('3'); break;
    //            case "0100": hexadecimal.Append('4'); break;
    //            case "0101": hexadecimal.Append('5'); break;
    //            case "0110": hexadecimal.Append('6'); break;
    //            case "0111": hexadecimal.Append('7'); break;
    //            case "1000": hexadecimal.Append('8'); break;
    //            case "1001": hexadecimal.Append('9'); break;
    //            case "1010": hexadecimal.Append('A'); break;
    //            case "1011": hexadecimal.Append('B'); break;
    //            case "1100": hexadecimal.Append('C'); break;
    //            case "1101": hexadecimal.Append('D'); break;
    //            case "1110": hexadecimal.Append('E'); break;
    //            case "1111": hexadecimal.Append('F'); break;
    //            default:
    //                return "Invalid number";
    //        }
    //    }

    //    if (isNegative)
    //    {
    //        hexadecimal.Insert(0, '-');
    //    }

    //    var str = hexadecimal.ToString();
    //    int index = 0;
    //    while (str[index] == '0')
    //    {
    //        index++;
    //    }
    //    return str.Substring((index == 0 ? (str[0] != '0' ? 0 : 1) : index), str.Length - index);
    //}
}

//8/8/2nN4/2N5/8/8/8/8