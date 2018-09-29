using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum BoardState { 
    InProgress,
    BlackMate,
    WhiteMate,
    StaleMate,
    Repetition,
}

public sealed class Board {

	//64 bit ulong for each square and piece
	//Two elements in an array for black(0) and white(1) side
    public ulong WKnight, BKnight;

    /// <summary>
    /// Pieces on board.
    /// </summary>
    public int[] pieces;

    /// <summary>
    /// Position hash.
    /// </summary>
    public long Position;

	/// <summary>
	/// Current side to play.
	/// </summary>
	public int SideToPlay;


    /// <summary>
    /// Fifty move rule.
    /// </summary>
    public int FiftyMove;

	/// <summary>
	/// History moves.
	/// </summary>+ 
	public HMove[] History;
	public int ply = 0;

    /// <summary>
    /// Principal variatian.
    /// </summary>
    public PVTable PvTable;
    public int[] PvArray;

    public int[][] SearchHistory;
    public int[][] SearchKillers;

    public int scoreBlack = 0;
    public int scoreWhite = 0;
    

	/// <summary>
	/// Construct board based on FEN.
	/// </summary>
	public Board(string Fen)
	{

        pieces = new int[64]; //Every square for every piece
        PvArray = new int[Defs.MaxDepth];

        History = new HMove[2056];
        for (int i = 0; i < History.Length; i++)
        {
            History[i] = new HMove();
        }
		//Init board from FEN
        Init();

	}

    public void Reset()
    {
        int i;
        for (i = 0; i < Defs.BoardSize * Defs.BoardSize; i++)
            pieces[i] = 0;

        for (i = 0; i < Defs.MaxDepth; i++)
            PvArray[i] = 0;

        for (i = 0; i < History.Length; i++)
            History[i].Reset();

        InitPVTable();

        SearchHistory = new int[14][];
        SearchKillers = new int[2][];
        for (i = 0; i < 14; i++)
        {
            SearchHistory[i] = new int[64];
        }

        SearchKillers[0] = new int[Defs.MaxDepth];
        SearchKillers[1] = new int[Defs.MaxDepth];

    }

    public void Init()
    {
        Reset();
        this.BoardFromFen(GameManager.main.Fen == "" ? FEN.Default : GameManager.main.Fen);
        //Get hash key
        Position = Zobrist.GetHashPosition(this);
        // set Ply
        ply = 0;
        // set fifty move
        FiftyMove = 0;
        // set sideToplay
        SideToPlay = 1;

        
    }

    public void InitPVTable(float TableSizeInMB = 2.0f)
    {
        int size = System.Runtime.InteropServices.Marshal.SizeOf(typeof(PVEntry));
        int numOfElements = (int)((TableSizeInMB * (1024f * 1024f)) / size);
        //Debug.Log(numOfElements);    
        PvTable = new PVTable() { data = new PVEntry[numOfElements], numEntries = numOfElements };
    }


    public void ClearPVTable()
    {
        for (int i = 0; i < PvTable.data.Length; i++)
        {
            PvTable.data[i].hash = 0L;
            PvTable.data[i].move = 0;
        }
    }

    /// <summary>
    /// Stores a move at the board hash position.
    /// </summary>
    public void StorePVMove(int move)
    {
        int index = (int)(Position % PvTable.numEntries);
        if (index >= 0 && index < PvTable.numEntries)
        {
            PvTable.data[index].move = move;
            PvTable.data[index].hash = Position;
        }
    }

    /// <summary>
    /// Check if the move exsist at the board.
    /// </summary>
    public bool MoveExists(int move) {
        int num = MoveGen.GenerateMoves(this, CheckingMoves);
        //For every move
        for (int i = 0; i < num; i++)
        {
            int myMove = CheckingMoves[i].move;
            
            MakeMove(myMove);

            if (myMove == move)
            {
                UndoMove();
                return true;
            }
         
            UndoMove();
        }
        return false;
    }

    /// <summary>
    /// Get the move at this hash position.
    /// </summary>
    public int ProbePVTable()
    {
        int index = (int)(Position % PvTable.numEntries);

        if (index >= 0 && index < PvTable.numEntries)
        {

            if (PvTable.data[index].hash == Position)
            {
                return PvTable.data[index].move;
            }
        }

        return 0;
    }

    /// <summary>
    /// Gets the PV line.
    /// </summary>
    public int GetPVLine(int depth)
    {

        int move = ProbePVTable();
        int count = 0;

        while (move != 0 && count < depth)
        {

            if (MoveExists(move))
            {
                //Debug.Log("GetPVLine");
                MakeMove(move);
                //Debug.Log("Count: " + count);
                PvArray[count] = move;
                count++;
            }
            else
            {
                break;
            }

            move = ProbePVTable();
        }
        int temp = count;
        while (temp > 0)
        {
            UndoMove();
            temp--;
        }
        return count;
    }


