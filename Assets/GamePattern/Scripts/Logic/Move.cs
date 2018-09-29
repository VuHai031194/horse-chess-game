
/// <summary>
/// History move.
/// </summary>
public class HMove {
    public int move;
    public long pos;
    public int fiftyMove;

    public void Reset()
    {
        move = 0;
        pos = 0;
        fiftyMove = 0;
    }
}

/// <summary>
/// Standard move or search move.
/// </summary>
public struct SMove {
    public int move;
    public int score; //How good is this move?

}

public static class Move {

    //INT 32 
    //0000 0000 0000 0000 0000 0000 0011 1111 - FROM
    //0000 0000 0000 0000 0000 1111 1100 0000 - TO
    //0000 0000 0000 0000 1111 0000 0000 0000 - MOVING PIECE
    //0000 0000 0000 1111 0000 0000 0000 0000 - CAPTURE (piece)

    #region MethodsToGetMoveData
    public static int GetFrom(this int value) {
        return value & 0x3f; // 0x3f = 0000 0000 0011 1111 
    }


    public static int GetTo(this int value) {
        return (value >> 6) & 0x3f;
    }


    public static int GetPiece(this int value)
    {
        return (value >> 12) & 0xf;
    }


    public static int GetCapture(this int value)
    {
        return (value >> 16) & 0xf;
    }


    #endregion


    #region MoveCheckMethods

    public static int MovingPieceSide(this int value) {
        return value >> 24;
    }


    public static bool IsCapture(this int value) { 
        return (value & 0xf0000) != 0;
    }

    public static string PrintMove(this int move) {
        return ((Squares)move.GetFrom()).ToString().ToLower() + ((Squares)move.GetTo()).ToString().ToLower();
    }

    #endregion

}
