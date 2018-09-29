using UnityEngine;
using System.Collections;

/// <summary>
/// Computer chess analayzing.
/// </summary>
public class AI : MonoBehaviour
{
    public static AI main;
    private Board board;
    public int thinkingLevel;
    private SMove[] Moves;

    //private int NodesEvaluated;
    //private int QNodes;

    //private Stopwatch Watch;

    //private float FailHigh;
    //private float FailHighFirst;

    //private bool RunOutOfTime;
    //private int count = 0;
    public int bestMove = 0;
    int ply = 5;
    int count = 0;

    void Awake()
    {
        main = this;
    }

    public void Init(Board board)
    {
        this.board = board;
        Moves = new SMove[Defs.MaxMoves * 64];
        thinkingLevel = PlayerPrefs.GetInt("thinkinglevel");

        switch (thinkingLevel)
        {
            case 1: ply = 3;
                Defs.ThinkingTime = 30;
                break;
            case 2: ply = 5;
                Defs.ThinkingTime = 45;
                break;
            case 3: ply = 7;
                Defs.ThinkingTime = 60;
                break;
            default: ply = 3;
                Defs.ThinkingTime = 30;
                break;
        }
        
    }


    //Iterative depending, search init
    public int SearchPosition()
    {
        
        int num = 1;
            
        //ply = 1;
        switch (num)
        {
            case 0: Negamax(-Defs.MaxValue, Defs.MaxValue, ply); break;
            case 1: PVS(-Defs.MaxValue, Defs.MaxValue, ply); break;
            case 2: MtdF(ply, -Defs.MaxValue); break;
            case 3: TestMemoryEnhance(ply, 0, -Defs.MaxValue); break;
            case 4: AlphaBetaMax(-Defs.MaxValue, Defs.MaxValue, ply); break;
            case 5: AlphaBetaNegascout(-Defs.MaxValue, Defs.MaxValue, 0); break;
        }

        return bestMove;

        //UnityEngine.Debug.Log("Count: " + count);
      
    }

    /// <summary>
    /// Negamax alpha beta recursion.
    /// </summary>
    public int Negamax(int alpha, int beta, int depth)
    {
        if (depth == 0 || IsEndGame())
        {
            return Quiescence(alpha, beta, 63); //Try to find calm position
        }

        int bestScore = -Defs.MaxValue;
        //int MadeALegalMove = 0;

        int add = depth * Defs.MaxMoves;
        int num = MoveGen.GenerateMoves(board, Moves, add, depth) + add;
        int move;

        for (int i = add; i < num; i++)
        {
            PickBestMove(i, num);
            move = Moves[i].move;

            
            board.MakeMove(move);
            SpecialMove(move);
            //if (depth == ply)
            //{
            //    UnityEngine.Debug.Log("Alpha " + i.ToString() + ": " + move.PrintMove() + " " + Moves[i].score + " " + depth);
            //}
            int score = -Negamax(-beta, -alpha, depth - 1);

            UndoMove(move);

            if (score > bestScore)
            {
                bestScore = score;
                if (depth == ply)
                    bestMove = move;
                if (bestScore > alpha)
                {
                    alpha = bestScore;
                }
            }

            if (bestScore >= beta)
            {
                return bestScore;
            }

        }

        return bestScore;
              
    }

    private void UndoMove(int move)
    {
        int To = move.GetTo();
        if ((board.SideToPlay == 1 && (To == 59 || To == 60)) || ((board.SideToPlay == 0) && (To == 3 || To == 4)))
        {
            board.UndoMove();
            board.SideToPlay = 1 - board.SideToPlay;
        }
        board.UndoMove();
    }

    /// <summary>
    /// Searches only capturing moves.
    /// </summary>
    ///

