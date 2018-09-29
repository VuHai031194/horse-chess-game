using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class GUIPlay : MonoBehaviour
{

    public static GUIPlay main;

    #region UNITY_EDITOR
    public GameObject _board;
    public GameObject _horsesContain;
    public GameObject _curClick;
    public Text _txtScoreBlack, _txtScoreWhite;
    public Text _txtTimeWin, _txtTimeLose;
    public Text _txtTextWin;
    public Sprite _sprTrans, _sprRedHorse, _sprBlueHorse, _sprCurrent, _sprHint;
    public Button btnUndo;
    public Text txtRedName, txtBlueName;
    public Image imgRedAvatar;
    public Sprite sprCPU, sprYou;
    public GUIHighScore guiHighScore;
    #endregion

    private Transform[] _cellsTrans = null;
    private Transform[] _horses = null;

    private int firstPos = -1;
    private int secondPos = -1;

    private bool isSpecialMove = false;

    private Board board = null;

    private AIorNETJob aiOrNetJob = null;

    private int moveUndo = 0;



    void Awake()
    {
        main = this;
        InitCells();
    }

    void InitCells()
    {
        if (_cellsTrans == null)
        {
            int totalCell = Defs.BoardSize * Defs.BoardSize;
            _cellsTrans = new Transform[totalCell];

            for (int i = 0; i < totalCell; i++)
            {
                _cellsTrans[i] = _board.transform.GetChild(i);
            }
        }

        if (_horses == null)
        {
            int totalHorse = Defs.MaxChess * 2;
            _horses = new Transform[totalHorse];
            for (int i = 0; i < totalHorse; i++)
            {
                _horses[i] = _horsesContain.transform.GetChild(i);
            }
        }
    }

    void Update()
    {
        if (aiOrNetJob != null)
        {
            if (aiOrNetJob.Update())
            {
                // Alternative to the OnFinished callback
                aiOrNetJob = null;
            }
        }
    }

    public void Init(Board board)
    {
        // remove all hint in board and set default pos of horse
        InitBoard(board);
        // hidden cur click 
        SetCurClick();
        // Set default Score
        UpdateScore();

        moveUndo = 0;
        btnUndo.interactable = false;

        if (SceneManager.GetActiveScene().name.Equals("Game"))
        {
            board.SideToPlay = PlayerPrefs.GetInt("gofirst");
            //print("Side: " + board.SideToPlay + " Type: " + Defs.typePlay);
            if (board.SideToPlay == 0 && Defs.typePlay == TypePlay.PVC)
            {
                //StartCoroutine(ComMove());
                aiOrNetJob = new AIorNETJob();
                aiOrNetJob.Start();
            }

            if (Defs.typePlay == TypePlay.PVC)
            {
                txtRedName.text = "COM";
                txtBlueName.text = "YOU";
                imgRedAvatar.sprite = sprCPU;
            }
            else if (Defs.typePlay == TypePlay.PVP)
            {
                txtRedName.text = "RED";
                txtBlueName.text = "BLUE";
                imgRedAvatar.sprite = sprYou;
            }
        }
        else
        {
            board.SideToPlay = 1;

        } 
    }

    public void InitBoard(Board board)
    {

        //print("Call InitBoard");
        int totalCell = Defs.BoardSize * Defs.BoardSize;
        int countBlackHorse = 0;
        int countWhiteHorse = Defs.MaxChess;

        this.board = board;

        //Hidden Horse
        HiddenAllHorses();

        for (int i = 0; i < totalCell; i++)
        {

            UpdateCell(i, "trans");

            // init horses position
            if (board.pieces[i] == Defs.BKnight)
            {
                SetHorseVisible(countBlackHorse, true);
                //print("Blue Hourse: " + countBlueHorse);
                _horses[countBlackHorse].localPosition = _cellsTrans[i].localPosition;
                _cellsTrans[i].GetComponent<Cell>().IndexHorse = countBlackHorse;
                countBlackHorse++;

            }
            else if (board.pieces[i] == Defs.WKnight)
            {

                SetHorseVisible(countWhiteHorse, true);
                //print("Blue Hourse: " + countBlueHorse);
                _horses[countWhiteHorse].localPosition = _cellsTrans[i].localPosition;
                _cellsTrans[i].GetComponent<Cell>().IndexHorse = countWhiteHorse;
                countWhiteHorse++;
            }
        }

    }

    void UpdateCell(int indexCell, string typeCell)
    {
        if (typeCell.Equals("trans"))
        {
            _cellsTrans[indexCell].GetComponent<Image>().sprite = _sprTrans;
        }
        else if (typeCell.Equals("hint"))
        {
            _cellsTrans[indexCell].GetComponent<Image>().sprite = _sprHint;
        }
    }

    public void OnClickCell(int indexCell)
    {
        if (Defs.gameState == GameState.Moving)
            return;

        if (board.SideToPlay == 1)
            isSpecialMove = false;
        //print("value " + board.pieces[indexCell] + " side: " + board.SideToPlay + " cell: " + GetCell(indexCell));
        if (firstPos == -1) // neu chua dc click bao gio
        {
            if (2 - board.pieces[indexCell] == board.SideToPlay) // neu la quan cua ng dc click
            {
                firstPos = indexCell;
                SetCurClick(true, indexCell);
                SetHintCell(indexCell, true);
            }
        }
        else if (secondPos == -1)
        {
            if (2 - board.pieces[indexCell] == board.SideToPlay)
            {
                if (firstPos == indexCell)
                {
                    // Do nothing
                }
                else
                {
                    SetHintCell(firstPos, false);
                    firstPos = indexCell;
                    SetCurClick(true, indexCell);
                    SetHintCell(firstPos, true);
                }
            }
            else
            {
                secondPos = indexCell;
                int move = CheckMove();
                if (move != -1)
                {
                    SetCurClick();
                    SetHintCell(firstPos, false);
                    StartCoroutine(MoveHorse(firstPos, move));

                    if ((board.SideToPlay == 0 && (firstPos != 59 || firstPos != 60)) || (board.SideToPlay == 1 && (firstPos != 3 || firstPos != 4)))
                    {
                        btnUndo.interactable = true;
              
                        GUITime.main.SwitchTurn(1 - board.SideToPlay);
                    }
                }
                else
                {
                    secondPos = -1;
                }

            }
        }
    }

    void SetCurClick(bool isShow = false, int indexCell = 0)
    {
        if (isShow)
        {
            _curClick.transform.localPosition = _cellsTrans[indexCell].transform.localPosition;
            _curClick.GetComponent<Image>().enabled = true;
        }
        else
        {
            _curClick.GetComponent<Image>().enabled = false;
        }
    }

    int CheckMove()
    {
        SMove[] smoves = new SMove[256];
        int num = MoveGen.GenerateMoves(GameManager.main.board, smoves);

        for (int i = 0; i < num; i++)
        {
            //print(smoves[i].move.PrintMove());
            if (smoves[i].move.GetFrom() == firstPos && smoves[i].move.GetTo() == secondPos)
            {
                return smoves[i].move;
            }

        }
        return -1;
    }

    IEnumerator MoveHorse(int indexCell, int move)
    {
        Defs.gameState = GameState.Moving;

        //print("move:" + move.GetFrom());
        int speed = 10;
        int delta = 3;
        int indexHorse = GetCell(move.GetFrom());

        RemoveHorse(move);

        if (indexHorse != -1)
        {
            var horse = _horses[indexHorse];
            while (Vector3.Distance(horse.localPosition, _cellsTrans[move.GetTo()].localPosition) >= delta)
            {
                horse.localPosition = Vector3.Lerp(horse.localPosition, _cellsTrans[move.GetTo()].localPosition, speed * Time.deltaTime);
                yield return null;
            }

            SoundController.main.SoundPlay("move");

            horse.localPosition = _cellsTrans[move.GetTo()].localPosition;
            SetCell(move.GetTo(), indexHorse);
            SetCell(move.GetFrom(), -1);

            board.MakeMove(move);

            //print(FEN.FenFromBoard(board));

            // update score after move
            UpdateScore();

            // check win or lose
            if (CheckWin())
            {
                GUITime.main.isUpdateLogic = false;
                SoundController.main.PauseSound();
            }

            if (Defs.gameState == GameState.Moving)
            {
                Defs.gameState = GameState.Thinking;
            }

            // check if go to score cell
            SpeciallyMoveHorse(move);

            firstPos = secondPos = -1;
            //Debug.Log("Call AI Play");
            //if (indexCell != 3 && indexCell != 4 && indexCell != 59 && indexCell != 60)
            if (SceneManager.GetActiveScene().name.Equals("Game"))
            {
                if (board.SideToPlay == 0 && Defs.typePlay == TypePlay.PVC && !isSpecialMove)
                {
                    //StartCoroutine(ComMove());
                    //Debug.Log("Call AI Play");
                    aiOrNetJob = new AIorNETJob();
                    aiOrNetJob.Start();
                }
            }
        }
        else
        {
            Debug.LogError("Cell is Empty: " + move.GetFrom());
        }
    }

    public void ComMoveCall(int bestMove){
        StartCoroutine(ComMove(bestMove));
    }

    public IEnumerator ComMove(int bestMove)
    {
        //Debug.LogError("Cell is Empty: " + searchMove.PrintMove());
        yield return new WaitForSeconds(0.5f);
        OnClickCell(AI.main.bestMove.GetFrom());
        yield return new WaitForSeconds(0.5f);
        OnClickCell(AI.main.bestMove.GetTo());
        AI.main.bestMove = 0;
    }

    void SpeciallyMoveHorse(int move)
    {
        int piece = move.GetPiece();
        if (piece == Defs.WKnight)
        {
            if (move.GetTo() == 3)
            {
                SpeciallyMoveHorse(3, 56);
            }
            else if (move.GetTo() == 4)
            {
                SpeciallyMoveHorse(4, 63);
            }
        }
        else
        {
            if (piece == Defs.BKnight)
            {
                if (move.GetTo() == 59)
                {
                    SpeciallyMoveHorse(59, 0);
                }
                else if (move.GetTo() == 60)
                {
                    SpeciallyMoveHorse(60, 7);
                }
            }
        }
    }

    void SpeciallyMoveHorse(int from, int to)
    {
        if(from == 59 || from == 60)
            isSpecialMove = true;
        //print("from: " + from + " to: " + to);
        board.SideToPlay = 1 - board.SideToPlay;
        int moveSpecially = (from | (to << 6) | (board.GetPieceAt(from) << 12) | (board.GetPieceAt(to) << 16));
        //board.MakeMove(move);

        //print(moveSpecially.GetFrom());
        StartCoroutine(MoveHorse(from, moveSpecially));
    }

    bool CheckWin()
    {
        if (board.scoreWhite >= Defs.ScoreWin)
        {
            GUIEvents.main.RunForward("Win");
            _txtTimeWin.text = Defs.ConvertNumToTime(Mathf.FloorToInt(GUITime.main.countTotalTime));
            if (Defs.typePlay == TypePlay.PVC)
            {
                guiHighScore.UpdateHighScore(PlayerPrefs.GetString("nameplayer"), Mathf.FloorToInt(GUITime.main.countTotalTime));
                _txtTextWin.text = "YOU WIN";
            }
            else if(Defs.typePlay == TypePlay.PVP)
            {
                _txtTextWin.text = "BLUE WIN";
            }
            return true;
        }
        else if (board.scoreBlack >= Defs.ScoreWin)
        {
            GUIEvents.main.RunForward("Lose");
            _txtTimeLose.text = Defs.ConvertNumToTime(Mathf.FloorToInt(GUITime.main.countTotalTime));
            if (Defs.typePlay == TypePlay.PVC)
            {
                _txtTextWin.text = "YOU LOSE";
            }
            else if (Defs.typePlay == TypePlay.PVP)
            {
                _txtTextWin.text = "RED WIN";
            }
            //    guiHighScore.UpdateHighScore(PlayerPrefs.GetString("nameplayer"), Mathf.FloorToInt(GUITime.main.countTotalTime));
            return true;
        }

        return false;
    }


    #region Complete Function
    void SetHintCell(int from, bool isHint)
    {
        //print("from: " + from);
        SMove[] smoves = new SMove[256];
        int num = MoveGen.GenerateMoves(GameManager.main.board, smoves);

        for (int i = 0; i < num; i++)
        {
            //print(smoves[i].move.PrintMove());
            if (smoves[i].move.GetFrom() == from)
            {
                if (isHint)
                {
                    UpdateCell(smoves[i].move.GetTo(), "hint");
                }
                else
                {
                    UpdateCell(smoves[i].move.GetTo(), "trans");
                }
            }

        }
    }
    void HiddenAllHorses()
    {
        int total = Defs.MaxChess * 2;
        for (int i = 0; i < total; i++)
        {
            SetHorseVisible(i, false);
        }
    }

    void SetHorseVisible(int indexHorse, bool visible)
    {
        //print("visi: " + indexHorse);
        _horses[indexHorse].GetComponent<Image>().enabled = visible;
    }

    void UpdateScore()
    {
        _txtScoreBlack.text = board.scoreBlack.ToString();
        _txtScoreWhite.text = board.scoreWhite.ToString();
    }

    void RemoveHorse(int move)
    {
        if (board.pieces[move.GetTo()] != 0)
        {
            SetHorseVisible(GetCell(move.GetTo()), false);
        }
    }

    void SetCell(int indexCell, int indexHorse)
    {
        _cellsTrans[indexCell].GetComponent<Cell>().IndexHorse = indexHorse;
    }

    int GetCell(int indexCell)
    {
        return _cellsTrans[indexCell].GetComponent<Cell>().IndexHorse;
    }

    public void OnClickUndo()
    {
        if (board.SideToPlay == 1 && Defs.gameState == GameState.Thinking)
        {
            moveUndo = board.UndoMove(); // move lan di chuyen cua minh
            UndoSpecialMove(moveUndo);
            if (board.SideToPlay == 0)
            {
                moveUndo = board.UndoMove();
                UndoSpecialMove(moveUndo);
            }

            // Dang lam undo
            InitBoard(board);

            GUITime.main.countTime = 0;

            btnUndo.interactable = false;
        }
        
    }

    public void UndoSpecialMove(int move)
    {
        if ((move.GetFrom() == 59 && move.GetTo() == 0)||
            (move.GetFrom() == 60 && move.GetTo() == 7)||
            (move.GetFrom() == 3 && move.GetTo() == 54)||
            (move.GetFrom() == 4 && move.GetTo() == 63))
        {
            board.UndoMove();
        }
    }

    #endregion

}
