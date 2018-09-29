using UnityEngine;
using System.Collections;
#if UNITY_5_3
using UnityEngine.SceneManagement;
#endif

public enum GameState
{
    Map,
    PrepareGame,
    Moving,
    Playing,
    Highscore,
    GameOver,
    Pause,
    Win,
    Thinking
}

public class GameManager : MonoBehaviour {

    public static GameManager main;
    public static GameManager Instance;

    public string Fen;
    //public bool FacebookEnable;

    [HideInInspector]
    public Board board;

    void Awake()
    {
        main = this;

        if (PlayerPrefs.GetInt("settinginit") == 0)
        {
            PlayerPrefs.SetString("nameplayer", "Noname");
            PlayerPrefs.SetInt("thinkinglevel", 1);
            PlayerPrefs.SetInt("gofirst", 1);
            PlayerPrefs.SetInt("settinginit", 1);
        }
    }

    void OnEnable()
    {
        Zobrist.Init();
        board = new Board(Fen == "" ? FEN.Default : Fen);
#if UNITY_5_3
        if (SceneManager.GetActiveScene().name.Equals("Game"))
#else
        if (Application.loadedLevelName.Equals("Game"))
#endif
            FEN.Default = "nnn2nnn/8/8/8/8/8/8/NNN2NNN b 00";

        AI.main.Init(board);
        Defs.gameState = GameState.Thinking;
        //GUIPlay.main.Init(board);
       //GUITime.main.Init();

        
    }

    public void Init()
    {
        board = new Board(Fen == "" ? FEN.Default : Fen);

#if UNITY_5_3
        if (SceneManager.GetActiveScene().name.Equals("Game"))
#else
        if (Application.loadedLevelName.Equals("Game"))
#endif
            FEN.Default = "nnn2nnn/8/8/8/8/8/8/NNN2NNN b 00";

        SoundController.main.SoundPlay("clock");
        Zobrist.Init();
        AI.main.Init(board);
        Defs.gameState = GameState.Thinking;
        GUIPlay.main.Init(board);
        GUITime.main.Init();

        
    }
}