    public int Quiescence(int alpha, int beta, int depth)
    {
        int score = (Evaluation.Evaluate(board) * ((board.SideToPlay * 2) - 1));
        count++;
        if(alpha == 0 && beta == 0 && depth == 0)
            return score;
        if (IsEndGame())
            return score;

        if (score >= beta)
            return beta;

        if (alpha < score)
            alpha = score;

        int add = depth * Defs.MaxMoves;
        int num = MoveGen.GenerateCapturingMoves(board, Moves, add) + add;
        int move;

        for (int i = add; i < num; i++)
        {
            PickBestMove(i, num);
            move = Moves[i].move;

            board.MakeMove(move);
            SpecialMove(move);
            
            //UnityEngine.Debug.Log("Quies " + i.ToString() + ": " + move.PrintMove() + " " + Moves[i].score + " " + depth);
            
            int bestScore = -Quiescence(-beta, -alpha, depth - 1);
            
            UndoMove(move);

            if (bestScore >= beta)
            {
                return beta;
            }

            if (bestScore > alpha)
                alpha = bestScore;
        }

        return alpha;
    }

    /// <summary>
    /// Picks the best move from the list
    /// </summary>
    public void PickBestMove(int pos, int max)
    {
        SMove temp;
        int bestScore = -1;
        int bestIndex = pos;
        for (int i = pos; i < max; i++)
        {
            if (Moves[i].score > bestScore)
            {
                //UnityEngine.Debug.Log("Moves: " + Moves[i].score);
                bestIndex = i;
                bestScore = Moves[i].score;
            }
        }
        //Set it at the bottom
        temp = Moves[pos];
        Moves[pos] = Moves[bestIndex];
        Moves[bestIndex] = temp;
    }

    /// Get Score when play score slot
    /// 

    public void SpecialMove(int move)
    {
        if (board.SideToPlay == 1) // cac luot di cua black
        {
            if (Move.GetTo(move) == 59) // an o 1 diem
            {
                int moveSpecial = (59 | (0 << 6) | (Defs.BKnight << 12) | (board.GetPieceAt(0) << 16));
                board.SideToPlay = 1 - board.SideToPlay;
                board.MakeMove(moveSpecial);
            }
            else if (Move.GetTo(move) == 60) // an o 2 diem
            {
                //UnityEngine.Debug.Log("Check");
                int moveSpecial = (60 | (7 << 6) | (Defs.BKnight << 12) | (board.GetPieceAt(7) << 16));
                board.SideToPlay = 1 - board.SideToPlay;
                board.MakeMove(moveSpecial);
            }


        }
        else // cac luot di cua white
        {
            if (Move.GetTo(move) == 4) // an o 1 diem
            {
                int moveSpecial = (4 | (63 << 6) | (Defs.WKnight << 12) | (board.GetPieceAt(63) << 16));
                board.SideToPlay = 1 - board.SideToPlay;
                board.MakeMove(moveSpecial);
            }
            else if (Move.GetTo(move) == 3) // an o 2 diem
            {
                int moveSpecial = (3 | (56 << 6) | (Defs.WKnight << 12) | (board.GetPieceAt(56) << 16));
                board.SideToPlay = 1 - board.SideToPlay;
                board.MakeMove(moveSpecial);
            }
        }
    }


    public bool IsEndGame()
    {
        if (board.scoreWhite >= Defs.ScoreWin || board.scoreBlack >= Defs.ScoreWin)
            return true;
        return false;
    }

    /*
     * Cai dat them
     */

    #region Principal Variation Search
    /// <summary>
    /// Negacout
    /// </summary>
    /// <param name="alpha"></param>
    /// <param name="beta"></param>
    /// <param name="depth"></param>
    /// <returns></returns>
    /* Speusodocode
     * 
     * (* Negascout is also termed Principal Variation Search - hence - pvs *)

        function pvs(node, depth, α, β, color)
            if node is a terminal node or depth = 0
                 return color × the heuristic value of node
            for each child of node
                if child is not first child
                     score := -pvs(child, depth-1, -α-1, -α, -color)       (* search with a null window *)
                     if α < score < β                                      (* if it failed high,
                        score := -pvs(child, depth-1, -β, -score, -color)        do a full re-search *)
                else
                    score := -pvs(child, depth-1, -β, -α, -color)
                 α := max(α, score)
                if α ≥ β
                    break                                            (* beta cut-off *)
            return α
     */ 
    