	/// <summary>
	/// Gets the piece at given side in the given square.
	/// </summary>
	public int GetPieceAt(int index)
	{
        return pieces[index];
	}

	/// <summary>
	/// Makes a move.
	/// </summary>
	public void MakeMove(int move){
        HMove hisMove = History[ply];
        hisMove.move = move;
        hisMove.pos = Position;
        hisMove.fiftyMove = FiftyMove;

        //increase it, but it may reset
        FiftyMove++;

        int From = move & 0x3f;
        int To = (move >> 6) & 0x3f;
        int movePiece = (move >> 12) & 0xf;

        // print move infor
        //Debug.Log(move.PrintMove());
		switch(movePiece)
        {
            #region WHITE
            
            case Defs.WKnight:
                //Debug.Log("White");
                if (From == 3 || From == 4)
                {
                    WKnight ^= Ops.Pow2[From];

                    Position ^= Zobrist.Pieces[From][Defs.WKnight];
                    //Debug.Log("Position Before: " + Position);
                    if (pieces[To] != Defs.WKnight)
                    {
                        WKnight ^= Ops.Pow2[To];
                        Position ^= Zobrist.Pieces[To][Defs.WKnight];
                    }
                 
                }
                else
                {
                    WKnight ^= Ops.Pow2[From];
                    WKnight ^= Ops.Pow2[To];

                    Position ^= Zobrist.Pieces[From][Defs.WKnight];
                    Position ^= Zobrist.Pieces[To][Defs.WKnight];

                    if (To == 3)
                    {
                        Position ^= Zobrist.Scores[1][scoreWhite];
                        //scoreWhite += 2;
                        UpdateScore(1);
                        //Debug.Log("Score: " + scoreWhite);
                        Position ^= Zobrist.Scores[1][scoreWhite];
                    }
                    else if (To == 4)
                    {
                        Position ^= Zobrist.Scores[1][scoreWhite];
                        //scoreWhite += 1;
                        UpdateScore(2);
                        Position ^= Zobrist.Scores[1][scoreWhite];
                    }

                }

                //Debug.Log("Position Normal: " + Position);
                pieces[From] = 0;
                pieces[To] = Defs.WKnight;

			break;
            
            #endregion

            #region BLACK
            
            case Defs.BKnight:
            
            if (From == 59 || From == 60)
            {
                BKnight ^= Ops.Pow2[From];
                

                Position ^= Zobrist.Pieces[From][Defs.BKnight];
                //Debug.Log("Position Before: " + Position);
                if (pieces[To] != Defs.BKnight)
                {
                    BKnight ^= Ops.Pow2[To];
                    Position ^= Zobrist.Pieces[To][Defs.BKnight];
                }
            }
            else
            {
                BKnight ^= Ops.Pow2[From];
                BKnight ^= Ops.Pow2[To];

                Position ^= Zobrist.Pieces[From][Defs.BKnight];
                Position ^= Zobrist.Pieces[To][Defs.BKnight];

                if (To == 60)
                {
                    Position ^= Zobrist.Scores[0][scoreBlack];
                    //scoreBlack += 2;
                    UpdateScore(1);
                    Position ^= Zobrist.Scores[0][scoreBlack];
                   
                }
                else if (To == 59)
                {
                    Position ^= Zobrist.Scores[0][scoreBlack];
                    //scoreBlack += 1;
                    UpdateScore(2);
                    Position ^= Zobrist.Scores[0][scoreBlack];
                }

            }
            
            pieces[From] = 0;
            pieces[To] = Defs.BKnight;
            //Debug.Log("Position Normal: " + Position);

            break;
            
            #endregion
        }

        if ((move & 0xf0000) != 0) //Move was capture
        {
            //Debug.Log("Move: " + move.PrintMove());
            #region capture

            int capturePiece = ((move >> 16) & 0xf);
            if (movePiece != capturePiece)
            {
                //Debug.Log("Move: " + move.PrintMove());
                switch (capturePiece)
                {

                    #region white

                    case Defs.WKnight:
                        //if (movePiece != Defs.WKnight)
                        //{
                        WKnight ^= Ops.Pow2[To];
                        Position ^= Zobrist.Pieces[To][Defs.WKnight];

                        Position ^= Zobrist.Scores[0][scoreBlack];
                        //scoreBlack += 1;
                        UpdateScore(1);
                        //Debug.Log("Score: " + scoreBlack);
                        Position ^= Zobrist.Scores[0][scoreBlack];
                        //}

                        break;
                    #endregion
                    #region black

                    case Defs.BKnight:
                        //if (movePiece != Defs.BKnight)
                        //{
                        BKnight ^= Ops.Pow2[To];
                        Position ^= Zobrist.Pieces[To][Defs.BKnight];
               
                        Position ^= Zobrist.Scores[1][scoreWhite];
                        //scoreWhite += 1;
                        UpdateScore(1);
                        Position ^= Zobrist.Scores[1][scoreWhite];
                 
                        //}

                        break;
                    #endregion

                }
            }
            
            #endregion

            //Reset fifty move
            FiftyMove = 0;
        }

		    SideToPlay = 1-SideToPlay;

		History[ply] = hisMove;
		ply ++;

        //Debug.Log("End score: " + scoreBlack);
	}

