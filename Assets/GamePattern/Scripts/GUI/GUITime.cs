using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Text))]
public class GUITime : MonoBehaviour {

    public static GUITime main;

    [HideInInspector]
    public bool isUpdateLogic = false;

    public Image imgRedTime, imgBlueTime;

    private int thinkingTime = 0;

    [HideInInspector]
    public float countTime = 0;
    [HideInInspector]
    public float countTotalTime = 0;

    private Text txtTime;

    private int turn = 0;
    void Awake()
    {
        main = this;

        txtTime = gameObject.GetComponent<Text>();
        InitImgTime(imgRedTime);
        InitImgTime(imgBlueTime);
    }

    public void Init()
    {
        isUpdateLogic = true;
        thinkingTime = Defs.ThinkingTime;
        this.turn = PlayerPrefs.GetInt("gofirst");
        SwitchTurn(this.turn);
        countTotalTime = 0;

    }

    void Update()
    {
        if (isUpdateLogic && SceneManager.GetActiveScene().name.Equals("Game"))
        {
            var deltaTime = Time.deltaTime;
            if (Defs.gameState == GameState.Thinking)
                countTime += deltaTime;
            countTotalTime += deltaTime;
            SetTime(Mathf.FloorToInt(countTotalTime));

            if (turn == 0)
            {
                
                UpdateImgTime(imgRedTime);
            }
            else if (turn == 1)
            {
                UpdateImgTime(imgBlueTime);
            }
            if (countTime > thinkingTime)
            {
                isUpdateLogic = false;
                // Do something here
                if (turn == 0)
                {
                    
                    GUIEvents.main.RunForward("Win");
                    GUIPlay.main._txtTimeWin.text = Defs.ConvertNumToTime(Mathf.FloorToInt(countTotalTime));
                    if (Defs.typePlay == TypePlay.PVP)
                    {
                        GUIPlay.main._txtTextWin.text = "BLUE WIN";
                    }
                    else if(Defs.typePlay == TypePlay.PVC)
                    {
                        GUIPlay.main._txtTextWin.text = "YOU WIN";
                    }
                }
                else if (turn == 1)
                {
                    if (Defs.typePlay == TypePlay.PVC)
                    {
                        GUIPlay.main._txtTextWin.text = "YOU LOSE";
                        GUIEvents.main.RunForward("Lose");
                    }
                    else if (Defs.typePlay == TypePlay.PVP)
                    {
                        GUIPlay.main._txtTextWin.text = "RED WIN";
                        GUIEvents.main.RunForward("Win");
                        GUIPlay.main._txtTimeLose.text = Defs.ConvertNumToTime(Mathf.FloorToInt(countTotalTime));
                    }
                }

                SoundController.main.PauseSound();
            }
        }

    }

    void UpdateImgTime(Image imgTime){
        
        imgTime.fillAmount = Mathf.Max(1 - countTime/thinkingTime, 0);
        imgTime.color = Color.Lerp(Defs.blue, Color.red, countTime / thinkingTime);
        //
    }

    void SetTime(int time)
    {
        txtTime.text = Defs.ConvertNumToTime(time);
    }

    public void SwitchTurn(int turn)
    {
        //print("turn: " + turn);
        this.turn = turn;
        countTime = 0;

        imgBlueTime.fillAmount = 1;
        imgRedTime.fillAmount = 1;
        imgBlueTime.color = Defs.blue;
        imgRedTime.color = Defs.blue;

        if(turn == 0) // black
        {
            
        }
        else if (turn == 1) // white
        {

        }
    }

    void InitImgTime(Image imgTime)
    {
        imgTime.type = Image.Type.Filled;
        imgTime.fillMethod = Image.FillMethod.Radial360;
        imgTime.fillOrigin = 2;
        imgTime.fillClockwise = false;
        imgTime.color = Defs.blue;
    }

    public void PauseTime()
    {
        isUpdateLogic = false;
        SoundController.main.PauseSound();
    }

    public void ResumeTime()
    {
        isUpdateLogic = true;
        SoundController.main.RunSound();
    }


}