    public int PVS(int alpha, int beta, int depth)
    {
        if (depth == 0 || IsEndGame())
        {
            return Quiescence(alpha, beta, 63); // //Try to find calm position
        }

        int add = depth * Defs.MaxMoves;
        int num = MoveGen.GenerateMoves(board, Moves, add, depth) + add;
        int move;

        for (int i = add; i < num; i++)
        {
            PickBestMove(i, num);
            move = Moves[i].move;
            board.MakeMove(move);
            SpecialMove(move);
            int score = 0;
            if (i != add)
            {
                score = -PVS(-alpha - 1, -alpha, depth - 1);
                if (alpha < score && score < beta)
                    score = -PVS(-beta, -score, depth - 1);
            }
            else
            {
                score = -PVS(-beta, -alpha, depth - 1);
            }
            
            UndoMove(move);

            if (score > alpha)
            {
                alpha = score;
                if (depth == ply)
                    bestMove = move;
            }
            if (alpha >= beta)
                break;
        }
        return alpha;
    }
    #endregion

    #region AlphaBeta Search
    /// <summary>
    /// Alpha Beta Prunning
    /// </summary>
    /// <param name="alpha"></param>
    /// <param name="beta"></param>
    /// <param name="depth"></param>
    /// <returns></returns>
    /// 
    /*
    int alphaBetaMax(int alpha, int beta, int depthleft) {
           if ( depthleft == 0 ) return evaluate();
           for ( all moves) {
              score = alphaBetaMin( alpha, beta, depthleft - 1 );
              if( score >= beta )
                 return beta;   // fail hard beta-cutoff
              if( score > alpha )
                 alpha = score; // alpha acts like max in MiniMax
           }
           return alpha;
     }

      int alphaBetaMin(int alpha, int beta, int depthleft) {
           if ( depthleft == 0 ) return -evaluate();
           for ( all moves) {
              score = alphaBetaMax( alpha, beta, depthleft - 1 );
              if( score <= alpha )
                 return alpha; // fail hard alpha-cutoff
              if( score < beta )
                 beta = score; // beta acts like min in MiniMax
           }
           return beta;
       }
     */
    public int AlphaBetaMax(int alpha, int beta, int depth)
    {
        if (depth == 0 || IsEndGame())
        {
            return Quiescence(alpha, beta, 63);
        }

        int add = depth * Defs.MaxMoves;
        int num = MoveGen.GenerateMoves(board, Moves, add, depth) + add;
        int move;

        for (int i = add; i < num; i++)
        {
            PickBestMove(i, num);
            move = Moves[i].move;

            board.MakeMove(move);
            SpecialMove(move);

            int score = AlphaBetaMin(alpha, beta, depth - 1);

            UndoMove(move);

            if (score >= beta)
                return beta;
            if (score > alpha)
            {
                alpha = score;
                if (ply == depth)
                    bestMove = move;
            }
        }
        return alpha;
    }

    public int AlphaBetaMin(int alpha, int beta, int depth){

        if (depth == 0 || IsEndGame())
        {
            return -Quiescence(alpha, beta, 63);
        }

        int add = depth * Defs.MaxMoves;
        int num = MoveGen.GenerateMoves(board, Moves, add, depth) + add;
        int move;

        for (int i = add; i < num; i++)
        {
            PickBestMove(i, num);
            move = Moves[i].move;

            board.MakeMove(move);
            SpecialMove(move);

            int score = AlphaBetaMax(alpha, beta, depth - 1);

            UndoMove(move);

            if (score <= alpha)
                return alpha;
            if (score < beta)
            {
                beta = score;
            }
        }
        return alpha;
    }
    #endregion