    // truong hop an quan nhung da co mot quan o nha la quan cua minh
	public int UndoMove(){

		if(ply == 0) //No moves to undo
			return 0;

		ply--;

        SideToPlay = 1 - SideToPlay;

		HMove hisMove = History[ply];
        int move = hisMove.move;

        int From = move & 0x3f;
        int To = (move >> 6) & 0x3f;
        int movePiece = (move >> 12) & 0xf;
        int capturePiece = (move >> 16) & 0xf;

        switch (movePiece)
        {

            #region WHITE
            case Defs.WKnight:
                WKnight ^= Ops.Pow2[From];

                if (capturePiece != Defs.WKnight)
                {
                    WKnight ^= Ops.Pow2[To];
                    pieces[To] = 0;
                }
                
                pieces[From] = Defs.WKnight;

                if (To == 3)
                {
                    Position ^= Zobrist.Scores[1][scoreWhite];
                    UpdateScore(-1);
                    Position ^= Zobrist.Scores[1][scoreWhite];
                }
                else if (To == 4)
                {
                    Position ^= Zobrist.Scores[1][scoreWhite];
                    UpdateScore(-2);
                    Position ^= Zobrist.Scores[1][scoreWhite];
                }

                break;
            
            #endregion

            #region BLACK
           
            case Defs.BKnight:
                BKnight ^= Ops.Pow2[From];

                if (capturePiece != Defs.BKnight)
                {
                    BKnight ^= Ops.Pow2[To];
                    pieces[To] = 0;
                }
                
                pieces[From] = Defs.BKnight;

                if (To == 60) {
                    Position ^= Zobrist.Scores[0][scoreBlack];
                    UpdateScore(-1);
                    Position ^= Zobrist.Scores[0][scoreBlack];
                }
                else if (To == 59) {
                    Position ^= Zobrist.Scores[0][scoreBlack];
                    UpdateScore(-2);
                    Position ^= Zobrist.Scores[0][scoreBlack];
                }
                break;
             #endregion
        }

        if ((move & 0xf0000) != 0 )
        {
            #region capture
            switch (capturePiece)
            {
                #region white
               
                case Defs.WKnight:
                    if (movePiece != Defs.WKnight)
                    {
                        WKnight ^= Ops.Pow2[To];
                        pieces[To] = Defs.WKnight;

                        Position ^= Zobrist.Scores[0][scoreBlack];
                        UpdateScore(-1);
                        Position ^= Zobrist.Scores[0][scoreBlack];
                    }
                    
                    break;
                
                #endregion
                #region black
                
                case Defs.BKnight:
                    if (movePiece != Defs.BKnight)
                    {
                        BKnight ^= Ops.Pow2[To];
                        pieces[To] = Defs.BKnight;

                        Position ^= Zobrist.Scores[1][scoreWhite];
                        UpdateScore(-1);
                        Position ^= Zobrist.Scores[1][scoreWhite];
                    }
                    
                    break;
                
                #endregion

            }
            #endregion   
        
        }

        Position = hisMove.pos;
        FiftyMove = hisMove.fiftyMove;

        return move;
	}

    public void UpdateScore(int value)
    {
        if (SideToPlay == 0)
        {
            scoreBlack += value;
        }
        else if(SideToPlay == 1)
        {
            scoreWhite += value;
        }

    }
    /// <summary>
    /// This should be called when move should be checked for mate, stale mate and other rules. Always!
    /// </summary>
    private SMove[] CheckingMoves = new SMove[Defs.MaxMoves];
    
	/// <summary>
	/// Returns true if the position BB is attacked by the given side.
	/// </summary>
    public bool IsAttacked(ulong pos, int attackedBySide)
    {


        while (pos != 0)
        {

            //Square index
            int index = Ops.FirstBit(pos);
            pos ^= Ops.Pow2[index];

            return true;
        }


        return false;
    }

    public bool MoveWasIllegal()
    {
        switch (SideToPlay)
        {
            case 1:
                if (scoreWhite >= Defs.ScoreWin)
                    return true;

                return false;
            case 0:
                if (scoreBlack >= Defs.ScoreWin)
                    return true;

                return false;
            default:
                return false;
        }

    }

    public bool IsRepetition()
    {
        int found = 0;
        for (int i = ply - 1; i > ply - FiftyMove; i--)
        {
            if (History[i].pos == Position)
            {
                found++;
                if (found >= 3)
                    return true;
            }
        }
        return false;
    }


    public void PrintPieces(){
        string str = "";
        for(int i = 0; i < 64; i++)
        {
            str += pieces[i].ToString() + " ";
            if ((i + 1) % 8 == 0)
                str += "\n";
        }
        Debug.Log(str);
    }
}
