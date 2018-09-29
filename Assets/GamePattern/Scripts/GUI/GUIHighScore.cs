using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GUIHighScore : MonoBehaviour {

    public static GUIHighScore main;
    public struct Player
    {
        public string name;
        public int time;

        public void Print()
        {
            Debug.Log("Name: " + name + " Time: " + time);
        }
    }


    const int MaxPlayer = 6;
    const int Infinited = 99999;

    public Text[] txtNamePlayers;
    public Text[] txtScorePlayers;
    private Player[] players = new Player[MaxPlayer + 1];

    void Awake()
    {
        main = this;
    }
    void OnEnable()
    {
        Init();

        SetHighScore();
    }

    void Init()
    {
        if (PlayerPrefs.GetInt("highscoreinit") == 0)
        {
            for (int i = 0; i <= MaxPlayer; i++)
            {
                players[i].name = "";
                players[i].time = Infinited;
            }
            PlayerPrefs.SetInt("highscoreinit", 1);

        }
        else
        {
            GetValue();
        }
    }

    public void ResetHighScore()
    {
        for (int i = 0; i <= MaxPlayer; i++)
        {
            players[i].name = "";
            players[i].time = Infinited;
        }
        SetHighScore();
        SetValue();
    }

    void GetValue()
    {
        for (int i = 0; i < MaxPlayer; i++)
        {
            players[i].name = PlayerPrefs.GetString("highscorename" + i.ToString());
            players[i].time = PlayerPrefs.GetInt("highscoretime" + i.ToString());
            players[i].Print();
        }
    }

    void SetValue()
    {
        for (int i = 0; i < MaxPlayer; i++)
        {
            players[i].Print();
            PlayerPrefs.SetString("highscorename" + i.ToString(), players[i].name);
            PlayerPrefs.SetInt("highscoretime" + i.ToString(), players[i].time);
        }
    }

    void SetHighScore()
    {
        for (int i = 0; i < MaxPlayer; i++)
        {
            txtNamePlayers[i].text = players[i].name;
            if (players[i].time != Infinited && players[i].time != 0)
                txtScorePlayers[i].text = Defs.ConvertNumToTime(players[i].time);
            else
                txtScorePlayers[i].text = "";
        }
    }

    public void UpdateHighScore(string name, int time)
    {
        players[MaxPlayer].name = name;
        players[MaxPlayer].time = time;

        Init();

        for (int i = 0; i < MaxPlayer; i++)
        {
            for (int j = i + 1; j <= MaxPlayer; j++)
            {
                if (players[i].time > players[j].time)
                {
                    Player temp = players[i];
                    players[i] = players[j];
                    players[j] = temp;
                }
            }
        }

        SetValue();
    }
}