    public int AlphaBetaNegascout(int alpha, int beta, int currentDepth)
    {
        // check if we're done recursing
        if (currentDepth == ply || IsEndGame())
        {
            return Quiescence(alpha, beta, 63);
        }

        // Otherwise bubble up values from below
        int bestScore = -Defs.MaxValue;

        // Keep track of the Test window value
        int adaptiveBeta = beta;
        // Go through each move
        int add = currentDepth * Defs.MaxMoves;
        int num = MoveGen.GenerateMoves(board, Moves, add, currentDepth) + add;
        int move;

        for (int i = add; i < num; i++)
        {
            PickBestMove(i, num);
            move = Moves[i].move;

            board.MakeMove(move);
            SpecialMove(move);

            int recursedScore = AlphaBetaNegascout(-adaptiveBeta, -Mathf.Max(alpha, bestScore), currentDepth + 1);

            int currentScore = -recursedScore;
            // Update the best score
            if (currentScore > bestScore)
            {
                // If we are in 'narrow-mode' then widen and
                // do a regular AB negamax search
                if (adaptiveBeta == beta || currentDepth >= ply - 2)
                {
                    if (currentDepth == 0)
                    {
                        this.bestMove = move;
                        UnityEngine.Debug.Log("Move: " + this.bestMove.PrintMove());
                    }
                    bestScore = currentScore;
                    
                }
                // Otherwise we can do a Test
                else
                {
                    int negativeBestScore = -AlphaBetaNegascout(-beta, -currentScore, currentDepth);
                    bestScore = -negativeBestScore;
                }
                
                // If we 're outside the bounds, then prune: exit immediately
                if (bestScore >= beta)
                {
                    UndoMove(move);
                    return bestScore;
                }

                // Otherwise update the window location
                adaptiveBeta = Mathf.Max(alpha, bestScore) + 5;
            }

            UndoMove(move);
        }
        return bestScore;
    }

    public TranspositionTable table = new TranspositionTable();

    public int TestMemoryEnhance(int maxDepth, int curDepth, int gamma)
    {
        int lowestDepth = 0;
        if (curDepth > lowestDepth) lowestDepth = curDepth;

        // Lookup the entry from the transposition talbe
        var entry = table.GetEntry(board.Position);
        if (entry != null && entry.depth > maxDepth - curDepth)
        {
            // Early outs for stored positions
            if (entry.minScore > gamma)
                return entry.minScore;
            if (entry.maxScore < gamma)
                return entry.maxScore;
        }
        else
        {
            entry = new TableEntry();
            // We need to create the entry
            entry.hashValue = board.Position;
            entry.depth = maxDepth - curDepth;
            entry.minScore = -Defs.MaxValue;
            entry.maxScore = -Defs.MaxValue;
        }

        // Now we have the entry, we can get on with the text

        // Check if we're done recursing
        if (IsEndGame() || curDepth == maxDepth)
        {
            entry.minScore = entry.maxScore = Quiescence(0, 0, 0);
            table.StoreEntry(entry);
            return entry.minScore;
        }

        // Now go into bubbling up mode
        int bestMove = 0;
        int bestScore = -Defs.MaxValue;

        // Go through each move
        int add = curDepth * Defs.MaxMoves;
        int num = MoveGen.GenerateMoves(board, Moves, add, curDepth) + add;
        int move;

        for (int i = add; i < num; i++)
        {
            PickBestMove(i, num);
            move = Moves[i].move;

            board.MakeMove(move);
            SpecialMove(move);

            int recursedScore = TestMemoryEnhance(maxDepth, curDepth + 1, -gamma);

            UndoMove(move);
            int currentScore = -recursedScore;

            // Update the best score
            if (currentScore > bestScore)
            {
                // Track the current best move
                entry.bestMove = move;

                bestScore = currentScore;
                bestMove = move;
                if (curDepth == 0)
                {
                    this.bestMove = bestMove;
                }
            }
        }

        // If we pruned, the we have a min score, otherwise
        // we have a max score.
        if (bestScore < gamma)
        {
            entry.maxScore = bestScore;
        }
        else
        {
            entry.minScore = bestScore;
        }

        // Store the entry and return the best score and move.
        table.StoreEntry(entry);
        return bestScore;

    }

    public int MtdF(int maxDepth, int guess)
    {
        for (int i = 0; i < maxDepth; i++)
        {
            int gamma = guess;
            guess = TestMemoryEnhance(maxDepth, i, gamma - 1);

            // If there's no more improvement, stop looking
            if(gamma == guess)
                break;
        }

        return 0;
    }
}

